using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerJumpingData 
{
    [field: SerializeField] public Vector3 PlayerRotationData;
    [field: SerializeField] public AnimationCurve JumpModifierOnSlophUpwards;
    [field: SerializeField] public AnimationCurve JumpModifierOnSlophDwonwards;
    [field: SerializeField][field: Range(0f, 5f)] public float JumpToGroundDistance;
    [field: SerializeField][field: Range(0f, 10f)] public float DecelerationForce = 1.5f;
    [field: SerializeField] public Vector3 StationAryForce;
    [field: SerializeField] public Vector3 WeakForce;
    [field: SerializeField] public Vector3 MediumForce;
    [field: SerializeField] public Vector3 StrongForce;
}
