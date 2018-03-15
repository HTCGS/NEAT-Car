using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    public static List<GameObject> Objects;

    static ObjectPool()
    {
        Objects = new List<GameObject>();
    }

    public static void InitializeObjects(GameObject prefab, int size)
    {
        GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Objects.Add(gameObject);
    }

    public static void SetDefaultPosition(Vector3 position, Quaternion rotation)
    {
        foreach (var item in Objects)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
        }
    }
}
