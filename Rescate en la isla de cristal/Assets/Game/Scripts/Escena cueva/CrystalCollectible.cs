using Unity.VisualScripting;
using UnityEngine;

public class CrystalCollectible : MonoBehaviour
{
    [Header("ID unico del cristal")]
    public string crystalID = "cristal_01";

    private bool collecteeed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(collecteeed) return;
        if(!other.CompareTag("Player")) return;
        Collect();
    }


    private void Collect()
    {
        collecteeed = true; 

        GameDataStructure.Instance.AddCrystal(crystalID);

        int current = GameDataStructure.Instance.collectedCrystals.Count;
        int needed = GameManager.Instance.crystalsNeededInCave;
        UIManager.Instance.UpdateCrystalCount(current, needed);
        UIManager.Instance.ShowMessage($"Has recolectado un cristal! ({current}/{needed})");

    }

}
