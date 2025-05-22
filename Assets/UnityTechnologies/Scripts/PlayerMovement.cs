using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    public float turnSpeed = 20f;
    public float walkSpeed = 1f;             // base walking speed
    public float runSpeedMultiplier = 3f;    // how much faster when running

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

   void FixedUpdate()
{
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Transform cameraTransform = Camera.main.transform;
    Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
    Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;

    m_Movement = cameraForward * moveVertical + cameraRight * moveHorizontal;
    m_Movement.Normalize();

    bool hasHorizontalInput = !Mathf.Approximately(moveHorizontal, 0f);
    bool hasVerticalInput = !Mathf.Approximately(moveVertical, 0f);
    bool isWalking = hasHorizontalInput || hasVerticalInput;
    m_Animator.SetBool("IsWalking", isWalking);

    bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

    float currentSpeed = walkSpeed;
    if (isWalking && isRunning)
    {
        currentSpeed *= runSpeedMultiplier;
        m_Animator.speed = 1.5f;  // Speed up the walking animation by 50%
    }
    else
    {
        m_Animator.speed = 1f;    // Normal walking speed
    }

    if (isWalking)
    {
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        if (!m_AudioSource.isPlaying)
        {
            m_AudioSource.Play();
        }
    }
    else
    {
        m_AudioSource.Stop();
        m_Animator.speed = 1f;  // Ensure animation speed reset when idle
    }

    m_Movement *= currentSpeed;
}

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
