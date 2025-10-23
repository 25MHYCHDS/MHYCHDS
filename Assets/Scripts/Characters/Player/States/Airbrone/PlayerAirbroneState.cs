using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirbroneState : PlayerMovementState
{
    public PlayerAirbroneState(PlayerMoveStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.animationData.AirborneParameterHash);

        ResetShouldSprint();
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.AirborneParameterHash);
    }

    protected override void OnContractWithGround()
    {
        stateMachine.ChangeState(stateMachine.LightLandStates);
    }

    protected virtual void ResetShouldSprint()
    {
        stateMachine.ReuseableData.ShouldSprint = false;
    }
}
