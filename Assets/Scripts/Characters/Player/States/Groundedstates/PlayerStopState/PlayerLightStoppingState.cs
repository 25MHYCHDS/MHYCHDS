using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReuseableData.DecelerateModifier = GroundedData.BaseStopData.LightDecelarateForce;
    }
}
