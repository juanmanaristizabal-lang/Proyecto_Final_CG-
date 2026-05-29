using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSuelo : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameDataStructure.Instance.collectedCrystals.Clear();
        SceneManager.LoadScene("CUEVA");



    }

}