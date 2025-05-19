using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;

    bool m_IsPlayerInRange;

    public LevelEnding levelEnding;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
            Debug.Log("Player is in range of the enemy.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
            Debug.Log("Player is no longer in range of the enemy.");
        }
    }
    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up; //Vector3.up is a shortcut for (0, 1, 0)
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    levelEnding.CaughtPlayer();
                    Debug.Log("Player is caught by the enemy.");
                }
            }
        }
    }
}