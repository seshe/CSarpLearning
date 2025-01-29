using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class RobotController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float brakeDrag = 5f;
    [SerializeField] private float normalDrag = 0f;
    [SerializeField] private float slopeSpeedMultiplier = 0.7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation Settings")]
    [SerializeField] private float autoRotationSmoothTime = 0.2f;
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] private bool useProceduralAnimation = true;
    [SerializeField] private List<ProceduralLegAnimation> legs;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [Range(0, 1)]
    [SerializeField] private float jumpVolume = 0.7f;
    [SerializeField] private AudioSource audioSource;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private float autoRotationVelocity;
    private Vector3 groundNormal = Vector3.up;
    private Transform cachedTransform;
    private Vector3 direction;

    // State Management
    private Dictionary<RobotStateType, BaseState> stateDictionary;
    public BaseState CurrentState { get; private set; }

    // Ground Check Optimization
    private float lastGroundCheckTime;
    private const float groundCheckInterval = 0.1f;

    // Pending Force
    private Vector3 pendingForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cachedTransform = transform;

        // Freeze rotation on X and Z axes to prevent unwanted tilting
        rb.freezeRotation = true;

        // Ensure audioSource is not null
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize states
        InitializeStates();
    }

    private void Start()
    {
        // Set initial state
        ChangeState(RobotStateType.Idle);
    }

    private void Update()
    {
        CurrentState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        // Get input from InputManager
        float horizontalInput = InputManager.Instance.GetHorizontalAxis();
        float verticalInput = InputManager.Instance.GetVerticalAxis();

        // Calculate movement direction
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if the robot is grounded
        if (Time.time - lastGroundCheckTime > groundCheckInterval)
        {
            UpdateGroundedStatus();
            lastGroundCheckTime = Time.time;
        }

        CurrentState.UpdatePhysics();

        // Apply slope gravity
        ApplySlopeGravity();

        // Apply pending force
        if (pendingForce != Vector3.zero)
        {
            rb.AddForce(pendingForce, ForceMode.Impulse);
            pendingForce = Vector3.zero;
        }

        // Control leg animation
        if (useProceduralAnimation)
        {
            if (direction.magnitude > 0.1f)
            {
                foreach (var leg in legs)
                {
                    leg.TryStartStep(direction);
                }
            }
            else
            {
                foreach (var leg in legs)
                {
                    leg.StopStepping();
                }
            }
        }
    }

    private void InitializeStates()
    {
        stateDictionary = new Dictionary<RobotStateType, BaseState>
        {
            { RobotStateType.Idle, new IdleState(this) },
            { RobotStateType.Walk, new WalkState(this) },
            { RobotStateType.Jump, new JumpState(this) }
        };
    }

    public void ChangeState(RobotStateType newStateType)
    {
        if (CurrentState?.Type == newStateType) return;

        if (stateDictionary.ContainsKey(newStateType))
        {
            BaseState newState = stateDictionary[newStateType];
            if (CurrentState != null)
            {
                CurrentState.ExitState();
            }

            CurrentState = newState;
            CurrentState.EnterState();
        }
        else
        {
            Debug.LogError("State " + newStateType + " does not exist in the dictionary.");
        }
    }

    public void Move(Vector3 direction)
    {
        float currentMoveSpeed = moveSpeed;

        if (direction.magnitude < 0.1f || IsSlopeWalkable())
        {
            // Apply braking drag when there is no input or on steep slope
            rb.drag = brakeDrag;
            return;
        }

        // Remove braking drag when moving
        rb.drag = normalDrag;

        // Reduce speed on slopes
        if (IsOnSteepSlope())
        {
            currentMoveSpeed *= slopeSpeedMultiplier;
        }

        // Calculate target velocity
        Vector3 targetVelocity = cachedTransform.forward * direction.z * currentMoveSpeed + cachedTransform.right * direction.x * currentMoveSpeed;
        targetVelocity.y = rb.velocity.y;

        // Apply force to reach the target velocity
        rb.AddForce(targetVelocity - rb.velocity, ForceMode.VelocityChange);

        // Limit horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > currentMoveSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * currentMoveSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    public void Rotate(float horizontalInput, Vector3 movementDirection)
    {
        // Manual rotation
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        // Autorotation towards movement direction
        else if (movementDirection.magnitude > 0.1f)
        {
            float targetRotation = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
            float smoothedRotation = Mathf.SmoothDampAngle(cachedTransform.eulerAngles.y, targetRotation, ref autoRotationVelocity, autoRotationSmoothTime);
            rb.rotation = Quaternion.Euler(0f, smoothedRotation, 0f);
        }
    }

    public void Jump()
    {
        // Add jump force to the Rigidbody
        AddPendingForce(Vector3.up * jumpForce);

        // Play jump sound
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }

    private void UpdateGroundedStatus()
    {
        // Use CheckSphere to detect ground contact
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // Get ground normal
        RaycastHit hit;
        if (Physics.Raycast(cachedTransform.position, Vector3.down, out hit, groundCheckRadius * 2f, groundLayer))
        {
            groundNormal = hit.normal;
            // Debug.DrawRay(hit.point, hit.normal, Color.green, 1f);
        }
        else
        {
            groundNormal = Vector3.up; // Reset to up if no ground is detected
        }
    }

    private bool IsOnSteepSlope()
    {
        if (!isGrounded) return false;
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        return slopeAngle > maxSlopeAngle;
    }

    private bool IsSlopeWalkable()
    {
        if (!isGrounded) return false;
        if (direction.magnitude < 0.1f) return false;
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        return slopeAngle > maxSlopeAngle;
    }

    private void ApplySlopeGravity()
    {
        if (isGrounded && direction.magnitude < 0.1f && !IsSlopeWalkable())
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal);
            rb.AddForce(slopeDirection * 5f, ForceMode.Acceleration);
        }
    }

    public void SetAnimatorBool(string param, bool value)
    {
        if (animator != null)
            animator.SetBool(param, value);
    }

    public void SetAnimatorFloat(string param, float value, float dampTime = 0.1f)
    {
        if (animator != null)
            animator.SetFloat(param, value, dampTime, Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsStableGrounded() => isGrounded && rb.velocity.y <= 0.1f;

    public void AddPendingForce(Vector3 force)
    {
        pendingForce += force;
    }

    public Vector3 GetMovementDirection()
    {
        return direction;
    }
}