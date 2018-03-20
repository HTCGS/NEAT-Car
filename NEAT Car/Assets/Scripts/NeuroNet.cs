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
            Neuron neuron = new Neuron(Neuron.Input);
            neuron.InputsSum = 1;
            this.Input.Add(neuron);
        }
        for (int i = 0; i < output; i++)
        {
            Neuron neuron = new Neuron(Neuron.Sigmoid);
            this.Output.Add(neuron);
        }
    }

    public virtual void GenerateDefaultNet()
    {
        foreach (var inNode in Input)
        {
            foreach (var outNode in Output)
            {
                inNode.AddConnection(outNode);
            }
        }
    }

    public virtual void GenerateDefaultNet(params int[] layers)
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

    public List<Neuron> ToNeuronList()
    {
        List<Neuron> neuronList = new List<Neuron>();

        foreach (var node in Input)
        {
            neuronList.Add(node);
        }

        foreach (var nodeList in HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                neuronList.Add(node);
            }
        }

        foreach (var node in Output)
        {
            neuronList.Add(node);
        }
        return neuronList;
    }

    public Neuron FindNeuron(int index)
    {
        foreach (var node in Input)
        {
            if (node.Index == index) return node;
        }

        foreach (var node in Output)
        {
            if (node.Index == index) return node;
        }

        for (int i = 0; i < HiddenLayer.Count; i++)
        {
            foreach (var node in HiddenLayer[i])
            {
                if (node.Index == index) return node;
            }
        }
        return null;
    }

    public List<Neuron> NeuronLayer(Neuron neuron)
    {
        foreach (var node in Input)
        {
            if (node == neuron) return Input;
        }

        foreach (var node in Output)
        {
            if (node == neuron) return Output;
        }

        for (int i = 0; i < HiddenLayer.Count; i++)
        {
            foreach (var node in HiddenLayer[i])
            {
                if (node == neuron) return HiddenLayer[i];
            }
        }
        return null;
    }

    public List<Neuron> PreviousLayer(Neuron neuron)
    {
        foreach (var node in Input)
        {
            if (node == neuron) return null;
        }

        foreach (var node in Output)
        {
            if (node == neuron)
            {
                if (HiddenLayer.Count != 0)
                {
                    return HiddenLayer[HiddenLayer.Count - 1];
                }
                else return Input;
            }
        }

        for (int i = 0; i < HiddenLayer.Count; i++)
        {
            foreach (var node in HiddenLayer[i])
            {
                if (node == neuron)
                {
                    if (i != 0) return HiddenLayer[i - 1];
                    else return Input;
                }
            }
        }
        return null;
    }

    public bool IsInput(List<Neuron> neuronList)
    {
        if (neuronList.Count != Input.Count) return false;

        for (int i = 0; i < Input.Count; i++)
        {
            if (neuronList[i] != Input[i]) return false;
        }
        return true;
    }

    public bool IsInput(Neuron neuron)
    {
        foreach (var node in Input)
        {
            if (node == neuron) return true;
        }
        return false;
    }

    public bool IsOutput(List<Neuron> neuronList)
    {
        if (neuronList.Count != Output.Count) return false;

        for (int i = 0; i < Output.Count; i++)
        {
            if (neuronList[i] != Output[i]) return false;
        }
        return true;
    }

    public bool IsOutput(Neuron neuron)
    {
        foreach (var node in Output)
        {
            if (node == neuron) return true;
        }
        return false;
    }

    public void Clear()
    {
        foreach (var node in Input)
        {
            node.Connections.Clear();
        }
        foreach (var node in Output)
        {
            node.Connections.Clear();
            node.InputsSum = 0;
        }
        HiddenLayer.Clear();
    }
}
