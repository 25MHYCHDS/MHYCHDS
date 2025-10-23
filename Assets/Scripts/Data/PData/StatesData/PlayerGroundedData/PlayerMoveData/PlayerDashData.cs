using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDashData
{
    [field: SerializeField]public float DashRotationTime = 0.02f;

    [field: SerializeField][field:Range(2f,6f)] public float DishModifer = 4f;
    [field: SerializeField][field: Range(0f, 2f)] public float TimeToConsecutive = 1f;
    [field: SerializeField][field: Range(1, 10)] public float DashLimit = 2;
    [field: SerializeField][field: Range(0f, 5f)] public float CoolDown = 2f;
}
