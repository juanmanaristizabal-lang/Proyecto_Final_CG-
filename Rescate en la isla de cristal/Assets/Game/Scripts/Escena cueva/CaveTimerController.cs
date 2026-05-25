using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CaveTimerController : MonoBehaviour
{
    [Header("UI del timer")]
    public TextMeshProUGUI timerText;

    [Header("Color de advertencia")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float warningThreshold = 15f;

    private float TiempoRestante; 
    private bool tiempoCorriendo = false;
    private bool jugadorAfuera = false;

    private void Start()
    {
        GameDataStructure.Instance.collectedCrystals.Clear();

        int needed = GameManager.Instance.crystalsNeededInCave;
        UIManager.Instance.UpdateCrystalCount(0, needed);



        if (JsonManager.Instance.Data.config != null)
        {
            TiempoRestante = JsonManager.Instance.Data.config.caveTimeLimit;
        }
        else
        {
            TiempoRestante = 60f; // Valor por defecto

        }

        tiempoCorriendo = true;
        Debug.Log($"[CaveTimer] Tiempo límite: {TiempoRestante}s");
    }

    private void Update()
    {
        if (!tiempoCorriendo || jugadorAfuera) return; 
        TiempoRestante -= Time.deltaTime;
        UpdateTimerUI();
        if (TiempoRestante <= 0f)
            TimeUp();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        float t = Mathf.Max(TiempoRestante, 0f);
        int minutos = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        timerText.text = $"{minutos:00}{seconds:00}";
        timerText.color = TiempoRestante <= warningThreshold ? warningColor : normalColor;
    }

    private void TimeUp()
    {
        tiempoCorriendo = false;
        Debug.Log("[CaveTimer] ¡Tiempo agotado!");
        UIManager.Instance.ShowMessage("La cueva colapsó, inténtalo de nuevo", 2f);

        GameDataStructure.Instance.collectedCrystals.Clear();

        Invoke(nameof(RestartScene), 2f);

    }

    private void RestartScene()
    {
       SceneManager.LoadScene("CUEVA");
    }
     
    public void StopTimer()
    {
        tiempoCorriendo = false;
        jugadorAfuera = true;
    }

}
