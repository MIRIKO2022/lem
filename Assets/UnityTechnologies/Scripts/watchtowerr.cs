using Unity.Mathematics;
using UnityEngine;

public class WatchtowerLight : MonoBehaviour
{
    public Transform lightPivot; // The object that rotates (e.g., spotlight head)
    public float sweepAngle = 60f; // Total angle to rotate to each side
    public float rotationSpeed = 30f; // Speed of rotation
    public float detectionRange = 20f; // Raycast distance
    public LayerMask detectionLayer; // Assign to "Player" layer
 
    private float currentAngle = 0f;
    private float direction = 1f;
    public float yrotation;
    void Start()
    {
        
    }


    void Update()
    {
        // Sweep light back and forth
        float angleChange = rotationSpeed * Time.deltaTime * direction;
        currentAngle += angleChange;

        if (Mathf.Abs(currentAngle) > sweepAngle)
        {
            direction *= -1f; // Reverse direction
            currentAngle = Mathf.Clamp(currentAngle, -sweepAngle, sweepAngle);
        }

        lightPivot.localRotation = Quaternion.Euler(0, currentAngle+yrotation, 0);
        

        // Detect player with raycast
        RaycastHit hit;
        if (Physics.Raycast(lightPivot.position, lightPivot.forward, out hit, detectionRange, detectionLayer))
        {
            Debug.DrawLine(lightPivot.position, hit.point, Color.red); // Visualize
            Debug.Log("Player detected by watchtower!");
        }
        else
        {
            Debug.DrawRay(lightPivot.position, lightPivot.forward * detectionRange, Color.green);
        }
    }
}
