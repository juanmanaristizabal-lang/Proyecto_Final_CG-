using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public NavMeshAgent agent;
    private Animator animator;

    [Header("Waypoints patrulla")]
    public Transform[] waypoints;
    public float waypointDistance = 1f;
    private int currentWaypoints = 0;

    [Header("Patrulla")]
    public float rangoDettecionPatrulla = 12f;
    public float speedPatrulla = 3f;

    [Header("Persecucion")]
    public float speedPersecucion = 5.5f;

    [Header("Agresividad")]
    public float deteccionAgresividad = 25f;
    public float velocidadAgresividad = 7f;

    [Header("Daño al jugador")]
    public int daño = 25;
    public float dañoCooldown = 2f;

    
  
    private static readonly int AnimAtacar = Animator.StringToHash("Atacar");

    private float TiempoUltimoDaño = -999f;
    private bool jugadorDetectado = false;
    private int faseActual = 1;

    
    private const float UMBRAL_MOVIMIENTO = 0.1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("[Boss] No se encontró Animator en el GameObject.");

        agent.speed = speedPatrulla;

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentWaypoints].position);
    }

    private void Update()
    {
        float distancia = Vector3.Distance(transform.position, player.position);

        float detectionRange = faseActual == 3
            ? deteccionAgresividad
            : deteccionAgresividad;

        jugadorDetectado = distancia <= detectionRange;

        ActualizarFases();

        if (jugadorDetectado)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= waypointDistance)
                GoToNextWaypoint();
        }

        ActualizarAnimaciones();
    }

   

    private void ActualizarAnimaciones()
    {
        if (animator == null) return;

       
        float velocidadActual = agent.velocity.magnitude;
       
    }

    

    private void ActualizarFases()
    {
        int partsCollected = GameDataStructure.Instance.PlanePartsDataBase.Count;
        int partsNeeded = GameManager.Instance.PlanePartsNeeded;

        if (partsCollected >= partsNeeded)
        {
            if (faseActual != 3)
            {
                faseActual = 3;
                agent.speed = velocidadAgresividad;
                UIManager.Instance.ShowMessage("El jefe se ha vuelto más agresivo", 2f);
                Debug.Log("[Boss] Modo agresivo");
            }
            return;
        }

        if (!jugadorDetectado && faseActual == 2)
        {
            faseActual = 1;
            agent.speed = speedPatrulla;
            Debug.Log("[Boss] Volviendo a patrullar");
        }
    }

    private void GoToNextWaypoint()
    {
        currentWaypoints++;
        if (currentWaypoints >= waypoints.Length)
            currentWaypoints = 0;
        agent.SetDestination(waypoints[currentWaypoints].position);
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        DañoJugador();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - TiempoUltimoDaño >= dañoCooldown)
            DañoJugador();
    }

    private void DañoJugador()
    {
        TiempoUltimoDaño = Time.time;

        
        if (animator != null)
            animator.SetTrigger(AnimAtacar);

        FindFirstObjectByType<PlayerHealth>()?.TakeDamage(daño);
        GameDataStructure.Instance.LogEvent($"Boss daño al jugador - fase {faseActual}");
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDettecionPatrulla);
        Gizmos.color = new Color(1f, 0.3f, 0f);
        Gizmos.DrawWireSphere(transform.position, deteccionAgresividad);
    }
}