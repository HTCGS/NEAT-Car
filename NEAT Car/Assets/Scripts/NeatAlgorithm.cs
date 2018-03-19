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

        List<List<Connection>> connectionsList = new List<List<Connection>>();
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
            List<List<Connection>> nextPopulation = new List<List<Connection>>();
            int pos1 = 0;
            int pos2 = 0;
            do
            {
                List<Connection> child1 = new List<Connection>();
                List<Connection> child2 = new List<Connection>();
                Connection conn = null;
                if(pos1 < conn1.Count && pos2 < conn2.Count)
                {

                }
                else if( pos1 < conn1.Count)
                {

                }
                else if(pos2 < conn2.Count)
                {

                }
            } while (pos1 != conn1.Count && pos2 != conn2.Count);


        }
    }

    public static void Mutation()
    {

    }
}