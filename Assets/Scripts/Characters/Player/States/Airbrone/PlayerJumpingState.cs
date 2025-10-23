using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingState : PlayerAirbroneState
{
    private PlayerJumpingData JumpingData;

    private bool ShouldKeepRotating;

    private bool CanStartFall = false;

    public PlayerJumpingState(PlayerMoveStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
        JumpingData = AirbroneData.playerJumpData;

        stateMachine.ReuseableData.rotationTime = JumpingData.PlayerRotationData;
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReuseableData.MovementSpeedModifier = 0f;

        stateMachine.ReuseableData.DecelerateModifier = JumpingData.DecelerationForce;

        ShouldKeepRotating = stateMachine.ReuseableData.MovementInput != Vector2.zero;

        Jump();
    }
    public override void Update()
    {
        if (!CanStartFall && IsMovingUp()) 
        {
            CanStartFall = true;
        }

        if (!CanStartFall || GetVerticalVelocity().y > 0f)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.FallingStates);
    }
    public override void PhysicsUpdate()
    {

        if (ShouldKeepRotating)
        {
            RotateToTagetDri();
        }

        if (IsMovingUp())
        {
            DecelerateVertically();
        }
    }
    public override void Exit()
    {
        base.Exit();

        SetBaseRotationData();

        CanStartFall = false;
    }
    private void Jump()
    {
        Vector3 JumpForce = stateMachine.ReuseableData.PlayerJumpForce;

        Vector3 JumpDir = stateMachine.Player.transform.forward;

        //跳跃后旋转速度更快

        if (ShouldKeepRotating)
        {
            UpdateTargetRotation(GetMovementDir());

            JumpDir = GetTargetRoatationDir(stateMachine.ReuseableData.currenTagetDri.y);
        }

        JumpForce.x *= JumpDir.x;
        JumpForce.z *= JumpDir.z;

        Vector3 CapsuleColiderCenterInWorld = stateMachine.Player.coliderFloat.CColiderD.ColliderCenter;
        Ray DownWardsFromColiderCenater = new Ray(CapsuleColiderCenterInWorld,Vector3.down);
        if (Physics.Raycast(DownWardsFromColiderCenater, out RaycastHit hit,JumpingData.JumpToGroundDistance,
            stateMachine.Player.playerLayerData.GroundLayer,QueryTriggerInteraction.Ignore))
        {
            float GroundAngle = Vector3.Angle(hit.normal,DownWardsFromColiderCenater.direction);
            if (IsMovingUp())
            {
                float ForceModifer = JumpingData.JumpModifierOnSlophUpwards.Evaluate(GroundAngle);
                JumpForce.x *= ForceModifer;
                JumpForce.z *= ForceModifer;
            }
            if (IsMovingDwon())
            {
                float ForceModifer = JumpingData.JumpModifierOnSlophDwonwards.Evaluate(GroundAngle);
                JumpForce.y *= ForceModifer;
            }
        }

        ReSetVolocity();

        stateMachine.Player.Rigidbody.AddForce(JumpForce, ForceMode.VelocityChange);
    }
    protected override void ResetShouldSprint()
    {
       
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
}
