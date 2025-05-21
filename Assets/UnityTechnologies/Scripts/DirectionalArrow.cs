using UnityEngine;
using System.Collections.Generic;

public class DirectionalArrow : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();

    private int currentTargetIndex = 0;

    public float targetReachedThreshold = 2.0f;

    public Transform playerObject;

    public float distanceFromPlayer = 1.5f;

    public float heightOffset = 0.0f;

    public bool lockYAxis = true;

    public Vector3 arrowRotationOffset = Vector3.zero;

    public Vector3 arrowForwardAxis = Vector3.forward;
    
    // Flag to determine if the current target should be collected or just reached
    public bool currentTargetIsCollectible = true;
      // Internal reference to the current target
    private Transform CurrentTarget => (targets != null && targets.Count > currentTargetIndex) ? targets[currentTargetIndex] : null;// Public method to be called when an item is collected
    public void OnItemCollected(Transform collectedItem)
    {
        // Check if the collected item is the current target or nearby
        if (CurrentTarget != null && Vector3.Distance(CurrentTarget.position, collectedItem.position) < 2.0f)
        {
            Debug.Log($"Item {currentTargetIndex} collected! Switching to next target.");
            MoveToNextTarget();
        }
        else
        {
            Debug.Log("Item collected but it's not the current target.");
        }
    }    void Start()
    {
        // Convert legacy targets if migrating from old DirectionalArrow
        // This section can be uncommented if you're upgrading from the original script
        /*
        DirectionalArrow oldArrow = GetComponent<DirectionalArrow>();
        if (oldArrow != null)
        {
            if (oldArrow.targetObject != null)
                targets.Add(oldArrow.targetObject);
                
            if (oldArrow.secondaryTarget != null)
                targets.Add(oldArrow.secondaryTarget);
                
            Destroy(oldArrow);
        }
        */

        // Add a tag to this GameObject for easier finding
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            // Note: You need to add "DirectionalArrow" to your Tags in Unity Editor
            // If the tag doesn't exist, this will cause an error
            try
            {
                gameObject.tag = "DirectionalArrow";
            }
            catch (System.Exception)
            {
                Debug.LogWarning("DirectionalArrowEnhanced: Please add 'DirectionalArrow' tag to your project tags");
            }
        }

        // Validate required components
        if (targets.Count == 0 || playerObject == null)
        {
            Debug.LogError("DirectionalArrowEnhanced: Target list empty or Player reference is missing!");
        }
    }

    void Update()
    {
        
        if (CurrentTarget == null || playerObject == null)
            return;

        // Check if player reached the current target
        CheckReachedTarget();

        // Position the arrow in front of the player with offset
        PositionArrowInFrontOfPlayer();

        // Make the arrow point towards the target
        PointArrowAtTarget();
    }    void CheckReachedTarget()
    {
        // Only check if we have a valid target
        if (CurrentTarget == null)
            return;

        // Calculate distance to target (ignoring Y axis for easier navigation)
        Vector3 playerPos = new Vector3(playerObject.position.x, 0, playerObject.position.z);
        Vector3 targetPos = new Vector3(CurrentTarget.position.x, 0, CurrentTarget.position.z);
        float distance = Vector3.Distance(playerPos, targetPos);

        // Check if player reached the current target
        if (distance <= targetReachedThreshold)
        {
            // If the current target is not a collectible (must be reached but not collected)
            // then move to the next target automatically
            if (!currentTargetIsCollectible)
            {
                Debug.Log($"Target {currentTargetIndex} reached! Switching to next target.");
                MoveToNextTarget();
            }
            else
            {
                // For collectibles, we wait for the OnItemCollected method to be called
                Debug.Log($"Target {currentTargetIndex} reached but needs to be collected!");
            }
        }
    }
    void MoveToNextTarget()
    {
        if (targets.Count > currentTargetIndex + 1)
        {
            // We have more targets, so increment the index to point to the next target
            currentTargetIndex++;
            Debug.Log($"Moving to next target. Current target index: {currentTargetIndex}");
        }
        else if (currentTargetIndex == targets.Count - 1)
        {
            // If this was the last target, reset to the first target
            currentTargetIndex = 0;
            Debug.Log("All targets reached! Resetting to first target.");
        }
        else
        {
            // This was the last target
            Debug.Log("Last target reached!");

            // Optionally, disable the arrow when reaching the last target
            gameObject.SetActive(false);
        }
    }

    void PositionArrowInFrontOfPlayer()
    {
        // Calculate position in front of the player with the specified offset
        Vector3 positionOffset = playerObject.forward * distanceFromPlayer;
        positionOffset.y += heightOffset;

        // Apply position
        transform.position = playerObject.position + positionOffset;
    }

    void PointArrowAtTarget()
    {
        if (CurrentTarget == null)
            return;

        // Get the direction from the arrow to the target
        Vector3 direction = CurrentTarget.position - transform.position;

        // If Y-axis is locked, zero out the Y component to keep the arrow horizontal
        if (lockYAxis)
        {
            direction.y = 0;
        }

        // Only update rotation if we have a valid direction
        if (direction.magnitude > 0.001f)
        {
            // Normalize the direction vector
            direction.Normalize();

            // Create a rotation that aligns the specified forward axis with the target direction
            Quaternion targetRotation;

            if (arrowForwardAxis == Vector3.forward)
            {
                // Standard case - arrow's local Z axis is its forward direction
                targetRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                // Custom forward axis case
                targetRotation = Quaternion.FromToRotation(arrowForwardAxis, direction);
            }

            // Apply 90-degree correction to Z axis
            Vector3 correctionRotation = new Vector3(0, 90, 0);

            // Apply any additional rotation offset (in case the arrow model isn't aligned properly)
            targetRotation *= Quaternion.Euler(arrowRotationOffset + correctionRotation);

            // Apply the final rotation
            transform.rotation = targetRotation;

            // Draw debug rays to help visualize the pointing direction
            Debug.DrawRay(transform.position, direction * 2f, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
        }
    }


}
