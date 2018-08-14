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

    public static void Mutation(List<float> weights)
    {
        int index = Random.Range(0, weights.Count);
        float mutationRate = Mathf.Lerp(0.1f, 0.01f, Environment.Generation / 200f);
        float mutation = 12f * mutationRate;
        if (Random.Range(0, 100) < 50) weights[index] += mutation;
        else weights[index] -= mutation;
    }
}
