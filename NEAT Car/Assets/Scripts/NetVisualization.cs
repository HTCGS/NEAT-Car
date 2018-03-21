using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetVisualization : MonoBehaviour
{
    public GameObject Prefab;

    public GameObject Parent;

    private Dictionary<int, GameObject> Nodes;

    void Start()
    {
        Nodes = new Dictionary<int, GameObject>();
    }

    public void Visualize(NeuroNet neuroNet)
    {
        DeleteNodes();
        NodeInit(neuroNet);
        foreach (var node in neuroNet.Input)
        {
            int pos = 0;
            foreach (var conn in node.Connections)
            {
                GameObject gameObject = Nodes[node.Index];
                LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

                Vector3[] points = new Vector3[lineRenderer.positionCount];
                lineRenderer.GetPositions(points);
                lineRenderer.positionCount = lineRenderer.positionCount + 2;
                lineRenderer.SetPositions(points);

                lineRenderer.SetPosition(pos, gameObject.transform.position);
                lineRenderer.SetPosition(pos + 1, Nodes[conn.Target.Index].transform.position);
                pos += 2;
            }
        }
        foreach (var nodeList in neuroNet.HiddenLayer)
        {
            foreach (var node in nodeList)
            {
                int pos = 0;
                foreach (var conn in node.Connections)
                {
                    GameObject gameObject = Nodes[node.Index];
                    LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

                    Vector3[] points = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(points);
                    lineRenderer.positionCount = lineRenderer.positionCount + 2;
                    lineRenderer.SetPositions(points);

                    lineRenderer.SetPosition(pos, gameObject.transform.position);
                    lineRenderer.SetPosition(pos + 1, Nodes[conn.Target.Index].transform.position);
                    pos += 2;
                }
            }
        }
    }

    private void NodeInit(NeuroNet neuroNet)
    {
        Vector3 pos = this.transform.position;
        foreach (var node in neuroNet.Input)
        {
            GameObject gameObject = Instantiate(Prefab, pos, Quaternion.identity);
            gameObject.transform.SetParent(Parent.transform);
            Nodes.Add(node.Index, gameObject);
            pos += new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
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
                pos += new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
            }
            pos += new Vector3(Prefab.transform.localScale.magnitude * 2, 0, 0);
            pos.z = this.transform.position.z;
        }
        foreach (var node in neuroNet.Output)
        {
            GameObject gameObject = Instantiate(Prefab, pos, Quaternion.identity);
            gameObject.transform.SetParent(Parent.transform);
            Nodes.Add(node.Index, gameObject);
            pos += new Vector3(0, 0, Prefab.transform.localScale.magnitude * 1.5f);
        }
    }

    private void DeleteNodes()
    {
        foreach (var node in Nodes)
        {
            Destroy(node.Value);
        }
        Nodes.Clear();
    }

    public float MaxZPosition()
    {
        float maxZ = 0;
        foreach (var node in Nodes)
        {
            if (node.Value.transform.position.z > maxZ) maxZ = node.Value.transform.position.z;
        }
        return maxZ;
    }
}
