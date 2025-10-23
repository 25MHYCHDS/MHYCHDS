using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerlandingState : PlayerGroundedState
{
    public PlayerlandingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {        
    }
    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = 0;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.LandingParameterHash);

        //stateMachine.Player.Input.gamePlayActions.Move.Disable();

        ReSetVolocity();
    }

    public override void Update()
    {
        if (stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            return;
        }
    }
    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.LandingParameterHash);

        //stateMachine.Player.Input.gamePlayActions.Move.Enable();
    }

    public override void OnAniTransationEvent()
    {
        base.OnAniTransationEvent();

        stateMachine.ChangeState(stateMachine.IdlingState);
    }
    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
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

        stateMachine.Player.Input.gamePlayActions.Move.started += OnMoveMoentStated;
    }
    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();

        stateMachine.Player.Input.gamePlayActions.Move.started -= OnMoveMoentStated;
    }

    protected override void OnLightAttack(InputAction.CallbackContext context)
    {
        InputBuffer.instance.AddInputBuffer(InputType.Attack);
    }

    private void OnMoveMoentStated(InputAction.CallbackContext context)
    {

    }

}
