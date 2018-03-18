﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [HideInInspector]
    public NeuroNet Control;

    public float Fitness;

    public float Speed = 1;
    public float Rotation = 1;

    [Space(10)]
    public float SensorRadius = 1;
    public GameObject LeftSensor;
    public GameObject MiddleSensor;
    public GameObject RightSensor;
    public LayerMask Mask;

    [HideInInspector]
    public bool run = true;

    private List<float> input;
    private List<float> output;

    private float time;
    private Vector3 lastPos;
    private float fitParam = 1;

    void Start()
    {
        input = new List<float>();
        output = new List<float>() { 0, 0, 1 };
        lastPos = this.transform.position;
        Control = new NeuroNet(4, 3);
        Control.GenerateDefaultNet(5);
    }

    void Update()
    {
        if (run)
        {
            input = SensorData();
            input.Add(output[2] * Speed);
            output = Control.Run(input);

            //Debug.Log(input[0] + "-" + input[1] + "-" + input[2]);
            this.transform.Rotate(this.transform.up * -Rotation * output[0] * Time.deltaTime, Space.Self);
            this.transform.Rotate(this.transform.up * Rotation * output[1] * Time.deltaTime, Space.Self);
            this.transform.position += this.transform.forward * Speed * output[2] * Time.deltaTime;

            Fitness += (this.transform.position - lastPos).magnitude * fitParam;
            lastPos = this.transform.position;
        }
    }

    private void OnEnable()
    {
        Fitness = 0;
        run = true;
        lastPos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Start") run = false;
        else fitParam = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Start") fitParam = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Start")
        {
            fitParam = 1;
        }
    }

    private List<float> SensorData()
    {
        Debug.DrawLine(LeftSensor.transform.position, LeftSensor.transform.position + LeftSensor.transform.forward * SensorRadius, Color.green);
        Debug.DrawLine(MiddleSensor.transform.position, MiddleSensor.transform.position + MiddleSensor.transform.forward * SensorRadius, Color.green);
        Debug.DrawLine(RightSensor.transform.position, RightSensor.transform.position + RightSensor.transform.forward * SensorRadius, Color.green);

        List<float> data = new List<float>();
        RaycastHit raycastHit;
        if (Physics.Raycast(LeftSensor.transform.position, LeftSensor.transform.forward, out raycastHit, SensorRadius, Mask))
        {

            float distanse = (raycastHit.point - LeftSensor.transform.position).magnitude;
            data.Add(distanse);
        }
        else data.Add(0);
        if (Physics.Raycast(MiddleSensor.transform.position, MiddleSensor.transform.forward, out raycastHit, SensorRadius, Mask))
        {
            float distanse = (raycastHit.point - MiddleSensor.transform.position).magnitude;
            data.Add(distanse);
        }
        else data.Add(0);
        if (Physics.Raycast(RightSensor.transform.position, RightSensor.transform.forward, out raycastHit, SensorRadius, Mask))
        {
            float distanse = (raycastHit.point - RightSensor.transform.position).magnitude;
            data.Add(distanse);
        }
        else data.Add(0);
        return data;
    }
}
