using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 1;
    public float Rotation = 1;

    void Start()
    {
        //NeuroNet neuroNet = new NeuroNet(4, 3);
        //neuroNet.GenerateDefaultNet(4);
        //neuroNet.ToConnectionList();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(this.transform.up * -Rotation * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(this.transform.up * Rotation * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += this.transform.forward * Speed * Time.deltaTime;
        }
    }
}
