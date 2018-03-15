using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    public Neuron Target;

    public float Weight;

    public Connection()
    {

    }

    public Connection(Neuron target, float weight)
    {
        this.Target = target;
        this.Weight = weight;
        target.InputsSum++;
    }

    public void SendValue(float value)
    {
        float sendedValue = value * Weight;
        Target.ReciveValue(sendedValue);
    }
}
