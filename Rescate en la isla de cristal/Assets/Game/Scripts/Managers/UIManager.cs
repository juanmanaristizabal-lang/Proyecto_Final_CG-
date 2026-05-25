using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Texto de interaccion")]
    public GameObject interactionPanel;
    public TextMeshProUGUI interactionText;

    [Header("Misión activa")]
    public TextMeshProUGUI missionTitleText;
    public TextMeshProUGUI missionDescText;

    [Header("Mensaje temporal en pantalla")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    [Header("Contador de cristales (activo en Cueva)")]
    public TextMeshProUGUI crystalCountText;

    private float messageTimer;

    private void Awake()
    {
        Instance = this;
        HideInteractionText();
        if (messagePanel) messagePanel.SetActive(false);
    }

    private void Update()
    {
        if (messagePanel != null && messagePanel.activeSelf)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                messagePanel.SetActive(false);
            }
        }


    }

    public void ShowInteractionText(string text)
    {
        if (interactionPanel == null) return;
        interactionPanel.SetActive(true);
        interactionText.text = text;

    }

     public void UpdateMissionText(string title, string description)
    {
        if (missionTitleText != null) missionTitleText.text = title;
        if (missionDescText  != null) missionDescText.text  = description;
    }


    public void HideInteractionText()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    public void ShowMessage(string text, float duration = 3f)
    {
        if (messagePanel == null) return; 
        messageText.text = text;
        messagePanel.SetActive(true);
        messageTimer = duration;
    }

    public void UpdateCrystalCount(int current, int needed)
    {
        if (crystalCountText != null)
            crystalCountText.text = $"Cristales: {current} / {needed}";
    }

}