using UnityEngine;

public class KeyCollectable : MonoBehaviour
{
    [HideInInspector] public string keyId = "llave_isla"; // lo asigna ItemSpawner

    public GameObject collectEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Collect();
    }

    private void Collect()
    {
        GameManager.Instance.CollectKey();
        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
