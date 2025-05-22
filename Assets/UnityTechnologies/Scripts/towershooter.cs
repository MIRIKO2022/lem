using UnityEngine;

public class towershooter : MonoBehaviour
{
    public Transform player;               // Assign the player object in the Inspector
    public GameObject projectilePrefab;    // Assign your bullet prefab
    public Transform firePoint;            // Where the bullet spawns (usually a child of the watchtower)
    public float rotationSpeed = 5f;       // How fast the tower rotates to face the player
    public float fireRate = 2f;            // Seconds between shots
    public float projectileForce = 10f;    // Speed of the projectile

    private float fireTimer = 0f;

    void Update()
    {
        if (player == null) return;

        // --- 1. Rotate toward the player ---
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Ignore vertical difference (optional for ground enemies)
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // --- 2. Fire timer countdown ---
        fireTimer -= Time.deltaTime;

        // --- 3. Shoot when timer reaches 0 ---
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
        }
    }
}
