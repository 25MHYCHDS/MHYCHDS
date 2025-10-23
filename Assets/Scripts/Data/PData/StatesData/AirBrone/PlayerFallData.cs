using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerFallData 
{
    [field: SerializeField][field: Range(1f, 15f)] public float LimitedFallData = 15f;
    [field: SerializeField][field: Range(1f, 15f)] public float MinDisanceToHanrdStop = 3f;
}
