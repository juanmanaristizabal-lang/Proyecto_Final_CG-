
using UnityEngine;


public class NovoJugadorTPT : MonoBehaviour
{
    //CONTROLADOR
    private CharacterController controller;
    private Animator anim;   



    [Header("Movimiento")]
    public float velocidadCaminar = 2f;
    public float velocidadCorrer = 5f;
    public float veloRota = 10f;

    private float velocidadActual;

    float x, z;

    [Header("Camara")]
    [SerializeField]
    private Camera followCamera;

    [Header("Salto y gravedad")]
    private Vector3 veloJugador;

    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;

    public float gravedad = -9.81f;
    public float salto = 1f;

    bool enPiso;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Movimiento();
    }

    void Movimiento()
    {
        //Detectar piso
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);

        if (enPiso && veloJugador.y < 0)
        {
            veloJugador.y = -2f;
        }

        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        float animX = x;
        float animY = z;

        //si el personaje corre
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animX *= 2;
            animY *= 2;
        }

        //animaciones
        anim.SetFloat("VelX", animX, 0.1f, Time.deltaTime);
        anim.SetFloat("VelY", animY, 0.1f, Time.deltaTime);

        //caminar y correr
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocidadActual = velocidadCorrer;
        }
        else
        {
            velocidadActual = velocidadCaminar;
        }

        //Movimiento relativo a la camara
        Vector3 moveInput =
            Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)
            * new Vector3(x, 0, z);

        Vector3 moveDirection = moveInput.normalized;

        //Mover personaje
        controller.Move(moveDirection * velocidadActual * Time.deltaTime);

        //Rotacion
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotacion =
                Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacion,
                veloRota * Time.deltaTime
            );
        }

        //SALTO
        if (Input.GetButtonDown("Jump") && enPiso)
        {
            anim.SetTrigger("Jump");

            veloJugador.y = Mathf.Sqrt(salto * -2f * gravedad);
        }

        //GRAVEDAD
        veloJugador.y += gravedad * Time.deltaTime;

        controller.Move(veloJugador * Time.deltaTime);
    }
}