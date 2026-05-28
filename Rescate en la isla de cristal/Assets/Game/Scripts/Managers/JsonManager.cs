using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance { get; private set; }

    private GameData data;
    private string savePath;

    public GameData Data
    {
        get
        {
            if (data == null)
            {
                Debug.LogWarning("[JSONManager] Data null, recargando...");
                LoadGameData();
            }
            return data;
        }
    }

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

        savePath = Path.Combine(Application.persistentDataPath, "save_data.json");
        LoadGameData();
    }

    public void LoadGameData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "game_data.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"[JSONManager] No encontrado: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        data = JsonUtility.FromJson<GameData>(json);
        Debug.Log("[JSONManager] game_data.json cargado.");
    }

    public void SaveGame(PlayerSaveData data)
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("[JSONManager] Progreso guardado.");
    }

    public PlayerSaveData LoadSave()
    {
        if (!File.Exists(savePath)) return null;
        return JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(savePath));
    }

    public bool HasSaveFile() => File.Exists(savePath);

    public void DeleteSave()
    {
        if (File.Exists(savePath)) File.Delete(savePath);
    }
}