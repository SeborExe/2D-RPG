using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentID;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentID = new List<string>();
    }
}
