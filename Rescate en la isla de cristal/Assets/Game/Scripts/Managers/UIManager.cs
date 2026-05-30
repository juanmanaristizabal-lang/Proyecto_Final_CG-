using System.Collections;
using TMPro;

using UnityEngine;

using UnityEngine.SceneManagement;

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

    [Header("Contador piezas nave")]
    public TextMeshProUGUI planePartsText;

    [Header("Contador piezas nave")]    
    private float messageTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            Destroy(gameObject);
        }

        HideInteractionText();

        if (messagePanel)
            messagePanel.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(CargarMisionDelJSON());
    }

    private IEnumerator CargarMisionDelJSON()
    {
        yield return null;

        if (JsonManager.Instance?.Data == null) yield break;

        string escena = SceneManager.GetActiveScene().name;
        var misiones = JsonManager.Instance.Data.missions;

        MissionData misionAMostrar = null;

        if (escena == "ISLA")
        {
            int parts = GameDataStructure.Instance.PlanePartsDataBase.Count;
            int needed = GameManager.Instance.PlanePartsNeeded;

         
            if (parts >= needed)
            {
                foreach (var m in misiones)
                {
                    if (m.id == "reconstruir_nave")
                    {
                        misionAMostrar = m;
                        break;
                    }
                }
            }
            else
            {
              
                foreach (var m in misiones)
                {
                    if (m.id == "encontrar_llave")
                    {
                        misionAMostrar = m;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (var m in misiones)
            {
                if (m.scene.Trim() == escena.Trim())
                {
                    misionAMostrar = m;
                    break;
                }
            }
        }

        // Mostrar la misión encontrada
        if (misionAMostrar != null)
        {
            UpdateMissionText(misionAMostrar.title, misionAMostrar.description);
            ShowMessage(misionAMostrar.description, 4f);
            Debug.Log($"[UIManager]  Misión: {misionAMostrar.title}");
        }
        else
        {
            Debug.Log($"[UIManager] Sin misión para: {escena}");
        }
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
        if (missionDescText != null) missionDescText.text = description;
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

    public void UpdatePlaneParts(int current, int needed)
    {
        if (planePartsText != null)
        {
            planePartsText.text = $"Piezas: {current} / {needed}";
        }
    }

   

}