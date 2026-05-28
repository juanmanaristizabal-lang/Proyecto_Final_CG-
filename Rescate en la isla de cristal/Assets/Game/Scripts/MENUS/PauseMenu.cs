using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Panel de pausa")]
    public GameObject pausePanel;

    private bool pausado = false;


    private void Start()
    {
        pausado = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePausa();
    }

    private void TogglePausa()
    {
        pausado = !pausado;

        pausePanel.SetActive(pausado);
        Time.timeScale = pausado ? 0f : 1f;
        Cursor.visible = pausado;
        Cursor.lockState = pausado
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }

    public void Reanudar()
    {
        pausado = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GuardarYSalirAlMenu()
    {
  
        GameManager.Instance.SaveGame();
        GameDataStructure.Instance.LogEvent("Jugador guardó y salió al menú");

        
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("MenuPrincipal");
    }
}
