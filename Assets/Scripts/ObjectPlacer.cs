using System;
using System.Collections.Generic;

using UnityEngine;


public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position, GridOrientation Orientation )
    {
         
        GameObject newObject = Instantiate(prefab); //instancia objeto 

        //convertir posiciones a la grid
        newObject.transform.position = position;
        newObject.transform.rotation = Quaternion.Euler(0, (int)Orientation * 90, 0); //rotar el objeto dependiendo de la orientacion

        placedGameObjects.Add(newObject); //agregar a la lista de objetos que se colocaron

        return placedGameObjects.Count - 1; //return indice del objeto 
    }

    internal void RemoveObject(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null; //eliminar el objeto de la lista, pero no eliminar la referencia del indice
    }
}
