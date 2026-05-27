using System;
using System.Collections.Generic;




[Serializable]
public class GameConfig
{
    public int crystalsNeededInCave;  
    public float caveTimeLimit;
    public int shipPartsNeeded;
    public int playerHealth;          
    public int bossHealth;           
}


[Serializable]
public class MissionData
{
    public string id;
    public string title;
    public string description;
    public string scene;
    public bool completed;

}

[Serializable]
public class DialogueData
{
    public string id;
    public string scene;
    public string triggerZone;   
    public List<string> lines;         
}


[Serializable]
public class EnemyData
{
    public string id;
    public string scene;
    public string type;             
    public float detectionRange;
    public int damage;
    public int health;

}

[Serializable]
public class ItemData
{
    public string id;
    public string type;             
    public string scene;
    public string partName;         
    public string description;      
}


[Serializable]
public class GameData
{
    public GameConfig config;
    public List<MissionData> missions;
    public List<DialogueData> dialogues;
    public List<EnemyData> enemies;
    public List<ItemData> items;
}
