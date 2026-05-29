using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VidaAtaque : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool muerto = false;

    [Header("Detección")]
    public Transform player;
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 2f;

    [Header("Patrulla")]
    public Transform[] waypoints;
    public float waypointDistance = 1f;
    private int currentWaypoint = 0;

    [Header("Ataque al jugador")]
    public int danio = 25;
    public float cooldownAtaque = 1.5f;
    private float ultimoAtaque = -999f;

    [Header("Velocidades")]
    public float velocidadPatrulla = 3f;
    public float velocidadPersecucion = 5f;

    private NavMeshAgent agent;
    private Animator anim;

   

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;

        if (waypoints.Length > 0)
        {
            agent.speed = velocidadPatrulla;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    private void Update()
    {
        if (muerto) return;

        float distancia = Vector3.Distance(
            transform.position, player.position);

        // Actualizar animación según velocidad
        if (anim != null)
            anim.SetFloat("Velocidad", agent.velocity.magnitude);

        if (distancia <= rangoAtaque)
        {
            agent.SetDestination(transform.position);
            TryAtacar();
        }
        else if (distancia <= rangoDeteccion)
        {
            agent.speed = velocidadPersecucion;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.speed = velocidadPatrulla;
            if (!agent.pathPending &&
                agent.remainingDistance <= waypointDistance)
                GoToNextWaypoint();
        }
    }



    private void GoToNextWaypoint()
    {
        currentWaypoint++;
        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }



    private void TryAtacar()
    {
        if (Time.time - ultimoAtaque < cooldownAtaque) return;
        ultimoAtaque = Time.time;

        FindFirstObjectByType<PlayerHealth>()
            ?.TakeDamage(danio);

        Debug.Log("[Enemigo] Atacó al jugador");
    }

  

    public void TakeDamage(int amount)
    {
        if (muerto) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        GameDataStructure.Instance.LogEvent(
            $"Enemigo golpeado: -{amount} | Vida: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

   

    private void Die()
    {
        if (muerto) return;
        muerto = true;

        agent.enabled = false;

        if (anim != null)
            anim.SetTrigger("Muerte");

        GameDataStructure.Instance.LogEvent("Enemigo derrotado");
        StartCoroutine(Desaparecer());
    }

    private IEnumerator Desaparecer()
    {
        
        yield return new WaitForSeconds(2f);

        
        if (anim != null)
            anim.enabled = false;

        
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}

