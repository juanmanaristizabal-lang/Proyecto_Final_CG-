using UnityEngine;

public class PlanePart : MonoBehaviour
{
    [Header("Datos de la pieza")]
    public string partID;
    public string partName;
    public string description;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ALGO ENTRO");

        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            ItemData newPart = new ItemData
            {
                id = partID,
                partName = partName,
                description = description,
                type = "ship_part"
            };

            GameDataStructure.Instance.AddPlanePart(newPart);

            UIManager.Instance.ShowMessage(
                $"Has recogido: {partName}"
            );

            MisionManager.Instance.CollectShipPart();

            GameManager.Instance.SaveGame();

            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
    }
}