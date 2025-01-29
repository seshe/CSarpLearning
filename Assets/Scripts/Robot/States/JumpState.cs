using UnityEngine;
using System.Collections;

public class JumpState : BaseState
{
    private bool hasJumped;

    public JumpState(RobotController controller) : base(controller)
    {
        Type = RobotStateType.Jump;
    }

    public override void EnterState()
    {
        // Check if the robot is grounded before allowing a jump
        if (!robotController.IsGrounded()) return;

        robotController.Jump();
        hasJumped = true;
    }

    public override void UpdatePhysics()
    {
        if (!hasJumped) return;

        if (robotController.IsStableGrounded())
        {
            robotController.StartCoroutine(TransitionAfterLanding());
        }
    }

    private IEnumerator TransitionAfterLanding()
    {
        // Wait until the robot is grounded and stable
        yield return new WaitUntil(() => robotController.IsStableGrounded());

        // Calculate movement direction only when needed
        Vector3 movementDirection = robotController.GetMovementDirection();
        if (movementDirection.magnitude > 0.1f)
        {
            robotController.ChangeState(RobotStateType.Walk);
        }
        else
        {
            robotController.ChangeState(RobotStateType.Idle);
        }
        hasJumped = false;
    }
}