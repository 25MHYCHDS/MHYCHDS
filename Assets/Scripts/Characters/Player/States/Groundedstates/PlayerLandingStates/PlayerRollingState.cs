using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollingState : PlayerlandingState
{
    private PlayerRollingData RollingData;
    public PlayerRollingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
        RollingData = GroundedData.BaseRollingData;
    }
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReuseableData.ShouldSprint = false;

        stateMachine.ReuseableData.MovementSpeedModifier = RollingData.SpeedModifer;

        StartAnimation(stateMachine.Player.animationData.RollParameterHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.ReuseableData.MovementInput != Vector2.zero)
        {
            return;
        }
        RotateToTagetDri();
    }
    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.RollParameterHash);
    }

    public override void OnAniTransationEvent()
    {
        if (stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);

            return;
        }

        onMove();
    }
}
