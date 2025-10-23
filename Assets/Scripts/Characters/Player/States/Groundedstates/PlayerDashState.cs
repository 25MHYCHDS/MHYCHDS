using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerMovingState
{
    public PlayerDashData DashData;
    public float startTime;
    public int DashedAmount;
    private bool ShouldKeepRotate;

    public PlayerDashState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
        DashData = GroundedData.DashData;
    }
    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = DashData.DishModifer;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.DashParameterHash);

        stateMachine.ReuseableData.rotationTime.y = DashData.DashRotationTime;

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StrongForce;

        ShouldKeepRotate = stateMachine.ReuseableData.MovementInput != Vector2.zero;

        Dash();

        UpdateDash();

        startTime = Time.time;
    }
    public override void Update()
    {

    }
    protected override void onMove()
    {

    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!ShouldKeepRotate)
        {
            return;
        }
        RotateToTagetDri();
    }
    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.DashParameterHash);

        SetBaseRotationData();
    }

    public override void OnAniTransationEvent()
    {
        if(stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);

            return;
        }
        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    public void Dash()
    {

        Vector3 DashDir = stateMachine.Player.transform.forward;

        DashDir.y = 0;

        UpdateTargetRotation(DashDir, false);

        if (stateMachine.ReuseableData.MovementInput != Vector2.zero) 
        {
            UpdateTargetRotation(GetMovementDir());

            DashDir = GetTargetRoatationDir(stateMachine.ReuseableData.currenTagetDri.y);
        }

        stateMachine.Player.Rigidbody.linearVelocity = DashDir * GetMoveMentSpeed(false);
    }
    private void UpdateDash()
    {
        if (!IsConsecutive())
        {
            DashedAmount = 0;
        }
        DashedAmount++;
        if(DashedAmount == DashData.DashLimit)
        {
            DashedAmount = 0;
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime + DashData.TimeToConsecutive;
    }

    //允许冲刺中转向
    protected override void AddInputCallBack()
    {
        base.AddInputCallBack();

        stateMachine.Player.Input.gamePlayActions.Move.performed += OnMovementPerformed;
    }
    protected override void RemoveInputCallBack() 
    { 
        base.RemoveInputCallBack();
        stateMachine.Player.Input.gamePlayActions.Move.performed -= OnMovementPerformed;
    }
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        ShouldKeepRotate = true;
    }
    protected override void OnLightAttack(InputAction.CallbackContext context)
    {
        InputBuffer.instance.AddInputBuffer(InputType.Attack);
    }
}
