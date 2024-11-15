using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{

    public Transform[] waypoints; // Waypoint listesi
    private int currentWaypointIndex = 0; // Hangi waypoint’te olduðunu tutar
    private NavMeshAgent agent;
    private Animator animator;
    public bool isAlive = true;

    private bool isRunning = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            animator.SetBool("isRunning", isRunning);
        }
    }

    void Update()
    {
        if (!isAlive)
        {
            agent.isStopped = true; // AI durur
            return;
        }

        agent.isStopped = false; // AI hareket eder

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Bir sonraki waypoint’e geç
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public void ResetAICharacter()
    {
        animator.SetBool("isRunning", !isRunning); // Koþma animasyonu durdurulur
        isRunning = !isRunning;

        if (isAlive == false) currentWaypointIndex = 0;

        isAlive = !isAlive; // isAlive durumunu tersine çevirir
    }
}
