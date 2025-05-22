using UnityEngine;
public class trigtestchase : MonoBehaviour
{





    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<prefabchase>().FollowPlayer();
        }
    }
}