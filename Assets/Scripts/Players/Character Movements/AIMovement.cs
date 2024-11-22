using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    public Transform[] waypoints; // List of waypoints for the AI to follow
    [SerializeField] private int currentWaypointIndex = 0; // Current waypoint index
    private NavMeshAgent agent;
    private Animator animator;

    public bool isAlive = true;  // AI alive state
    public bool isRunning = true;  // AI running state
    public bool agentActive = true;  // AI movement active state

    public Transform startingPosition; // Starting position of the AI
    private bool isAtFinalWaypoint = false; // Check if AI reached the final waypoint

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startingPosition = this.transform;
        waypoints[0] = startingPosition; // Set the first waypoint as the starting position

        // Start moving to the first waypoint if waypoints are defined
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            animator.SetBool("isRunning", isRunning);
        }
    }

    void Update()
    {
        // Stop AI movement during the painting wall phase
        if (GameManager.Instance.currentGameMode == GameMode.PaintingWall)
        {
            AICharacterIsDone();
        }

        // Check if the agent is active and not moving to a new path
        if (agentActive && isAlive && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Handle final waypoint or move to the next one
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                StopAtFinalWaypoint();
            }
            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    private void StopAtFinalWaypoint()
    {
        if (!isAtFinalWaypoint)
        {
            isAtFinalWaypoint = true;
            agent.isStopped = true; // Stop movement
            animator.SetBool("isRunning", false); // Stop running animation
        }
    }

    public void ResetAICharacter()
    {
        // Reset running animation and AI state
        animator.SetBool("isRunning", !isRunning);
        isRunning = !isRunning;

        isAlive = !isAlive;

        if (!isAlive)
        {
            currentWaypointIndex = 0;
            isAtFinalWaypoint = false;
        }
    }

    public void SetAgentStatus()
    {
        // Toggle agent's movement capability
        agentActive = !agentActive;
        agent.enabled = agentActive;
    }

    public void AICharacterIsDone()
    {
        // Disable the AI character after a short delay
        StartCoroutine(AICharacterIsDoneCoroutine());
    }

    private IEnumerator AICharacterIsDoneCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
