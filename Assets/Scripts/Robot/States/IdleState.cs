using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(RobotController controller) : base(controller)
    {
        Type = RobotStateType.Idle;
    }

    public override void EnterState()
    {
        // Any logic to execute when entering the idle state
    }

    public override void UpdateLogic()
    {
        // Calculate movement direction only when needed
        Vector3 movementDirection = robotController.GetMovementDirection();

        if (Input.GetButtonDown("Jump") && robotController.IsGrounded())
        {
            robotController.ChangeState(RobotStateType.Jump);
        }
        else if (movementDirection.magnitude > 0.1f)
        {
            robotController.ChangeState(RobotStateType.Walk);
        }
    }
}