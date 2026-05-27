using UnityEngine;

public class LabExitTrigger : MonoBehaviour
{
    
    private bool jugadorEnRango = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        jugadorEnRango = true;
        EvaluateText();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        
            jugadorEnRango = false;
            UIManager.Instance.HideInteractionText();
        
    }
    private void Update()
    {
        if (!jugadorEnRango) return;
        if (Input.GetKeyDown(KeyCode.E))
            IntentarSalir();
    }

    private void EvaluateText()
    {
        int collected = GameDataStructure.Instance.PlanePartsDataBase.Count;
        int needed = GameManager.Instance.PlanePartsNeeded;

        if(collected >= needed)
        
            UIManager.Instance.ShowInteractionText($"[E] Salir del Laboratorio   {collected}/{needed} piezas");

        else 
            UIManager.Instance.ShowInteractionText($"Necesitas {needed - collected} piezas más para salir");

    }

    private void IntentarSalir()
    {
        int collected = GameDataStructure.Instance.PlanePartsDataBase.Count;
        int needed = GameManager.Instance.PlanePartsNeeded;
        if (collected >= needed)
        {
            UIManager.Instance.HideInteractionText();
            GameDataStructure.Instance.LogEvent($"Laboratorio completado con {collected} piezas.");
            MisionManager.Instance.CompleteMission("repararNave");
            GameManager.Instance.GoToIsla();
        }
        else
        {
            UIManager.Instance.ShowMessage($"Te faltan {needed - collected} piezas", 2f); 
        }

    }
}
