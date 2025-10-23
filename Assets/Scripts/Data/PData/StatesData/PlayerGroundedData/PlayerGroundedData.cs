using Assets.scripts.Characters.Player.PData.States.PlayerGroundedData.PlayerMoveData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundedData
{
    [field: SerializeField] [field:Range(0f,25f)] public float BaseSpeed = 5f;

    [field: SerializeField][field: Range(0f, 5f)] public float RayDistanceToFall = 1f;

    [field: SerializeField] public AnimationCurve SlopAngleSpeed;

    [field: SerializeField] public AnimationCurve RotateSpeed;

    [field: SerializeField] public PlayerDashData DashData;

    [field: SerializeField] public PlayerRotationData BaseRotationData;

    [field: SerializeField] public PlayerWalkData BaseWalkData;

    [field: SerializeField] public PlayerIdleData IdleWalkData;

    [field: SerializeField] public PlayerRunData BaseRunData;

    [field: SerializeField] public PlayerSprintData BaseSprintData;

    [field: SerializeField] public PlayerStopData BaseStopData;

    [field: SerializeField] public PlayerRollingData BaseRollingData;

    [field: SerializeField] public List<PlayerRecenteringCameraData> SideWayCameraData;

    [field: SerializeField] public List<PlayerRecenteringCameraData> BackWayCameraData;
}
