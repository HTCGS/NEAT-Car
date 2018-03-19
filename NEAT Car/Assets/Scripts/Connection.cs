using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    public Neuron Target;

    public Neuron Source;

    public float Weight;

    public bool IsActive;

    public int Innovation;

    public Connection()
    {
        this.IsActive = true;
    }

    public Connection(Neuron target, float weight) : this()
    {
        this.Target = target;
        this.Weight = weight;
        target.InputsSum++;
    }

    public void SendValue(float value)
    {
        if (IsActive)
        {
            float sendedValue = value * Weight;
            Target.ReciveValue(sendedValue);
        }
    }

    public void Deactivate()
    {
        this.IsActive = false;
        Target.InputsSum--;
    }

    public void Activate()
    {
        this.IsActive = true;
        Target.InputsSum++;
    }

    public Connection Copy()
    {
        Connection connection = new Connection(Target, Weight);
        connection.Source = Source;
        connection.IsActive = IsActive;
        connection.Innovation = Innovation;
        return connection;
    }
}
