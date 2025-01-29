using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(RobotController controller) : base(controller)
    {
        Type = RobotStateType.Walk;
    }

    public override void EnterState()
    {
        // Set the walking animation parameter to true when entering the walk state
        robotController.SetAnimatorBool("IsWalking", true);
    }

    public override void UpdatePhysics()
    {
        // Move the robot
        robotController.Move(robotController.GetMovementDirection());

        // Rotate the robot
        robotController.Rotate(Input.GetAxis("Horizontal"), robotController.GetMovementDirection());
    }

    public override void UpdateLogic()
    {
        // Calculate movement direction only when needed
        Vector3 movementDirection = robotController.GetMovementDirection();
        if (Input.GetButtonDown("Jump") && robotController.IsGrounded())
        {
            robotController.ChangeState(RobotStateType.Jump);
        }
        else if (movementDirection.magnitude < 0.1f)
        {
            robotController.ChangeState(RobotStateType.Idle);
        }
    }

    public override void ExitState()
    {
        // Set the walking animation parameter to false when exiting the walk state
        robotController.SetAnimatorBool("IsWalking", false);
    }
}