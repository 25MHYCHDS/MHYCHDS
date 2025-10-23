using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.Player.animationData.MovingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        EndAnimation(stateMachine.Player.animationData.MovingParameterHash);
    }
}
