using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public LayerMask collisionLayers;

    public float mouseSensitivity = 3.5f;
    public float positionSmooth = 10f, rotationSmooth = 10f;
    public float minDistance = 1.5f, maxDistance = 5f;
    public float idleDelay = 1f;

    public KeyCode recenterKey;
    public float recenterPitch = 10f, recenterSpeed = 10f;

    public float heightOffset = 1.5f;

    Vector3 offset;
    float yaw, pitch, idleTimer;
    bool recentering;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offset = transform.position - player.position;
        var angles = Quaternion.LookRotation(-offset).eulerAngles;
        yaw = angles.y; pitch = angles.x;
    }

    void LateUpdate()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        bool mouseMoved = Mathf.Abs(mx) > 0.01f || Mathf.Abs(my) > 0.01f;

        if (Input.GetKeyDown(recenterKey))
            recentering = true;

        if (recentering)
        {
            float targetYaw = player.eulerAngles.y;
            yaw   = Mathf.LerpAngle(yaw,   targetYaw,   Time.deltaTime * recenterSpeed);
            pitch = Mathf.Lerp(pitch, recenterPitch, Time.deltaTime * recenterSpeed);
            if (Mathf.Abs(Mathf.DeltaAngle(yaw, targetYaw)) < 0.5f &&
                Mathf.Abs(pitch - recenterPitch) < 0.5f)
                recentering = false;
        }
        else if (mouseMoved)
        {
            idleTimer = 0f;
            yaw += mx; pitch -= my;
        }

        pitch = Mathf.Clamp(pitch, -40f, 60f);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 dir = rot * Vector3.back;
        Vector3 desiredPos = player.position + Vector3.up * heightOffset
                             + dir * maxDistance;

        if (Physics.Linecast(player.position + Vector3.up * heightOffset, 
                             desiredPos, out var hit, collisionLayers))
        {
            float dist = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance);
            desiredPos = player.position + Vector3.up * heightOffset + dir * dist;
        }

        transform.position = Vector3.Slerp(transform.position, desiredPos,
                                           1 - Mathf.Exp(-positionSmooth * Time.deltaTime));
        transform.rotation = Quaternion.Slerp(transform.rotation, rot,
                                             1 - Mathf.Exp(-rotationSmooth * Time.deltaTime));
    }
}
// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform player;

//     private Vector3 offset;

//     void Start()
//     {
//         offset = transform.position - player.position;
//     }

//     void LateUpdate()
//     {
//         transform.position = player.position + offset;
//     }
// }
