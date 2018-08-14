using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetVisualization : MonoBehaviour
{
    public GameObject Prefab;

    public GameObject Parent;

    public GameObject Text;

    private Dictionary<int, GameObject> Nodes;

    void Start()
    {
        Nodes = new Dictionary<int, GameObject>();
    }

    public void Visualize(NeuroNet neuroNet)
    {
        DeleteNodes();
        NodeVizualization(neuroNet);
        foreach (var node in neuroNet.Input)
        {
            ConnectionVizualization(node);
        }
        foreach (var nodeList in neuroNet.HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                ConnectionVizualization(node);
            }
        }
    }

    private void NodeVizualization(NeuroNet neuroNet)
    {
        Vector3 pos = this.transform.position;
        foreach (var node in neuroNet.Input)
        {
            GameObject gameObject = Instantiate(Prefab, pos, Quaternion.identity);
            gameObject.transform.SetParent(Parent.transform);
            Nodes.Add(node.Index, gameObject);
            pos -= new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
        }
        pos += new Vector3(Prefab.transform.localScale.magnitude * 2, 0, 0);
        pos.z = this.transform.position.z;
        foreach (var nodeList in neuroNet.HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                GameObject gameObject = Instantiate(Prefab, pos, Quaternion.identity);
                gameObject.transform.SetParent(Parent.transform);
                Nodes.Add(node.Index, gameObject);
                pos -= new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
            }
            pos += new Vector3(Prefab.transform.localScale.magnitude * 2, 0, 0);
            pos.z = this.transform.position.z;
        }
        foreach (var node in neuroNet.Output)
        {
            GameObject gameObject = Instantiate(Prefab, pos, Quaternion.identity);
            gameObject.transform.SetParent(Parent.transform);
            Nodes.Add(node.Index, gameObject);
            pos -= new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
        }
    }

    private void ConnectionVizualization(Neuron node)
    {
        int pos = 0;
        foreach (var conn in node.Connections)
        {
            GameObject gameObject = Nodes[node.Index];

            GameObject child = Instantiate(Prefab, gameObject.transform.position, Quaternion.identity);
            child.GetComponent<MeshRenderer>().enabled = false;
            child.transform.SetParent(gameObject.transform);
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            if (!conn.IsActive) lineRenderer.material.color = Color.black;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, Nodes[conn.Target.Index].transform.position);

            Vector3 textPos = child.transform.position;
            textPos.z += child.transform.localScale.magnitude;
            textPos.x += (child.transform.localScale.magnitude);
            textPos.z -= pos * 1.2f;
            pos++;
            GameObject text = Instantiate(Text, textPos, Text.transform.rotation);
            text.transform.SetParent(child.transform);
            TextMesh textMesh = text.GetComponent<TextMesh>();
            textMesh.text = conn.Weight.ToString("F2");
        }
    }

    private void DeleteNodes()
    {
        foreach (var node in Nodes)
        {
            for (int i = 0; i < node.Value.transform.childCount; i++)
            {
                GameObject gameObject = node.Value.transform.GetChild(i).gameObject;
                for (int j = 0; j < gameObject.transform.childCount; j++)
                {
                    Destroy(gameObject.transform.GetChild(j).gameObject);
                }
                Destroy(node.Value.transform.GetChild(i).gameObject);
            }
            Destroy(node.Value);
        }
        Nodes.Clear();
    }

    public float MaxZPosition()
    {
        float maxZ = 0;
        foreach (var node in Nodes)
        {
            if (node.Value.transform.position.z < maxZ) maxZ = node.Value.transform.position.z;
        }
        return maxZ;
    }
}
