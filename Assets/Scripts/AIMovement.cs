using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{

    public Transform[] waypoints; // Waypoint listesi
    [SerializeField] private int currentWaypointIndex = 0; // Hangi waypoint’te olduðunu tutar
    private NavMeshAgent agent;
    private Animator animator;

    public bool isAlive = true;

    public bool isRunning = true;

    public bool agentActive = true;

    public Transform startingPosition;

    private bool isAtFinalWaypoint = false; // Son waypoint kontrolü


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startingPosition = this.transform;

        waypoints[0] = startingPosition;

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            animator.SetBool("isRunning", isRunning);
        }
    }

    void Update()
    {

        if (agentActive && isAlive && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                // Son waypoint'e ulaþýldý
                StopAtFinalWaypoint();
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);

            }
            // Bir sonraki waypoint’e geç
        }
    }

    private void StopAtFinalWaypoint()
    {
        if (!isAtFinalWaypoint)
        {
            isAtFinalWaypoint = true;
            agent.isStopped = true; // Hareketi durdur
            animator.SetBool("isRunning", false); // Koþma animasyonu durdurulur
        }
    }

    public void ResetAICharacter()
    {

        animator.SetBool("isRunning", !isRunning); // Koþma animasyonu durdurulur
        isRunning = !isRunning;

        isAlive = !isAlive;

        if (isAlive == false)
        {
            currentWaypointIndex = 0;
            isAtFinalWaypoint = false;
        }
    }

    public void SetAgentStatus()
    {
        agentActive = !agentActive;
        agent.enabled = agentActive;
    }


}
