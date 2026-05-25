using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    private bool _playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;

        if (GameManager.Instance.hasKey)
            UIManager.Instance?.ShowInteractionText("[E] Entrar a la Cueva Cristalina");
        else
            UIManager.Instance?.ShowInteractionText("Necesitas una llave para entrar");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
        UIManager.Instance?.HideInteractionText();
    }

    private void Update()
    {
        if (!_playerInRange) return;

        // Solo entra si tiene la llave y presiona E
        if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance.hasKey)
        {
            UIManager.Instance.HideInteractionText();
            GameManager.Instance.GoToCueva();
        }
    }
}



