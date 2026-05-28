using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab de la llave")]
    public GameObject keyPrefab;

    [Header("Punto donde aparece la llave")]
    public Transform keyPoint;

    private void Awake()
    {
        SpawnKey();
    }

    private void SpawnKey()
    {
        // Verificar en el save si ya tiene la llave
        PlayerSaveData save = JsonManager.Instance.LoadSave();
        bool yaTimeLlave = save != null && save.hasKey;

        if (yaTimeLlave)
        {
            Debug.Log("[ItemSpawner] Save indica que ya tiene la llave.");
            return;
        }

        var gameData = JsonManager.Instance?.Data;
        if (gameData == null || gameData.items == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

        foreach (ItemData item in gameData.items)
        {
            if (item.scene != currentScene) continue;
            if (item.type != "key") continue;
            if (keyPrefab == null || keyPoint == null) return;

            GameObject key = Instantiate(keyPrefab, keyPoint.position, Quaternion.identity);
            key.name = item.id;

            var kc = key.GetComponent<KeyCollectable>();
            if (kc != null) kc.keyId = item.id;

            Debug.Log($"[ItemSpawner] ✅ Llave spawneada: {item.id}");
        }
    }
}