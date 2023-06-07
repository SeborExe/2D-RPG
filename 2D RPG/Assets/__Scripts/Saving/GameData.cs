using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> skillTree;
    public List<string> equipmentID;
    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckPointID;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();
        equipmentID = new List<string>();
        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckPointID = string.Empty;
    }
}
