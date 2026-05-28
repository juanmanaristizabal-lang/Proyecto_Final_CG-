using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI crystalsText;
    public TextMeshProUGUI partsText;

    private void Start()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        if (titleText != null)
            titleText.text = "Escapaste de la Isla";

        if (scoreText != null)
            scoreText.text = $"Puntaje: {CalcularPuntaje()}";

        if (crystalsText != null)
            crystalsText.text = $"Cristales recolectados: " +
                $"{GameDataStructure.Instance.collectedCrystals.Count}";

        if (partsText != null)
            partsText.text = $"Piezas recolectadas: " +
                $"{GameDataStructure.Instance.PlanePartsDataBase.Count}";
    }

    private int CalcularPuntaje()
    {
        int cristales = GameDataStructure.Instance.collectedCrystals.Count;
        int piezas = GameDataStructure.Instance.PlanePartsDataBase.Count;
        return (cristales * 10) + (piezas * 50);
    }

    public void JugarDeNuevo()
    {

        GameManager.Instance.ResetearJuego();
        MisionManager.Instance.ResetearMisiones();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        JsonManager.Instance.DeleteSave();
        JsonManager.Instance.LoadGameData();
        GameDataStructure.Instance.collectedCrystals.Clear();
        GameDataStructure.Instance.PlanePartsDataBase.Clear();
        GameDataStructure.Instance.repairQueue.Clear();
        GameDataStructure.Instance.eventHistory.Clear();
        GameManager.Instance.hasKey = false;
        SceneManager.LoadScene("ISLA");
    }

    public void Salir()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        Application.Quit();
           Debug.Log("[Victory] Saliendo...");
    }
}
