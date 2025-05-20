using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour
{
    public Transform player;
    public WaypointPatrol waypointPatrol;
    public float resumePatrolDelay;
    bool m_IsPlayerInRange;
    Coroutine resumePatrolCoroutine;


    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
            Debug.Log("Player is in range of the enemy.");

            if (waypointPatrol != null)
            {
                waypointPatrol.FollowPlayer(player);
            }

            if (resumePatrolCoroutine != null)
            {
                StopCoroutine(resumePatrolCoroutine);
                resumePatrolCoroutine = null;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            Debug.Log("Player is no longer in range of the enemy.");

            if (waypointPatrol != null)
            {
                resumePatrolCoroutine = StartCoroutine(ResumePatrolAfterDelay());
            }
        }
    }

    IEnumerator ResumePatrolAfterDelay()
    {
        yield return new WaitForSeconds(resumePatrolDelay);
        if (!m_IsPlayerInRange && waypointPatrol != null)
        {
            waypointPatrol.ResumePatrol();
        }
    }


    void Update()
    {
        if (m_IsPlayerInRange && waypointPatrol != null)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    waypointPatrol.FollowPlayer(player);
                }
                else
                {
                    waypointPatrol.ResumePatrol();
                }
            }
        }
    }
}
