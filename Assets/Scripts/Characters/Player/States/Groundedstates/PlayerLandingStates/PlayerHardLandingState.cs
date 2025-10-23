using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerlandingState
{
    public PlayerHardLandingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.HardLandingParameterHash);

        stateMachine.Player.Input.gamePlayActions.Move.Disable();

        ReSetVolocity();
    }
    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.HardLandingParameterHash);

        stateMachine.Player.Input.gamePlayActions.Move.Enable();
    }
    public override void OnAniTransationEvent()
    {
        stateMachine.Player.Input.gamePlayActions.Move.Enable();

        stateMachine.ChangeState(stateMachine.IdlingState);
    }
    protected override void onMove()
    {
        if (stateMachine.ReuseableData.ShouldWalk)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.RuningState);
    }
    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();

        stateMachine.Player.Input.gamePlayActions.Move.started += OnMveStarted;
    }

    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();

        stateMachine.Player.Input.gamePlayActions.Move.started += OnMveStarted;
    }

    private void OnMveStarted(InputAction.CallbackContext context)
    {
        //onMove();
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
}


