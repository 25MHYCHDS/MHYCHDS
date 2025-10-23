using Assets.scripts.Characters.Player.PData.States.PlayerGroundedData.PlayerMoveData;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{

    private PlayerIdleData playerIdleData;
    public PlayerIdlingState(PlayerMoveStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
        playerIdleData =  GroundedData.IdleWalkData;
    }

    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.IdleParameterHash);

        ReSetVolocity();

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StationAryForce;

        stateMachine.ReuseableData.BackWayCameraData = playerIdleData.BackWayCameraData;
    }
    public override void Update()
    {
        base.Update();
        if(stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            return;
        }
        onMove();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontal())
        {
            return ;
        }
        ReSetVolocity();
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.IdleParameterHash);
    }
}
