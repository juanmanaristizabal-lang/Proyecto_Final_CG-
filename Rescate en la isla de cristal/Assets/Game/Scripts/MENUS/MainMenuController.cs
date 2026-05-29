using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Botones")]
    public GameObject btnCargar;

    private void Start()
    {
       
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (btnCargar != null)
            btnCargar.SetActive(JsonManager.Instance.HasSaveFile());
    }

    public void NuevaPartida()
    {
        ResetearTodo();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("ISLA");
    }

    private PlayerSaveData saveToLoad;

    public void CargarPartida()
    {
        saveToLoad = JsonManager.Instance.LoadSave();
        if (saveToLoad == null) { NuevaPartida(); return; }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

      
        SceneManager.sceneLoaded += OnEscenaCargada;
        SceneManager.LoadScene(saveToLoad.currentScene);
    }

    private void OnEscenaCargada(Scene scene, LoadSceneMode mode)
    {

        SceneManager.sceneLoaded -= OnEscenaCargada;

        if (scene.name != "CUEVA")
        {
            GameDataStructure.Instance.LoadFroamSave(saveToLoad);
        }
        else
        {
            GameDataStructure.Instance.collectedCrystals = new List<string>();
        }
    }


    public void Salir()
    {
        Application.Quit();
        Debug.Log("[Menu] Saliendo...");
    }

    private void ResetearTodo()
    {
        JsonManager.Instance.DeleteSave();
        GameManager.Instance.ResetearJuego();
        MisionManager.Instance.ResetearMisiones();
        GameDataStructure.Instance.collectedCrystals.Clear();
        GameDataStructure.Instance.PlanePartsDataBase.Clear();
        GameDataStructure.Instance.repairQueue.Clear();
        GameDataStructure.Instance.eventHistory.Clear();
    }
}
