using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public static Func<float, float> Sigmoid = (x) => { return 1 / (1 + Mathf.Pow((float)Math.E, -x)); };

    public static Func<float, float> Input = (x) => x;

    public Func<float, float> Function;

    public List<Connection> Connections;

    public List<float> InputValues;

    public int InputsSum;

    public float Value;

    public int Index;

    public Neuron()
    {
        this.InputValues = new List<float>();
        this.Connections = new List<Connection>();
    }

    public Neuron(Func<float, float> func) : this()
    {
        this.Function = func;
    }

    public Neuron(Func<float, float> func, int index) : this(func)
    {
        this.Index = index;
    }

    public bool CalculateNeuronValue()
    {
        this.Value = 0;
        if (InputValues.Count != InputsSum) return false;
        float value = 0;
        float inputSum = 0;
        foreach (var item in InputValues)
        {
            inputSum += item;
        }
        value = Function(inputSum);
        InputValues.Clear();
        this.Value = value;
        return true;
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
        AddConnection(target, 0);
    }

    public void AddConnection(Neuron target, int innovation)
    {
        float weight = UnityEngine.Random.Range(-6f, 6f);
        Connection connection = new Connection(target, weight);
        connection.Innovation = innovation;
        connection.Source = this;
        Connections.Add(connection);
    }

    public void AddConnection(Neuron target, Connection connectionData)
    {
        Connection connection = new Connection(target, connectionData.Weight);
        connection.Innovation = connectionData.Innovation;
        connection.IsActive = connectionData.IsActive;
        if (!connection.IsActive) connection.Deactivate();
        connection.Source = this;
        Connections.Add(connection);
    }

    public Neuron Copy()
    {
        Neuron neuron = new Neuron();
        neuron.Function = Function;
        neuron.Index = Index;
        return neuron;
    }
}
