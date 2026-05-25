using System.IO;
using UnityEngine;


public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance { get; private set; }

  
    public GameData Data { get; private set; }

    private string _savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _savePath = Path.Combine(Application.persistentDataPath, "save_data.json");
        LoadGameData();
    }

   
    private void LoadGameData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "game_data.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"[JSONManager] NO SE ENCONTRÓ game_data.json en: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        Data = JsonUtility.FromJson<GameData>(json);

        if (Data == null)
            Debug.LogError("[JSONManager] Error al econtrar game_data.json");
        else
            Debug.Log("[JSONManager] game_data.json cargado correctamente.");
    }


    public void SaveGame(PlayerSaveData data)
    {
        File.WriteAllText(_savePath, JsonUtility.ToJson(data, true));
        Debug.Log("[JSONManager] Progreso guardado.");
    }

    public PlayerSaveData LoadSave()
    {
        if (!File.Exists(_savePath)) return null;
        return JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(_savePath));
    }

    public bool HasSaveFile() => File.Exists(_savePath);

    public void DeleteSave()
    {
        if (File.Exists(_savePath)) File.Delete(_savePath);
    }
}