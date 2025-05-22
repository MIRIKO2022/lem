using UnityEngine;
using UnityEngine.AI;

public class prefabchase : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    private int m_CurrentWaypointIndex;
    private bool isChasingPlayer;
    private Transform playerTarget;

    void Start()
    {
        // Automatically find the player in the scene by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }

        // Start patrol
        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[0].position);
        }
    }

    void Update()
    {
        if (isChasingPlayer && playerTarget != null)
        {
            navMeshAgent.SetDestination(playerTarget.position);
            return;
        }

        // Patrol between waypoints
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }

    // Call this to start chasing the player
    public void FollowPlayer()
    {
        if (playerTarget == null) return;

        isChasingPlayer = true;
        navMeshAgent.SetDestination(playerTarget.position);
    }

    // Call this to resume patrolling after chasing
    public void ResumePatrol()
    {
        isChasingPlayer = false;
        if (waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}
