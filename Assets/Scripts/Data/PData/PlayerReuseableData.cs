using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReuseableData
{
    public Vector2 MovementInput;
    public float MovementSpeedModifier = 2f;
    public float SlopeSpeedModifier = 1f;
    public float DecelerateModifier = 1f;
    public float LastClickTime;
    public float MaxComboDelay = 2f;
    public int LeftMouseClicks = 0;

    //Íæ¼Ò´¹Ö±Ðý×ª
    public float currenGroundTagetRotateAngle = 0;
    public float countTime;
    public float PRatateSpeed = 0.1f;
    public float BaseRatateSpeed = 1;

    public List<PlayerRecenteringCameraData> SideWayCameraData;

    public List<PlayerRecenteringCameraData> BackWayCameraData;

    public bool ShouldWalk = false;
    public bool ShouldSprint = false;
    public bool CanNextAttack = true;
    public bool CanChageCharacter = false;

    public Vector3 currenTagetDri;
    public Vector3 rotationTime;
    public Vector3 rotationVelocity;
    public Vector3 rotationPassedTime;

    public PlayerReuseableData(PlayerMoveStateMachine playerMoveStateMachine)
    {
    }

    public ref Vector3 CurrenTagetDri
    {
        get
        {
            return ref currenTagetDri;
        }
    }
    public ref Vector3 RotationTime
    {
        get
        {
            return ref rotationTime;
        }
    }
    public ref Vector3 RotationVelocity
    {
        get
        {
            return ref rotationVelocity;
        }
    }
    public ref Vector3 RotationPassedTime
    {
        get
        {
            return ref rotationPassedTime;
        }
    }

    public Vector3 PlayerJumpForce;
}
