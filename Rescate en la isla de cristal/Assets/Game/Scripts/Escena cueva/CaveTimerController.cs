using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CaveTimerController : MonoBehaviour
{
    [Header("UI del timer")]
    public TextMeshProUGUI timerText;

    [Header("Color de advertencia (cuando queda poco tiempo)")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float warningThreshold = 15f;   

    private float timeRemaining;
    private bool timerRunning = false;
    private bool playerExited = false;

    private void Start()
    {
      
        if (JsonManager.Instance?.Data?.config != null)
            timeRemaining = JsonManager.Instance.Data.config.caveTimeLimit;
        else
            timeRemaining = 60f;

        timerRunning = true;
        Debug.Log($"[CaveTimer] Tiempo límite: {timeRemaining}s");
    }

    private void Update()
    {
        if (!timerRunning || playerExited) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0f)
            TimeUp();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        float t = Mathf.Max(timeRemaining, 0f);
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        timerText.text = $" {minutes:00}:{seconds:00}";
        timerText.color = timeRemaining <= warningThreshold ? warningColor : normalColor;
    }

    private void TimeUp()
    {
        timerRunning= false;
        GameDataStructure.Instance.collectedCrystals.Clear();
        SceneManager.LoadScene("CUEVA");
    }

    

    
    public void StopTimer()
    {
        timerRunning = false;
        playerExited = true;
    }
}
