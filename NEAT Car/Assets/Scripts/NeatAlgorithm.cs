using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NeatAlgorithm
{
    public static List<Car> Net;

    static NeatAlgorithm()
    {
        Net = new List<Car>();
    }

    public static void InitializePopulation(List<GameObject> gameObjects)
    {
        foreach (var item in gameObjects)
        {
            Car car = item.GetComponent<Car>();
            Net.Add(car);
        }
    }

    public static void SortPopulation()
    {
        Net = Net.OrderByDescending(c => c.Fitness).ToList();
    }

    public static void Selection()
    {
        List<List<Connection>> reproduction = new List<List<Connection>>();
        int repSum = Net.Count / 2;
        float chance = 100;
        int keeped = 0;
        foreach (var item in Net)
        {
            if (Random.Range(0, 100) < chance)
            {
                reproduction.Add(item.Control.ToConnectionList());
                keeped++;
                chance--;
            }
            if (keeped == repSum) break;
        }

        List<List<Connection>> nextPopulation = new List<List<Connection>>();
        for (int i = 0; i < repSum / 2; i++)
        {
            int index = Random.Range(0, reproduction.Count);
            List<Connection> conn1 = reproduction[index];
            reproduction.RemoveAt(index);
            index = Random.Range(0, reproduction.Count);
            List<Connection> conn2 = reproduction[index];
            reproduction.RemoveAt(index);

            conn1 = conn1.OrderBy(inn => inn.Innovation).ToList();
            conn2 = conn2.OrderBy(inn => inn.Innovation).ToList();
            nextPopulation.Add(CopyConnections(conn1));
            nextPopulation.Add(CopyConnections(conn2));
            List<Connection> child1 = new List<Connection>();
            List<Connection> child2 = new List<Connection>();
            int pos1 = 0;
            int pos2 = 0;
            do
            {
                if (pos1 < conn1.Count && pos2 < conn2.Count)
                {
                    if (conn1[pos1].Innovation == conn2[pos2].Innovation)
                    {
                        if (!conn1[pos1].IsActive)
                        {
                            child1.Add(conn1[pos1].Copy());
                            child2.Add(conn1[pos1].Copy());
                        }
                        else if (!conn2[pos2].IsActive)
                        {
                            child1.Add(conn2[pos2].Copy());
                            child2.Add(conn2[pos2].Copy());
                        }
                        else
                        {
                            if (Random.Range(0, 100) < 50)
                            {
                                child1.Add(conn1[pos1].Copy());
                                child2.Add(conn2[pos2].Copy());
                            }
                            else
                            {
                                child1.Add(conn2[pos2].Copy());
                                child2.Add(conn1[pos1].Copy());
                            }
                        }
                        pos1++;
                        pos2++;
                    }
                    else if (conn1[pos1].Innovation < conn2[pos2].Innovation)
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            child1.Add(conn1[pos1].Copy());
                            child2.Add(conn1[pos1].Copy());
                        }
                        pos1++;
                    }
                    else
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            child1.Add(conn2[pos2].Copy());
                            child2.Add(conn2[pos2].Copy());
                        }
                        pos2++;
                    }
                }
                else if (pos1 < conn1.Count)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        child1.Add(conn1[pos1].Copy());
                        child2.Add(conn1[pos1].Copy());
                    }
                    pos1++;
                }
                else if (pos2 < conn2.Count)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        child1.Add(conn2[pos2].Copy());
                        child2.Add(conn2[pos2].Copy());
                    }
                    pos2++;
                }
            } while (pos1 != conn1.Count && pos2 != conn2.Count);
            nextPopulation.Add(child1);
            nextPopulation.Add(child2);
        }

        for (int i = 0; i < Net.Count; i++)
        {
            Net[i].Control.GenerateFromConnections(nextPopulation[i]);
        }
        Mutation();
    }

    public static void Mutation()
    {
        foreach (var item in Net)
        {
            if (Random.Range(1, 100) < 10)
            {
                int mutationType = Random.Range(0, 100);
                if (mutationType < 80)
                {
                    WeightMutation(item.Control);
                }
                else if (mutationType >= 80 && mutationType < 90)
                {
                    List<Connection> connections = item.Control.ToConnectionList();
                    if (connections.Count == 0)
                    {
                        AddConnectionMutation(item.Control);
                    }
                    else
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            if (item.Control.IsfreeConnection())
                            {
                                AddConnectionMutation(item.Control);
                            }
                            else
                            {
                                OnOffConnectionMutation(item.Control);
                            }
                        }
                        else
                        {
                            OnOffConnectionMutation(item.Control);
                        }
                    }
                }
                else if (mutationType >= 90 && mutationType < 100)
                {
                    AddNodeMutation(item.Control);
                }
            }
        }
    }

    public static void WeightMutation(EvolveNeuroNet neuroNet)
    {
        List<Connection> connections = neuroNet.ToConnectionList();
        if (connections.Count != 0)
        {
            int index = Random.Range(0, connections.Count);
            float mutationRate = 0.1f - (0.005f * (Environment.Generation / 5));
            float mutation = 12f * mutationRate;
            if (Random.Range(0, 100) < 50) connections[index].Weight += mutation;
            else connections[index].Weight -= mutation;
        }
    }

    public static void AddConnectionMutation(EvolveNeuroNet neuroNet)
    {
        List<Neuron> neurons = neuroNet.ToNeuronList();
        int from = 0;
        int to = 0;

        bool found = false;
        do
        {
            from = Random.Range(0, neurons.Count);
            if (neuroNet.IsOutput(neurons[from])) found = false;
            else found = true;

            to = Random.Range(0, neurons.Count);
            if (to == from) found = false;
            else
            {
                if (neuroNet.IsInput(neurons[to])) found = false;
                else found = true;
                foreach (var conn in neurons[from].Connections)
                {
                    if (conn.Target.Index == neurons[to].Index) found = false;
                }
            }
        } while (!found);
        neuroNet.AddConnection(neurons[from], neurons[to]);
        RebuildNet(neuroNet);
    }

    public static void OnOffConnectionMutation(EvolveNeuroNet neuroNet)
    {
        List<Connection> connections = neuroNet.ToConnectionList();
        int index = Random.Range(0, connections.Count);
        if (connections[index].IsActive) connections[index].Deactivate();
        else connections[index].Activate();
    }

    public static void AddNodeMutation(EvolveNeuroNet neuroNet)
    {
        List<Connection> connections = neuroNet.ToConnectionList();
        if (connections.Count != 0)
        {
            bool found = false;
            foreach (var conn in connections)
            {
                if (conn.IsActive) found = true;
            }
            if (found)
            {
                int index = 0;
                found = false;
                do
                {
                    index = Random.Range(0, connections.Count);
                    if (connections[index].IsActive) found = true;
                } while (!found);

                connections[index].Deactivate();
                Neuron neuron = neuroNet.AddNeuron();
                neuroNet.AddConnection(connections[index].Source, neuron);
                neuroNet.AddConnection(neuron, connections[index].Target);
            }
            RebuildNet(neuroNet);
        }
    }

    private static void RebuildNet(EvolveNeuroNet neuroNet)
    {
        List<Connection> connCopy = CopyConnections(neuroNet.ToConnectionList());
        neuroNet.GenerateFromConnections(connCopy);
    }

    private static List<Connection> CopyConnections(List<Connection> connections)
    {
        List<Connection> copy = new List<Connection>();
        foreach (var conn in connections)
        {
            copy.Add(conn.Copy());
        }
        return copy;
    }
}