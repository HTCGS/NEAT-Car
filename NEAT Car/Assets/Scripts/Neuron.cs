using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public static Func<float, float> Sigmoid = (x) => { return 1 / (1 + Mathf.Pow((float)Math.E, -x)); };

    public Func<float, float> Function;

    public List<Connection> Connections;

    public List<float> InputValues;

    public int InputsSum;

    public float Value;

    public Neuron()
    {
        this.InputValues = new List<float>();
        this.Connections = new List<Connection>();
    }

    public Neuron(Func<float, float> func) : this()
    {
        this.Function = func;
    }

    public void CalculateNeuronValue()
    {
        this.Value = 0;
        if (InputValues.Count != InputsSum) return;
        float value = 0;
        float inputSum = 0;
        foreach (var item in InputValues)
        {
            inputSum += item;
        }
        value = Function(inputSum);
        this.Value = value;
    }

    public void SendValues()
    {
        foreach (var conn in Connections)
        {
            conn.SendValue(this.Value);
        }
        InputValues.Clear();
    }

    public void ReciveValue(float value)
    {
        InputValues.Add(value);
    }

    public void AddConnection(Neuron target)
    {
        float weight = UnityEngine.Random.Range(-6f, 6f);
        Connections.Add(new Connection(target, weight));
    }
}
