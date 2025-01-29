using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    [SerializeField] private RobotController robotController;
    [SerializeField] private float bobbingAmount = 0.05f;
    [SerializeField] private float bobbingSpeed = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;

        // Get the RobotController from the parent if not assigned
        if (robotController == null)
        {
            robotController = GetComponentInParent<RobotController>();
        }
    }

    private void Update()
    {
        // Check if the robot is moving using the GetMovementDirection() method
        if (robotController.GetMovementDirection().magnitude > 0.1f)
        {
            // Bobbing motion while moving
            float yOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            transform.localPosition = startPosition + new Vector3(0f, yOffset, 0f);
        }
        else
        {
            // Return to the starting position when the robot is not moving
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * bobbingSpeed);
        }
    }
}