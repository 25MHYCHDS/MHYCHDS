using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData SlopeData;
    public PlayerGroundedState(PlayerMoveStateMachine moveStateMachine): base(moveStateMachine)
    {
        SlopeData = stateMachine.Player.coliderFloat.slopeData;
    }
    public override void Enter()
    {
        base.Enter();
        
        StartAnimation(stateMachine.Player.animationData.GroundedParameterHash);

        UpdateShoudKeepSprint();

        UpdateCameraRececnter(stateMachine.ReuseableData.MovementInput);
    }

    public override void Update()
    {
        base.Update();
        UpdateBuffedInput();

        if (stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            return;
        }
        if (stateMachine.CurrentState != stateMachine.RuningState)
        {
            onMove();
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Time.time - stateMachine.ReuseableData.LastClickTime > stateMachine.ReuseableData.MaxComboDelay)
        {
            stateMachine.ReuseableData.LeftMouseClicks = 0;
            stateMachine.ReuseableData.CanNextAttack = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.GroundedParameterHash);
    }

    private void UpdateShoudKeepSprint()
    {
        if (!stateMachine.ReuseableData.ShouldSprint) 
        {
            return;
        }
        if (stateMachine.ReuseableData.MovementInput != Vector2.zero)
        {
            return ;
        }
        stateMachine.ReuseableData.ShouldSprint = false;
    }

    //Input»Øµ÷
    protected virtual void onMove()
    {

        if (stateMachine.ReuseableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);

            return;
        }
        if (stateMachine.ReuseableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);

            return;
        }
        stateMachine.ChangeState(stateMachine.RuningState);
    }

    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();
        stateMachine.Player.Input.gamePlayActions.LightAttack.started += OnLightAttack;
    }

    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();
        stateMachine.Player.Input.gamePlayActions.LightAttack.started -= OnLightAttack;
    }

    protected virtual void UpdateBuffedInput()
    {
        if (InputBuffer.instance.ConsumInputBuffer(InputType.Attack))
        {
            CameraManager.instance.Focus = true;
            if (stateMachine.ReuseableData.CanNextAttack)
            {
                Attack();
            }
        }
    }

    protected virtual void OnLightAttack(InputAction.CallbackContext context)
    {
        Attack();
    }
    protected void Attack()
    {
        if (stateMachine.ReuseableData.CanNextAttack && Stamina.instance.CheckCanAction(0.5f))
        {

            Stamina.instance.stamina -= 1f;
            Stamina.instance.RecoverStamina = false;
            stateMachine.ChangeState(stateMachine.LightAttackStates);
        }
        else
        {

        }
    }
}
