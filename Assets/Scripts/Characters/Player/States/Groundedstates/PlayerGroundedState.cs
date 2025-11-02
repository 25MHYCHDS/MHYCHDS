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

        Float();
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.GroundedParameterHash);
    }
    protected override void OnContractWithGroundExit()
    {
        if (IsThereGroundUnderNearth())
        {
            return ;
        }
        base.OnContractWithGroundExit();

        Vector3 CapsuleColiderCenterInWorld = stateMachine.Player.coliderFloat.CColiderD.Pcollider.bounds.center;

        Ray RayFromCapsuleColiderButton = new Ray(CapsuleColiderCenterInWorld - 
            stateMachine.Player.coliderFloat.CColiderD.ColliderVerticalExtent, Vector3.down);
    }

    private bool IsThereGroundUnderNearth()
    {
        BoxCollider GroundCheckColider = stateMachine.Player.coliderFloat.PlayerTriggerColiderData.GroundCheckColider;
        Vector3 GroundColiderInWorldSpaace = GroundCheckColider.bounds.center;

        Collider[] OverlapedGroundColider = Physics.OverlapBox(GroundColiderInWorldSpaace, 
            stateMachine.Player.coliderFloat.PlayerTriggerColiderData.GroundedCheckColliderExtents,GroundCheckColider.transform.rotation,stateMachine.Player.playerLayerData.GroundLayer);

        return OverlapedGroundColider.Length > 0;

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
    //½ºÄÒ¸¡¶¯
    public void Float()
    {
        Vector3 CColiderCenter = stateMachine.Player.coliderFloat.CColiderD.Pcollider.bounds.center;
        Ray DownRay = new Ray(CColiderCenter,Vector3.down);
        if(Physics.Raycast(DownRay,out RaycastHit hit,
            SlopeData.RayDistance,stateMachine.Player.playerLayerData.GroundLayer,
            QueryTriggerInteraction.Ignore))        
        {
            float GroundAngle = Vector3.Angle(hit.normal,- DownRay.direction);

            float slopeSpeedModfier = SetSlopeAngle(GroundAngle);
            
            if(slopeSpeedModfier == 0)
            { return; }

            float DistanceToCenter = SlopeData.RayDistance * stateMachine.Player.transform.localScale.y - hit.distance;
            
            if(DistanceToCenter == 0) 
            { return; }

            float LiftAmount = DistanceToCenter * SlopeData.ReachPForce - GetVerticalVelocity().y;
            Vector3 LiftFoce = new Vector3(0f,LiftAmount,0f);
            stateMachine.Player.Rigidbody.AddForce(LiftFoce,ForceMode.VelocityChange);
        }
    }

    private float SetSlopeAngle(float Angle)
    {
        float SlopeSpeedModfier = GroundedData.SlopAngleSpeed.Evaluate(Angle);

        UpdateCameraRececnter(stateMachine.ReuseableData.MovementInput);

        return SlopeSpeedModfier;
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

            Stamina.instance.stamina -= 0.5f;
            Stamina.instance.RecoverStamina = false;
            stateMachine.ChangeState(stateMachine.LightAttackStates);
        }
        else
        {

        }
    }
}
