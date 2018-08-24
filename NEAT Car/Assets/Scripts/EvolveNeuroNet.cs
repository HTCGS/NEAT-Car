using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EvolveNeuroNet : NeuroNet
{
    public int Innovation;
    public int NodeIndex;

    public EvolveNeuroNet() : base()
    {
    }

    public EvolveNeuroNet(int input, int output) : base(input, output)
    {
        foreach (var node in Input)
        {
            node.Index = NodeIndex;
            NodeIndex++;
        }
        foreach (var node in Output)
        {
            node.Index = NodeIndex;
            NodeIndex++;
        }
    }

    public override void GenerateDefaultNet()
    {
        base.GenerateDefaultNet();
        SetBiasNodesIndex();
        SetInnovations();
    }

    public override void GenerateDefaultNet(params int[] layers)
    {
        base.GenerateDefaultNet(layers);
        SetHiddenNodesIndex();
        SetBiasNodesIndex();
        SetInnovations();
    }

    public void GenerateFromConnections(List<Connection> connList)
    {
        Clear();
        foreach (var conn in connList)
        {
            Neuron sourceNeuron = FindNeuron(conn.Source.Index);
            Neuron targetNeuron = FindNeuron(conn.Target.Index);
            if (targetNeuron == null)
            {
                targetNeuron = conn.Target.Copy();
                List<Neuron> sourceLayer = NeuronLayer(sourceNeuron);
                if (IsInput(sourceLayer))
                {
                    if (HiddenLayer.Count == 0)
                    {
                        List<Neuron> targetLayer = new List<Neuron>();
                        targetLayer.Add(targetNeuron);
                        HiddenLayer.Add(targetLayer);
                    }
                    else
                    {
                        List<Neuron> targetLayer = HiddenLayer[0];
                        targetLayer.Add(targetNeuron);
                    }
                }
                else
                {
                    int hiddenPos = HiddenLayer.IndexOf(sourceLayer);
                    if (hiddenPos == HiddenLayer.Count - 1)
                    {
                        List<Neuron> targetLayer = new List<Neuron>();
                        targetLayer.Add(targetNeuron);
                        HiddenLayer.Add(targetLayer);
                    }
                    else
                    {
                        List<Neuron> targetLayer = HiddenLayer[hiddenPos + 1];
                        targetLayer.Add(targetNeuron);
                    }
                }
                sourceNeuron.AddConnection(targetNeuron, conn);
            }
            else
            {
                List<Neuron> sourceLayer = NeuronLayer(sourceNeuron);
                List<Neuron> targetLayer = NeuronLayer(targetNeuron);
                if(sourceLayer == targetLayer)
                {
                    int hiddenPos = HiddenLayer.IndexOf(sourceLayer);
                    if (hiddenPos == HiddenLayer.Count - 1)
                    {
                        targetLayer = new List<Neuron>();
                        targetLayer.Add(targetNeuron);
                        HiddenLayer.Add(targetLayer);
                    }
                    else
                    {
                        targetLayer = HiddenLayer[hiddenPos + 1];
                        targetLayer.Add(targetNeuron);
                    }
                }
                else if(!IsInput(sourceNeuron) && !IsOutput(targetNeuron))
                {
                    int sourceHiddenPos = HiddenLayer.IndexOf(sourceLayer);
                    int targetHiddenPos = HiddenLayer.IndexOf(targetLayer);
                    if(targetHiddenPos < sourceHiddenPos)
                    {
                        targetLayer.Remove(targetNeuron);
                        if(sourceHiddenPos == HiddenLayer.Count - 1)
                        {
                            List<Neuron> newLayer = new List<Neuron>();
                            newLayer.Add(targetNeuron);
                            HiddenLayer.Add(newLayer);
                        }
                        else
                        {
                            List<Neuron> newLayer = HiddenLayer[sourceHiddenPos + 1];
                            newLayer.Add(targetNeuron);
                            HiddenLayer.Add(newLayer);
                        }
                        if (targetLayer.Count == 0) HiddenLayer.RemoveAt(targetHiddenPos);
                    }
                }
                sourceNeuron.AddConnection(targetNeuron, conn);
            }
        }
        ResetInnovation();
        ResetNodeIndex();
    }

    public override void Clear()
    {
        base.Clear();
        Innovation = 0;
        NodeIndex = Input.Count + Output.Count;
    }

    private void SetInnovations()
    {
        foreach (var node in Input)
        {
            foreach (var conn in node.Connections)
            {
                conn.Innovation = Innovation;
                Innovation++;
            }
        }
        foreach (var nodeList in HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                foreach (var conn in node.Connections)
                {
                    conn.Innovation = Innovation;
                    Innovation++;
                }
            }
        }
    }

    private void SetHiddenNodesIndex()
    {
        foreach (var nodeList in HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                node.Index = NodeIndex;
                NodeIndex++;
            }
        }
    }

    private void SetBiasNodesIndex()
    {
        this.Input[this.Input.Count - 1].Index = NodeIndex;
        NodeIndex++;
        foreach (var nodeList in this.HiddenLayer)
        {
            nodeList[nodeList.Count - 1].Index = NodeIndex;
            NodeIndex++;
        }
    }

    private void ResetNodeIndex()
    {
        NodeIndex = ToNeuronList().Max(n => n.Index);
        NodeIndex++;
    }

    private void ResetInnovation()
    {
        List<Connection> connections = ToConnectionList();
        if (connections.Count != 0)
        {
            Innovation = ToConnectionList().Max(c => c.Innovation);
            Innovation++;
        }
        else Innovation = 0;
    }

    public void AddConnection(Neuron from, Neuron to)
    {
        from.AddConnection(to, Innovation);
        Innovation++;
    }

    public Neuron AddNeuron()
    {
        Neuron neuron = new Neuron(Neuron.Sigmoid, NodeIndex);
        NodeIndex++;
        return neuron;
    }
}
