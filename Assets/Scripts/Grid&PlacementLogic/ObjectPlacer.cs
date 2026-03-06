using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 pos)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = pos;
        placedObjects.Add(newObject);
        return placedObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedObjects.Count <= gameObjectIndex || placedObjects[gameObjectIndex] == null)
            return;
        Destroy(placedObjects[gameObjectIndex]);
        placedObjects[gameObjectIndex] = null;
    }
}
