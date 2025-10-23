using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSprintData 
{
    [field: SerializeField][field:Range(1f,3f)] public float speedModifier = 1.7f;
    [field: SerializeField][field: Range(0f,5f)] public float SprintToRunTime = 1f;
    [field: SerializeField][field: Range(0f, 2f)] public float RunToWalkTime = 0.5f;
}
