using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.animationData.MediumStopParameterHash);

        stateMachine.ReuseableData.PlayerJumpForce = AirbroneData.playerJumpData.MediumForce;

        stateMachine.ReuseableData.DecelerateModifier = GroundedData.BaseStopData.MediumDecelarateForce;
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.MediumStopParameterHash);
    }
}
