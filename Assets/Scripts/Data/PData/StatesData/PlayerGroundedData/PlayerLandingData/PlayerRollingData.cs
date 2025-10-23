using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerRollingData
{
    [field: SerializeField][field: Range(1, 3)] public float SpeedModifer = 1f;

}
