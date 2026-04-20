 using UnityEngine;
using System.Collections.Generic;


public class JSONReader : MonoBehaviour
{

    public Inventory inventory = new Inventory();

    public void SaveToJson()
    {
        string inventoryData = JsonUtility.ToJson(inventory);
        string filePath = Application.persistentDataPath + "/JSONText" ;
    }

    [System.Serializable]

public class Inventory
    {
        public int vibe; 
    }
}
