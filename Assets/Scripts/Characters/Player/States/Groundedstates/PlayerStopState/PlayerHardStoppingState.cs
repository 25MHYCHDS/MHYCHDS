using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.animationData.HardStopParameterHash);

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.StrongForce;

        stateMachine.ReuseableData.DecelerateModifier = GroundedData.BaseStopData.HardDecelarateForce;
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.HardStopParameterHash);
    }

    protected override void onMove()
    {
        if(stateMachine.ReuseableData.ShouldWalk)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.RuningState);
    }
}
