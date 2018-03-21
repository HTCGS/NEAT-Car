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

    public static int Generation = 1;
    public Text GenText;
    public Text FitnessText;

    public GameObject Visual;

    void Start ()
    {
        ObjectPool.InitializeObjects(CarPrefab, Population);
        ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);
        ObjectPool.EnableObjects();
        //GeneticAlgorithm.InitializePopulation(ObjectPool.Objects);
        NeatAlgorithm.InitializePopulation(ObjectPool.Objects);
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
        //GeneticAlgorithm.SortPopulation();
        //GeneticAlgorithm.Selection();
        NeatAlgorithm.SortPopulation();
        NetVisualization netVisualization = Visual.GetComponent<NetVisualization>();
        netVisualization.Visualize(NeatAlgorithm.Net[0].Control);
        float maxZ = netVisualization.MaxZPosition();
        float middle = (Visual.transform.position - new Vector3(Visual.transform.position.x, Visual.transform.position.y, maxZ)).magnitude;
        middle /= 2;
        Visual.transform.position = new Vector3(Visual.transform.position.x, Visual.transform.position.y, this.transform.position.z - middle);
        netVisualization.Visualize(NeatAlgorithm.Net[0].Control);
        NeatAlgorithm.Selection();
        ObjectPool.DisableObjects();
        ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);
        Generation++;
        GenText.text = "Generation " + Generation;
        //FitnessText.text = "Max fitness " + GeneticAlgorithm.Net[0].Fitness;
        FitnessText.text = "Max fitness " + NeatAlgorithm.Net[0].Fitness;
        ObjectPool.EnableObjects();
    }
}
