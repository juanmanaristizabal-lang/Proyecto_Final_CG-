using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    public TextMeshProUGUI meter;
    public Vector3 offset;

    public Transform player;

    public float hideDistance = 3f;

    public GameObject nextWaypoint;

    private void Update()
    {
        // Seguridad para evitar errores null
        if (img == null || target == null || meter == null || player == null || Camera.main == null)
            return;

        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Si el objetivo está detrás de la cámara
        if (Vector3.Dot((target.position - Camera.main.transform.position),
            Camera.main.transform.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        // Mantener el waypoint dentro de pantalla
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Mover waypoint
        img.transform.position = pos;

        // Distancia
        float distance = Vector3.Distance(target.position, player.position);

        meter.text = Mathf.Round(distance) + "m";

        // Cambiar al siguiente waypoint
        if (distance < hideDistance)
        {
            if (nextWaypoint != null)
            {
                Debug.Log(nextWaypoint);
                nextWaypoint.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
