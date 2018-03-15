using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject CarPrefab;

    public GameObject StartPosition;

    public int Population;

    public float MaxTime;

    public float Time;

	void Start ()
    {
        //ObjectPool.InitializeObjects(CarPrefab, Population);
        //ObjectPool.SetDefaultPosition(StartPosition.transform.position, StartPosition.transform.rotation);

        float chance = 100;
        int killed = 0;
        for (int i = 0; i < 100; i++)
        {
            if (Random.Range(0, 100) < chance)
            {
                Debug.Log(i);
                killed++;
                chance--;
            }
            if (killed == 50) break;
        }
	}
	
	void Update ()
    {
		
	}
}
