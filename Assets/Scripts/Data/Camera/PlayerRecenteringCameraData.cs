using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerRecenteringCameraData 
{
    [field: SerializeField][field: Range(0f, 360f)] public float MinimunAngel;

    [field: SerializeField][field: Range(0f, 360f)] public float MaximunAngel;

    [field: SerializeField][field: Range(-1f, 20f)] public float WaitTime = -1f;

    [field: SerializeField][field: Range(-1f, 20f)] public float RecentingTime =-1f;

    public bool IsWithInRange(float Angle)
    {
        return Angle >= MinimunAngel && Angle <= MaximunAngel;
    }
}
