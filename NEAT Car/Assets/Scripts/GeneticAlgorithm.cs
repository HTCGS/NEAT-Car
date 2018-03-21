using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GeneticAlgorithm
{
    public static List<Car> Net;

    static GeneticAlgorithm()
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
        List<List<float>> weightsList = new List<List<float>>();

        List<Connection> bestNet = Net[0].Control.ToConnectionList();
        List<float> weights = new List<float>();
        for (int i = 0; i < bestNet.Count; i++)
        {
            weights.Add(bestNet[i].Weight);
        }
        weightsList.Add(weights);

        int heirSum = (int)(Net.Count * 0.70f);
        for (int i = 1; i < heirSum; i++)
        {
            weights = new List<float>();
            for (int j = 0; j < bestNet.Count; j++)
            {
                weights.Add(bestNet[j].Weight);
            }
            Mutation(weights);
            weightsList.Add(weights);
        }

        for (int i = heirSum; i < Net.Count; i++)
        {
            weights = new List<float>();
            for (int j = 0; j < bestNet.Count; j++)
            {
                weights.Add(Random.Range(-6f, 6f));
            }
            weightsList.Add(weights);
        }

        for (int i = 0; i < Net.Count; i++)
        {
            List<Connection> conn = Net[i].Control.ToConnectionList();
            weights = weightsList[i];
            for (int j = 0; j < conn.Count; j++)
            {
                conn[j].Weight = weights[j];
            }
        }
    }

    //public static void Selection()
    //{
    //    List<List<Connection>> reproduction = new List<List<Connection>>();
    //    int keepedSum = Net.Count / 2;
    //    float chance = 100;
    //    int keeped = 0;
    //    foreach (var item in Net)
    //    {
    //        if (Random.Range(0, 100) < chance)
    //        {
    //            reproduction.Add(item.Control.ToConnectionList());
    //            keeped++;
    //            chance--;
    //        }
    //        if (keeped == keepedSum) break;
    //    }

    //    List<List<float>> weightsList = new List<List<float>>();
    //    for (int i = 0; i < keepedSum / 2; i++)
    //    {
    //        int index = Random.Range(0, reproduction.Count);
    //        List<Connection> conn1 = reproduction[index];
    //        reproduction.RemoveAt(index);
    //        index = Random.Range(0, reproduction.Count);
    //        List<Connection> conn2 = reproduction[index];
    //        reproduction.RemoveAt(index);

    //        List<float> weights1 = new List<float>();
    //        List<float> weights2 = new List<float>();
    //        List<float> weights3 = new List<float>();
    //        List<float> weights4 = new List<float>();
    //        int pos = conn1.Count / 2;
    //        //int pos = Random.Range(0, conn1.Count);
    //        for (int j = 0; j < conn1.Count; j++)
    //        {
    //            if (j < pos)
    //            {
    //                weights1.Add(conn1[j].Weight);
    //                weights2.Add(conn2[j].Weight);
    //                //weights3.Add(Random.Range(-1f, 1f));
    //                //weights4.Add(Random.Range(-1f, 1f));
    //            }
    //            else
    //            {
    //                weights1.Add(conn2[j].Weight);
    //                weights2.Add(conn1[j].Weight);
    //                //weights3.Add(Random.Range(-1f, 1f));
    //                //weights4.Add(Random.Range(-1f, 1f));
    //            }
    //            weights3.Add(Random.Range(-6f, 6f));
    //            weights4.Add(Random.Range(-6f, 6f));
    //        }
    //        if (Random.Range(0, 100) < 20) Mutation(weights1);
    //        if (Random.Range(0, 100) < 20) Mutation(weights2);
    //        weightsList.Add(weights1);
    //        weightsList.Add(weights2);
    //        weightsList.Add(weights3);
    //        weightsList.Add(weights4);
    //    }

    //    for (int i = 0; i < Net.Count; i++)
    //    {
    //        List<Connection> conn = Net[i].Control.ToConnectionList();
    //        List<float> weights = weightsList[i];
    //        for (int j = 0; j < conn.Count; j++)
    //        {
    //            conn[j].Weight = weights[j];
    //        }
    //    }
    //}

    public static void Mutation(List<float> weights)
    {
        int index = Random.Range(0, weights.Count);
        float mutationRate = Mathf.Lerp(0.1f, 0.01f, Environment.Generation / 1000f);
        float mutation = 12f * mutationRate;
        if (Random.Range(0, 100) < 50) weights[index] += mutation;
        else weights[index] -= mutation;
        //if (Random.Range(0, 100) < 50) weights[index] += weights[index] * 0.1f;
        //else weights[index] -= weights[index] * 0.1f;

    }
}
