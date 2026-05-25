using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab de la llave")]
    public GameObject keyPrefab;

    [Header("Punto donde aparece la llave")]
    public Transform keyPoint;

    private void Start()
    {
        SpawnKey();
    }

    private void SpawnKey()
    {
        var gameData = JsonManager.Instance?.Data;

        if (gameData == null || gameData.items == null)
        {
            Debug.LogWarning("[ItemSpawner] No se encontraron ítems.");
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;

        foreach (ItemData item in gameData.items)
        {
            // Solo buscar llaves de esta escena
            if (item.scene != currentScene)
                continue;

            if (item.type != "key")
                continue;

            if (keyPrefab == null || keyPoint == null)
            {
                Debug.LogWarning("[ItemSpawner] Falta asignar prefab o punto de spawn.");
                return;
            }

            GameObject key =
                Instantiate(keyPrefab,
                keyPoint.position,
                Quaternion.identity);

            key.name = item.id;

            var keyCollectible =
                key.GetComponent<KeyCollectable>();

            if (keyCollectible != null)
                keyCollectible.keyId = item.id;

            Debug.Log($"[ItemSpawner] Llave creada: {item.id}");
        }
    }
}