using UnityEngine;

public class ShipRepairStation : MonoBehaviour
{
   
    private bool jugadorEnRango = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorEnRango = true;
        EvaluateText();
    }


    private void Update()
    {
        if (!jugadorEnRango) return;
        if (Input.GetKeyDown(KeyCode.E))
            TryRepair();
    }

    private void EvaluateText()
    {
        int collected = GameDataStructure.Instance.PlanePartsDataBase.Count;
        int needed = GameManager.Instance.PlanePartsNeeded;

        if (collected >= needed)
            UIManager.Instance?.ShowInteractionText("[E] Reconstruir la Nave y Escapar");
        else
            UIManager.Instance?.ShowInteractionText(
                $"Necesitas {needed - collected} piezas del Laboratorio");
    }

    private void TryRepair()
    {
        int collected = GameDataStructure.Instance.PlanePartsDataBase.Count;
        int needed = GameManager.Instance.PlanePartsNeeded;
        if (collected >= needed)
        {
            UIManager.Instance.HideInteractionText();
            UIManager.Instance?.ShowMessage("¡Nave reconstruida! ¡Escapando!", 3f);
            GameDataStructure.Instance.LogEvent("VICTORIA — Nave reconstruida");
            MisionManager.Instance.CompleteMission("repararNave");
            GameManager.Instance.WinGame();
        }
        else
        {
            UIManager.Instance.ShowMessage($"Te faltan {needed - collected} piezas", 2f);
        }
    }


}
