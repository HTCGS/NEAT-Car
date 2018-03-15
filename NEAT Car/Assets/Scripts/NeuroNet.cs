using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroNet
{
    public List<Neuron> Input;
    public List<Neuron> Output;

    public List<List<Neuron>> HiddenLayer;

    public NeuroNet()
    {
        this.Input = new List<Neuron>();
        this.Output = new List<Neuron>();
        this.HiddenLayer = new List<List<Neuron>>();
    }

    public NeuroNet(int input, int output) : this()
    {
        for (int i = 0; i < input; i++)
        {
            Neuron neuron = new Neuron(Neuron.Sigmoid);
            neuron.InputsSum = 1;
            this.Input.Add(neuron);
        }
        for (int i = 0; i < output; i++)
        {
            Neuron neuron = new Neuron(Neuron.Sigmoid);
            neuron.InputsSum = 1;
            this.Output.Add(neuron);
        }
    }

    public void GenerateDefaultNet()
    {
        foreach (var inNode in Input)
        {
            foreach (var outNode in Output)
            {
                inNode.AddConnection(outNode);
            }
        }
    }

    public void GenerateDefaultNet(params int[] layers)
    {
        List<Neuron> layer = new List<Neuron>();
        for (int i = 0; i < layers[0]; i++)
        {
            Neuron neuron = new Neuron(Neuron.Sigmoid);
            layer.Add(neuron);
            foreach (var node in Input)
            {
                node.AddConnection(neuron);
            }
        }
        this.HiddenLayer.Add(layer);
        for (int i = 1; i < layers.Length; i++)
        {
            layer = new List<Neuron>();
            for (int j = 0; j < layers[i]; j++)
            {
                Neuron neuron = new Neuron(Neuron.Sigmoid);
                layer.Add(neuron);
                foreach (var node in HiddenLayer[i - 1])
                {
                    node.AddConnection(neuron);
                }
            }
            this.HiddenLayer.Add(layer);
        }
        if (layers.Length != 1)
        {
            this.HiddenLayer.Add(layer);
            layer = HiddenLayer[HiddenLayer.Count - 1];
        }
        foreach (var node in layer)
        {
            foreach (var outNode in Output)
            {
                node.AddConnection(outNode);
            }
        }
    }

    public List<Connection> ToConnectionList()
    {
        List<Connection> conn = new List<Connection>();
        foreach (var node in Input)
        {
            foreach (var nodeCon in node.Connections)
            {
                conn.Add(nodeCon);
            }
        }
        foreach (var nodeList in HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                foreach (var nodeCon in node.Connections)
                {
                    conn.Add(nodeCon);
                }
            }
        }
        return conn;
    }

    public List<float> Run(List<float> input)
    {
        List<float> output = new List<float>();
        int index = 0;
        foreach (var node in Input)
        {
            node.ReciveValue(input[index]);
            node.CalculateNeuronValue();
            node.SendValues();
            index++;
        }
        foreach (var nodeList in HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                node.CalculateNeuronValue();
                node.SendValues();
            }
        }
        foreach (var node in Output)
        {
            node.CalculateNeuronValue();
            output.Add(node.Value);
        }
        return output;
    }

}
