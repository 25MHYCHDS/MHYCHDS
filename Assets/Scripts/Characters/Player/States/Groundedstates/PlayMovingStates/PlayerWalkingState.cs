using Assets.scripts.Characters.Player.PData.States.PlayerGroundedData.PlayerMoveData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    private PlayerWalkData playerWalkData;
    public PlayerWalkingState(PlayerMoveStateMachine playerMoveStateMachine) : base(playerMoveStateMachine)
    {
        playerWalkData = GroundedData.BaseWalkData;
    }

    public override void Enter()
    {
        stateMachine.ReuseableData.MovementSpeedModifier = GroundedData.BaseWalkData.SpeedModifier;

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.WalkParameterHash);


        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StationAryForce;

        stateMachine.ReuseableData.BackWayCameraData = playerWalkData.BackWayCameraData;
    }
    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.WalkParameterHash);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.Animator.GetBool(stateMachine.Player.animationData.AttackParameterHash) == false)
        {
            stateMachine.ChangeState(stateMachine.LightStoppingState);
        }
        base.OnMovementCanceled(context);
    }
    protected override void OnwalkToggleStart(InputAction.CallbackContext context)
    {
        base.OnwalkToggleStart(context);

        stateMachine.ChangeState(stateMachine.RuningState);
    }

}
