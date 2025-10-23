using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirbroneState 
{
    private PlayerFallData PlayerFallData;
    private Vector3 PlayerPositionOnEnter;
    public PlayerFallingState(PlayerMoveStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
    }
    public override void Enter()
    {
        PlayerPositionOnEnter = stateMachine.Player.transform.position;

        PlayerFallData = stateMachine.Player.Data.AirbroneData.playerFallData;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.fallParameterHash);

        stateMachine.ReuseableData.MovementSpeedModifier = 0;

        ReSetVerticalVolocity();
    }
    public override void Update()
    {

    }
    public override void PhysicsUpdate()
    {
        LimitVerticalVocelity();
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.fallParameterHash);
    }

    protected override void OnContractWithGround()
    {
        float fallDistance = PlayerPositionOnEnter.y - stateMachine.Player.transform.position.y;

        if (fallDistance < PlayerFallData.MinDisanceToHanrdStop)
        {
            stateMachine.ChangeState(stateMachine.LightLandStates);
            return;
        }

        if (stateMachine.ReuseableData.ShouldWalk && !stateMachine.ReuseableData.ShouldSprint || stateMachine.ReuseableData.MovementInput == Vector2.zero) 
        {
            stateMachine.ChangeState(stateMachine.HardLandStates);
            return;
        }

        stateMachine.ChangeState(stateMachine.RollingStates);
    }
    protected override void ResetShouldSprint()
    {

    }
    protected virtual void LimitVerticalVocelity()
    {
        Vector3 PlayerVerticalV = GetVerticalVelocity();

        if (PlayerVerticalV.y >= -PlayerFallData.LimitedFallData)
        { 
            return;
        }

        Vector3 AddForce = new Vector3(0f,-PlayerFallData.LimitedFallData - PlayerVerticalV.y,0f);

        stateMachine.Player.Rigidbody.AddForce(AddForce,ForceMode.VelocityChange);
    }
}
