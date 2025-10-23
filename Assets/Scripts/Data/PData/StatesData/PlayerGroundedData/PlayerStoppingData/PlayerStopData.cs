using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStopData 
{
    [field: SerializeField][field:Range(0f, 15f)] public float LightDecelarateForce = 5f;
    [field: SerializeField][field: Range(0f, 15f)] public float MediumDecelarateForce = 6.5f;
    [field: SerializeField][field: Range(0f, 15f)] public float HardDecelarateForce = 5f;
}
 