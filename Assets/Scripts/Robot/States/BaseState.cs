public abstract class BaseState
{
    // The type of the state, used for identification in the state machine
    public RobotStateType Type { get; protected set; }
    
    // Reference to the RobotController, allowing access to its properties and methods
    protected RobotController robotController;

    // Constructor to initialize the state with the RobotController
    public BaseState(RobotController controller)
    {
        robotController = controller;
    }

    // Called when the state is entered
    public virtual void EnterState() { }

    // Called every frame during FixedUpdate, used for physics-related updates
    public virtual void UpdatePhysics() { }

    // Called every frame during Update, used for non-physics-related updates
    public virtual void UpdateLogic() { }

    // Called when the state is exited
    public virtual void ExitState() { }
}