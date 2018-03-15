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
        Net.OrderBy(c => c.Fitness);
    }

    public static void Selection()
    {
        List<List<Connection>> reproduction = new List<List<Connection>>();
        int keepedSum = Net.Count / 2;
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
            if (keeped == keepedSum) break;
        }

        List<List<float>> weightsList = new List<List<float>>();
        for (int i = 0; i < keepedSum; i++)
        {
            int index = Random.Range(0, reproduction.Count);
            List<Connection> conn1 = reproduction[index];
            reproduction.RemoveAt(index);
            List<Connection> conn2 = reproduction[index];
            reproduction.RemoveAt(index);

            List<float> weights1 = new List<float>();
            List<float> weights2 = new List<float>();
            int pos = conn1.Count / 2;
            for (int j = 0; j < conn1.Count; j++)
            {
                if(j < pos)
                {
                    weights1.Add(conn1[j].Weight);
                    weights2.Add(conn2[j].Weight);
                }
                else
                {
                    weights1.Add(conn2[j].Weight);
                    weights2.Add(conn1[j].Weight);
                }
            }
            if (Random.Range(0, 100) < 5) Mutation(weights1);
            if (Random.Range(0, 100) < 5) Mutation(weights2);
            weightsList.Add(weights1);
            weightsList.Add(weights2);
        }

        for (int i = 0; i < Net.Count; i++)
        {
            List<Connection> conn = Net[i].Control.ToConnectionList();
            List<float> weights = weightsList[i];
            for (int j = 0; j < conn.Count; j++)
            {
                conn[j].Weight = weights[j];
            }
        }
    }

    public static void Mutation(List<float> weights)
    {
        int index = Random.Range(0, weights.Count);
        weights[index] = Random.Range(-6f, 6f);
    }
}
