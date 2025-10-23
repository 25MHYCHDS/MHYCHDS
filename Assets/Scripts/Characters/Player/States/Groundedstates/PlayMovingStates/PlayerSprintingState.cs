using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData sprintData;
    private bool keepSprinting;
    public bool ShoudResetSprintState;
    private float startTime;
    public PlayerSprintingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
        sprintData = GroundedData.BaseSprintData;
    }
    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = sprintData.speedModifier;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.SprintParameterHash);

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StrongForce;

        startTime = Time.time;

        ShoudResetSprintState = true;
    }

    public override void Update()
    {
        if (keepSprinting)
        {
            return;
        }
        if(Time.time < startTime + sprintData.SprintToRunTime) 
        { 
            return;
        }
        StopSprinting();
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.SprintParameterHash);

        if (ShoudResetSprintState)
        {
            keepSprinting = false;

            stateMachine.ReuseableData.ShouldSprint = false;
        }
    }

    protected override void onMove()
    {
        
    }

    private void StopSprinting()
    {
        if(stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RuningState);
    }
    protected override void OnFall()
    {
        ShoudResetSprintState =false;
        base.OnFall();
    }
    //ÊäÈë
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;
        stateMachine.ReuseableData.ShouldSprint = true;
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.HardStoppingState);
        base.OnMovementCanceled(context);
    }
    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();
        stateMachine.Player.Input.gamePlayActions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();
        stateMachine.Player.Input.gamePlayActions.Sprint.performed -= OnSprintPerformed;
    }

}
