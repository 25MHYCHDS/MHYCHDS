using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlopeData
{
    [field: SerializeField][field:Range(0f, 1f)] public float SlopHighP = 0.25f;
    [field: SerializeField][field: Range(0f, 5f)] public float RayDistance = 2f;
    [field: SerializeField][field: Range(0f, 50f)] public float ReachPForce = 25f;
}
