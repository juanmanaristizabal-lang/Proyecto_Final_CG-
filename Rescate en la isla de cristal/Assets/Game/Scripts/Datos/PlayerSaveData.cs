using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerSaveData
{
    public string currentScene = "ISLA";
    public bool hasKey = false;
    public int score = 0;
    public List<string> collectedCrystals = new List<string>();
    public List<string> collectedShipParts = new List<string>();
}
