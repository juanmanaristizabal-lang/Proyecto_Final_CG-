using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    [Header("Llave")]
    public GameObject keyPrefab;
    public Transform keyPoint;

    [Header("Cristales")]
    public GameObject crystalPrefab;
    public Transform[] crystalPoints; // ← puntos visuales en Unity

    private void Start()
    {
        SpawnKey();
        SpawnCristales();
    }

    private void SpawnKey()
    {
        if (GameManager.Instance.hasKey) return;

        var gameData = JsonManager.Instance?.Data;
        if (gameData == null || gameData.items == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

        foreach (ItemData item in gameData.items)
        {
            if (item.scene != currentScene) continue;
            if (item.type != "key") continue;
            if (keyPrefab == null || keyPoint == null) return;

            GameObject key = Instantiate(
                keyPrefab, keyPoint.position, Quaternion.identity);
            key.name = item.id;

            var kc = key.GetComponent<KeyCollectable>();
            if (kc != null) kc.keyId = item.id;
        }
    }

    private void SpawnCristales()
    {
        var gameData = JsonManager.Instance?.Data;
        if (gameData == null || gameData.items == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

       
        var cristalesJSON = gameData.items.FindAll(
            i => i.scene == currentScene && i.type == "crystal");

        if (crystalPrefab == null)
        {
            Debug.LogWarning("[ItemSpawner] crystalPrefab no asignado");
            return;
        }

        
        for (int i = 0; i < cristalesJSON.Count; i++)
        {
            ItemData item = cristalesJSON[i];

            if (GameDataStructure.Instance
                .collectedCrystals.Contains(item.id)) continue;

           
            if (i >= crystalPoints.Length)
            {
                Debug.LogWarning($"[ItemSpawner] Falta CristalPoint_{i}");
                continue;
            }

            Vector3 pos = crystalPoints[i].position;

            GameObject cristal = Instantiate(
                crystalPrefab, pos, Quaternion.identity);
            cristal.name = item.id;

            var cc = cristal.GetComponent<CrystalCollectible>();
            if (cc != null) cc.crystalID = item.id;

            Debug.Log($"[ItemSpawner]  Cristal: {item.id} en {pos}");
        }
    }
}