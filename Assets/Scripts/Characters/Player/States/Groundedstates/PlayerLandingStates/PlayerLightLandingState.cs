using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightLandingState : PlayerlandingState
{
    public PlayerLightLandingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = 0f;
        stateMachine.ReuseableData.MovementInput = Vector2.zero;

        base.Enter();

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StationAryForce;

        ReSetVerticalVolocity();
    }
    public override void Update()
    {
        if (stateMachine.ReuseableData.MovementInput == Vector2.zero) 
        { 
            return;
        }
        onMove();
    }
    public override void OnAniTransationEvent()
    {
        base.OnAniTransationEvent();
    }
    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        stateMachine.ChangeState(stateMachine.IdlingState);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontal())
        {
            return;
        }
        ReSetVolocity();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
