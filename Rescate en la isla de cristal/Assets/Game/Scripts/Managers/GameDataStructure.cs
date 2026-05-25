
using System.Collections.Generic;
using UnityEngine;

public class GameDataStructure : MonoBehaviour
{

    public static GameDataStructure Instance { get; private set; }

    //List para los cristales 
    public List<string> collectedCrystals = new List<string>();

    //queue para las piezas de nave (  por orden de llegada)
    public Queue <ItemData> repairQueue = new Queue<ItemData>();

    // Historial para el lore (ultima mision va encima) 
    public Stack<string> eventHistory = new Stack<string>();

    public Dictionary<string, ItemData> PlanePartsDataBase = new Dictionary<string, ItemData>();

    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCrystal(string crystalId)
    {
        if (!collectedCrystals.Contains(crystalId))
        {
            collectedCrystals.Add(crystalId);
            LogEvent($"Cristal recolectado:  {crystalId}");
        }
    }

    public void AddPlanePart(ItemData part)
    {
        if(!PlanePartsDataBase.ContainsKey(part.id))
        {
            PlanePartsDataBase.Add(part.id, part);
            repairQueue.Enqueue(part);
            LogEvent($"Pieza de nave añadida a la cola de reparación: {part.partName}");
        }
    }

    public ItemData GetNextShipPart()
    {
        return repairQueue.Count > 0 ? repairQueue.Dequeue() : null;
    }

    public void LogEvent(string message)
    {
        string entry = $"[{System.DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        eventHistory.Push(entry);
        Debug.Log($"Evento registrado: {entry}");
    }

    public void LoadFroamSave(PlayerSaveData saveData)
    {
        if (saveData is null) return;    
        collectedCrystals = new List<string>(saveData.collectedCrystals);

    }
}
