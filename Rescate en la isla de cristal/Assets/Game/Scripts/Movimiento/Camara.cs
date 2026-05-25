using UnityEngine;

public class Camara : MonoBehaviour
{

    public float mouseSensitivity = 3.0f;

    //rotaciones en X y Y 
    private float rotY, rotX;

    //nuestro objeto a serguir 
    public Transform target;

    //Distancia entre la camara y el objetivo a seguir  
    public float distanceTarget = 3.0f;

    //Variables de rotacion
    Vector3 curRotacion; 
    Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    private float smoothTime = 0.2f;

    //variables X y Y para restringuir la rotacion total en Y
    [SerializeField]
    private Vector2 MaxMinRota = new Vector2(-20, 40); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //bloqueamos la posicion del mouse y lo desaparecemos en pamtalla 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        HandleRotation();
        HandlePosition();   
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotX -= mouseY;
        rotY += mouseX; 

        rotX = Mathf.Clamp(rotX, MaxMinRota.x, MaxMinRota.y);   

        Vector3 targetRotation = new Vector3(rotX, rotY);

        curRotacion = Vector3.SmoothDamp(curRotacion, targetRotation, ref smoothVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(curRotacion);
    }

    void HandlePosition()
    {
        transform.position = target.position - transform.forward * distanceTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
