using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateMachine : StateMachine
{
    public Player Player;
    public PlayerIdlingState IdlingState;
    public PlayerWalkingState WalkingState;
    public PlayerRuningState RuningState;
    public PlayerDashState DashState;
    public PlayerSprintingState SprintingState;
    public PlayerLightStoppingState LightStoppingState;
    public PlayerMediumStoppingState MediumStoppingState;
    public PlayerHardStoppingState HardStoppingState;
    public PlayerReuseableData ReuseableData;
    public PlayerJumpingState JumpingStates;
    public PlayerFallingState FallingStates;
    public PlayerlandingState LandingStates;
    public PlayerLightLandingState LightLandStates;
    public PlayerHardLandingState HardLandStates;
    public PlayerRollingState RollingStates;
    public PlayerLightAttackingState LightAttackStates;

    public PlayerMoveStateMachine(Player player)
    {
        Player = player;
        DashState = new PlayerDashState(this);
        ReuseableData = new PlayerReuseableData(this);
        IdlingState = new PlayerIdlingState(this);
        WalkingState = new PlayerWalkingState(this);
        RuningState = new PlayerRuningState (this);
        SprintingState = new PlayerSprintingState (this);
        LightStoppingState = new PlayerLightStoppingState (this);
        MediumStoppingState = new PlayerMediumStoppingState (this);
        HardStoppingState = new PlayerHardStoppingState (this);
        JumpingStates = new PlayerJumpingState(this);
        FallingStates = new PlayerFallingState(this);
        LandingStates = new PlayerlandingState(this);
        LightLandStates = new PlayerLightLandingState(this);
        HardLandStates = new PlayerHardLandingState(this);
        RollingStates = new PlayerRollingState(this);
        LightAttackStates = new PlayerLightAttackingState(this);
    }
    
}
    
