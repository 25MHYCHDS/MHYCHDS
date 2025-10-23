using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRuningState : PlayerMovingState
{
    private float startTime;
    private PlayerSprintData SprintData;
    public PlayerRuningState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine) 
    {
        SprintData = GroundedData.BaseSprintData;
    }
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.animationData.RunParameterHash);

        stateMachine.ReuseableData.MovementSpeedModifier = GroundedData.BaseRunData.SpeedModifier;

        startTime = Time.time;

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.MediumForce;
    }
    public override void Update()
    {

        base.Update();
        if (!stateMachine.ReuseableData.ShouldWalk)
        {
            return;
        }
        if(Time.time < startTime + SprintData.RunToWalkTime) 
        { 
            return;
        }
        StopRuning();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.RunParameterHash);
    }
    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();
    }
    protected override void RemoveInputCallBack()
    {
        base.RemoveInputCallBack();
    }

    //从步行切换为冲刺后，变为跑步，过0.5秒切换回步行
    private void StopRuning()
    {
        if(stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.WalkingState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.MediumStoppingState);

        base.OnMovementCanceled(context);
    }
    protected override void OnwalkToggleStart(InputAction.CallbackContext context)
    {
        base.OnwalkToggleStart(context);

        stateMachine.ChangeState(stateMachine.WalkingState);
    }
}

