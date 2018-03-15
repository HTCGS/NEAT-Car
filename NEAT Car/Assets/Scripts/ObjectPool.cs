using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    public static List<GameObject> Objects;

    private static GameObject Parent;

    static ObjectPool()
    {
        Objects = new List<GameObject>();
        Parent = GameObject.Find("Cars");
    }

    public static void InitializeObjects(GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            gameObject.transform.SetParent(Parent.transform);
            gameObject.SetActive(false);
            Objects.Add(gameObject);
        }
    }

    public static void SetDefaultPosition(Vector3 position, Quaternion rotation)
    {
        foreach (var item in Objects)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
        }
    }

    public static void DisableObjects()
    {
        foreach (var item in Objects)
        {
            item.SetActive(false);
        }
    }

    public static void EnableObjects()
    {
        foreach (var item in Objects)
        {
            item.SetActive(true);
        }
    }
}
