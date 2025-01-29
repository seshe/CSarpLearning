using UnityEngine;

public class ProceduralLegAnimation : MonoBehaviour
{
    [Header("Movement Settings")]
    public RobotController robotController;  // Reference to your RobotController
    public float stepDistance = 0.5f;          // Distance the foot moves in one step
    public float stepHeight = 0.2f;            // Height of the foot lift during a step
    public float stepDuration = 0.5f;          // Duration of a single step
    public float footRotationSmoothTime = 0.1f; // Smoothing time for foot rotation

    [Header("Leg Configuration")]
    public Transform body;                    // Reference to the robot's body
    public Transform footTarget;              // Target position for the foot
    public Transform restingPosition;         // Foot's resting position (under the body)
    public LayerMask groundLayer;              // Ground layer

    private bool isMoving = false;
    private Vector3 newPosition;
    private Vector3 currentVelocity;
    private Quaternion targetFootRotation;

    private void Start()
    {
        newPosition = footTarget.position;
        targetFootRotation = footTarget.rotation;
    }

    private void Update()
    {
        // Smooth foot rotation
        footTarget.rotation = Quaternion.Slerp(footTarget.rotation, targetFootRotation, footRotationSmoothTime);
    }

    // Method called externally to initiate a step
    public void TryStartStep(Vector3 direction)
    {
        if (!isMoving && robotController.IsGrounded())
        {
            isMoving = true;
            StartCoroutine(StepCoroutine(direction));
        }
    }

    // Stop leg animation
    public void StopStepping()
    {
        isMoving = false;
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator StepCoroutine(Vector3 direction)
    {
        // Calculate the new foot position based on the robot's movement direction
        Vector3 startPosition = footTarget.position;

        RaycastHit hit;
        if (Physics.Raycast(restingPosition.position, Vector3.down, out hit, 10f, groundLayer))
        {
            newPosition = hit.point + direction * stepDistance;
            newPosition.y = hit.point.y;

            // Rotate the foot according to the surface normal
            targetFootRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * body.rotation;
        }

        // Raise and move the foot
        float elapsedTime = 0f;
        while (elapsedTime < stepDuration && isMoving)
        {
            float t = elapsedTime / stepDuration;
            float yOffset = Mathf.Sin(t * Mathf.PI) * stepHeight;

            footTarget.position = Vector3.SmoothDamp(footTarget.position, newPosition + Vector3.up * yOffset, ref currentVelocity, stepDuration - elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (isMoving)
            footTarget.position = newPosition;
        isMoving = false;
    }
}