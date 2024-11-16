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
            // Bir sonraki waypoint’e geç
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
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
        }
    }

    public void SetAgentStatus()
    {
        agentActive = !agentActive;
        agent.enabled = agentActive;
    }


}
