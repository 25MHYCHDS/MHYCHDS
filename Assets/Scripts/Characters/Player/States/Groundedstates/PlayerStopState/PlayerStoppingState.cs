using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = 0f;

        SetBaseCameraRecenterData();

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.StopingParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        RotateToTagetDri();

        if (!IsMovingHorizontal())
        {
            return;
        }
        DecelerateHorizontally();
    }

    public override void Exit()
    {
        base.Exit();
        EndAnimation(stateMachine.Player.animationData.StopingParameterHash);
    }

    public override void OnAniTransationEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    // ‰»Î
    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();
        stateMachine.Player.Input.gamePlayActions.Move.started += MoveStarted;
    }
    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();
        stateMachine.Player.Input.gamePlayActions.Move.started -= MoveStarted;
    }

    private void MoveStarted(InputAction.CallbackContext context)
    {
        //onMove();
    }

}
