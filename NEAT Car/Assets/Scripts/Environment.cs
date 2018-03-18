using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    public static Environment Instance;
    public GameObject CarPrefab;

    public GameObject StartPosition;

    public int Population;

    public float MaxTime;

    private float runTime;
    private float globalTime;

    private int Generation = 1;
    public Text GenText;
    public Text FitnessText;

    void Start ()
    {
        ObjectPool.InitializeObjects(CarPrefab, Population);
        ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);
        ObjectPool.EnableObjects();
        GeneticAlgorithm.InitializePopulation(ObjectPool.Objects);
        Instance = this;
	}
	
	void Update ()
    {
        runTime += Time.deltaTime;
        globalTime += Time.deltaTime;
        if (runTime >= 1)
        {
            if (!ObjectPool.IsRunable())
            {
                NewGeneration();
                globalTime = 0;
            }
            runTime = 0;
        }
        if (globalTime >= MaxTime)
        {
            ObjectPool.DisableCar();
            NewGeneration();
            globalTime = 0;
            runTime = 0;
        }
    }

    private void NewGeneration()
    {
        GeneticAlgorithm.SortPopulation();
        GeneticAlgorithm.Selection();
        ObjectPool.DisableObjects();
        ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);
        Generation++;
        GenText.text = "Generation " + Generation;
        FitnessText.text = "Max fitness " + GeneticAlgorithm.Net[0].Fitness;
        ObjectPool.EnableObjects();
    }
}
