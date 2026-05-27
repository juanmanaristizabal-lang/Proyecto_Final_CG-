using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    [Header("Estado del jugador")] 
       public bool hasKey = false;

    [Header("Configuracion")]
    public int crystalsNeededInCave = 5;
    public int PlanePartsNeeded = 3;    

    private const string SCENE_ISLA = "ISLA";   
    private const string SCENE_CUEVA = "CUEVA";
    private const string SCENE_LABORATORIO = "LABORATORIO";

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
        }
    }

    private void Start()
    {
        // Leer configuración desde el JSON maestro
        if (JsonManager.Instance.Data != null)
        {
            var cfg = JsonManager.Instance.Data.config;
            crystalsNeededInCave = cfg.crystalsNeededInCave;
            PlanePartsNeeded = cfg.shipPartsNeeded;
            Debug.Log($"[GameManager] Config cargada → Cristales: {crystalsNeededInCave}, Piezas: {PlanePartsNeeded}");
        }

        // Cargar progreso si existe
       // if (JsonManager.Instance.HasSaveFile())
        //{
          //  ApplySaveData(JsonManager.Instance.LoadSave());
       // }
    }

    public void CollectKey()
    {
        hasKey = true;
        GameDataStructure.Instance.LogEvent("Llave recolectada");
        UIManager.Instance.ShowMessage("Has recolectado la llave!");
        MisionManager.Instance.CompleteMission("Encontrar la llave");
        SaveGame();
    }

    public void GoToCueva()
    {

        GameDataStructure.Instance.collectedCrystals.Clear();
        GameDataStructure.Instance.LogEvent("Entrando a la Cueva");
        SaveGame();
        SceneManager.LoadScene(SCENE_CUEVA);
    }

    public void GoToLaboratorio()
    {
        GameDataStructure.Instance.PlanePartsDataBase.Clear();
        GameDataStructure.Instance.repairQueue.Clear();

        GameDataStructure.Instance.LogEvent("Entrando al Laboratorio");
        SaveGame();
        SceneManager.LoadScene("Laboratorio");
    }

    public void GoToIsla()
    {
        GameDataStructure.Instance.LogEvent("Regresando a la Isla");
        SaveGame();
        SceneManager.LoadScene(SCENE_ISLA);
    }

    public void WinGame()
    {
        GameDataStructure.Instance.LogEvent("¡VICTORIA! La nave fue reconstruida.");
        SaveGame();
        Debug.Log("🏆 ¡El juego ha terminado!");
        
    }

    public void SaveGame()
    {
        PlayerSaveData data = new PlayerSaveData
        {
            currentScene = SceneManager.GetActiveScene().name,
            hasKey = this.hasKey,

            collectedCrystals = GameDataStructure.Instance.collectedCrystals,

            collectedShipParts = new List<string>(
                GameDataStructure.Instance.PlanePartsDataBase.Keys
            ),

            score = GameDataStructure.Instance.collectedCrystals.Count * 10
        };

        JsonManager.Instance.SaveGame(data);
    }
    private void ApplySaveData(PlayerSaveData save)
    {
        if (save == null) return;
        hasKey = save.hasKey;
        GameDataStructure.Instance.LoadFroamSave(save);
        Debug.Log($"[GameManager] Progreso cargado → Llave: {hasKey}");
    }

}
