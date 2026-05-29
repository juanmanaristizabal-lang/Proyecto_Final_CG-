using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LabController : MonoBehaviour
{
    private bool reiniciando = false;

    private void Start()
    {
       
        GameDataStructure.Instance.PlanePartsDataBase.Clear();
        GameDataStructure.Instance.repairQueue.Clear();

        
        UIManager.Instance?.UpdatePlaneParts(
            0, GameManager.Instance.PlanePartsNeeded);

        Debug.Log("[LabController] Laboratorio iniciado.");
    }


    public void JugadorMurio()
    {
        if (reiniciando) return;
        reiniciando = true;

        GameDataStructure.Instance.PlanePartsDataBase.Clear();
        GameDataStructure.Instance.repairQueue.Clear();
        SceneManager.LoadScene("Laboratorio"); 
    }

    
}
