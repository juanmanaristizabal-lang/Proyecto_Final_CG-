using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header("Vida")]
    public int maximaSalud = 100; 
    private int saludActual;    
    private bool seMurio = false;

    [Header("solo visible en laboratorio")]
    public Slider healthBar; 
    public TextMeshProUGUI healthText;
    public GameObject healthBarPanel;

    private void ActualizarSaludUI()
    {
        if(healthBar != null)
        {
            healthBar.maxValue = maximaSalud;
            healthBar.value = maximaSalud;
        }
        if (healthText != null)
            healthText.text = $"{saludActual} | {maximaSalud}";

    }

    private void Die()
    {
        if (seMurio) return;

        seMurio = true;

        GameDataStructure.Instance.LogEvent("Jugador muerto");

        string escenaActual = SceneManager.GetActiveScene().name;

        if (escenaActual.Equals("Laboratorio"))
        {
            FindFirstObjectByType<LabController>()?.JugadorMurio();
        }
        else if (escenaActual.Equals("ISLA"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }



    private void Start()
    {
        if (JsonManager.Instance?.Data?.config != null)
       maximaSalud = JsonManager.Instance.Data.config.playerHealth;
        
        saludActual = maximaSalud;
        ActualizarSaludUI();


    }

    public void TakeDamage (int amount)
    {
        if (seMurio) return;
        saludActual = Mathf.Max(0, saludActual - amount);
        ActualizarSaludUI();

        GameDataStructure.Instance.LogEvent($"El jugador recibio daño: -{amount} | Vida: {saludActual}/{maximaSalud}");
        UIManager.Instance.ShowMessage($"!DAÑO! VIDA : -{amount} | Vida: {saludActual}/{maximaSalud}", 1f);
        if(saludActual <= 0)
        {
            Die();
        }

    }

   

}
