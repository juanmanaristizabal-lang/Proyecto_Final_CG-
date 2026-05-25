using UnityEngine;

public class CaveExitTrigger : MonoBehaviour
{

    private bool jugadorEnRango = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        {
            jugadorEnRango = true;
            EvaluateText();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        {
            jugadorEnRango = false;
            UIManager.Instance.HideInteractionText();
        }

    }

    private void Update()
    {
        if (!jugadorEnRango) return;

        if (Input.GetKeyDown(KeyCode.E))
            IntentarSalir();
    }


    private void EvaluateText()
    {
        int current = GameDataStructure.Instance.collectedCrystals.Count;
        int needed = GameManager.Instance.crystalsNeededInCave;

        if (current >= needed)
            UIManager.Instance?.ShowInteractionText($"[E] Salir de la Cueva   {current}/{needed} cristales");
        else
            UIManager.Instance?.ShowInteractionText($"Necesitas {needed - current} cristales más para salir");
    }


    private void IntentarSalir()
    {
        int current = GameDataStructure.Instance.collectedCrystals.Count;
        int needed = GameManager.Instance.crystalsNeededInCave;

        if (current >= needed)
        {
            // Detiene el timer para que no reinicie mientras carga
            FindFirstObjectByType<CaveTimerController>()?.StopTimer();

            UIManager.Instance.HideInteractionText();
            GameDataStructure.Instance.LogEvent($"Cueva completada con {current} cristales.");
            MisionManager.Instance.CompleteMission("sobrevivir_cueva");
            GameManager.Instance.GoToLaboratorio();
        }
        else
        {
            UIManager.Instance?.ShowMessage(
                $"¡Necesitas {needed - current} cristales más para salir!", 2f);
        }
    }

}
