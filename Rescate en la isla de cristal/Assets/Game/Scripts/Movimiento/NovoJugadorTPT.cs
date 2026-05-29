
using UnityEngine;

public class NovoJugadorTPT : MonoBehaviour
{
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
    public LayerMask enemyLayer; 
    public float gravedad = -9.81f;
    public float salto = 1f;
    bool enPiso;

    [Header("Ataque")]
    public Transform attackPoint;    // punto frente al personaje
    public float attackRange = 2f;
    public int attackDamage = 25;
    public float attackCooldown = 1f;
    private float _lastAttackTime = -999f;
   

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Movimiento();
        Ataque();
    }

    void Movimiento()
    {
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);
        if (enPiso && veloJugador.y < 0)
            veloJugador.y = -2f;

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        float animX = x;
        float animY = z;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animX *= 2;
            animY *= 2;
        }

        anim.SetFloat("VelX", animX, 0.1f, Time.deltaTime);
        anim.SetFloat("VelY", animY, 0.1f, Time.deltaTime);

        velocidadActual = Input.GetKey(KeyCode.LeftShift)
            ? velocidadCorrer
            : velocidadCaminar;

        Vector3 moveInput = Quaternion.Euler(
            0, followCamera.transform.eulerAngles.y, 0)
            * new Vector3(x, 0, z);

        Vector3 moveDirection = moveInput.normalized;
        controller.Move(moveDirection * velocidadActual * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion rotacion = Quaternion.LookRotation(
                moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacion,
                veloRota * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && enPiso)
        {
            anim.SetTrigger("Jump");
            veloJugador.y = Mathf.Sqrt(salto * -2f * gravedad);
        }

        veloJugador.y += gravedad * Time.deltaTime;
        controller.Move(veloJugador * Time.deltaTime);
    }

    void Ataque()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;
        if (Time.time - _lastAttackTime < attackCooldown) return;

        _lastAttackTime = Time.time;
        anim.SetTrigger("Attack");

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            enemyLayer); // ← solo detecta enemigos

        Debug.Log($"[Ataque] Enemigos detectados: {hits.Length}");

        foreach (var hit in hits)
        {
            VidaAtaque boss = hit.GetComponent<VidaAtaque>();
            if (boss != null)
            {
                boss.TakeDamage(attackDamage);
                Debug.Log("[Ataque]  Daño al enemigo");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}