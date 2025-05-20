using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    int m_CurrentWaypointIndex;
    bool isChasingPlayer;
    Transform playerTarget;

    void Start ()
    {
        navMeshAgent.SetDestination (waypoints[0].position);
    }

    void Update ()
    {
        if (isChasingPlayer && playerTarget != null)
        {
            navMeshAgent.SetDestination(playerTarget.position);
            return;
        }

        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination (waypoints[m_CurrentWaypointIndex].position);
        }
    }

    public void FollowPlayer(Transform player)
    {
        isChasingPlayer = true;
        playerTarget = player;
        navMeshAgent.SetDestination(player.position);
    }

    public void ResumePatrol()
    {
        isChasingPlayer = false;
        playerTarget = null;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
}