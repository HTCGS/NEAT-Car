using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{   
    [HideInInspector]
    public NeuroNet Control;

    public float Fitness;

    public float Speed = 1;
    public float Rotation = 1;

    void Start ()
    {
        Control = new NeuroNet(4, 3);
        Control.GenerateDefaultNet(4);
    }
	
	void Update ()
    {
        this.transform.Rotate(this.transform.up * -Rotation * Time.deltaTime, Space.Self);
        this.transform.Rotate(this.transform.up * Rotation * Time.deltaTime, Space.Self);
        this.transform.position += this.transform.forward * Speed * Time.deltaTime;
    }
}
