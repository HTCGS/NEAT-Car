using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment Instance;
    public GameObject CarPrefab;

    public GameObject StartPosition;

    public int Population;

    public float MaxTime;

    public float Time;

	void Start ()
    {
        ObjectPool.InitializeObjects(CarPrefab, Population);
        ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);
        ObjectPool.EnableObjects();
        Instance = this;
	}
	
	void Update ()
    {
		
	}
}
