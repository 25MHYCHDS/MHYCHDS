using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerWalkData 
{
    [field: SerializeField][field:Range(0f, 2f)] public float SpeedModifier = 1f;

    [field: SerializeField] public List<PlayerRecenteringCameraData> BackWayCameraData;
}
