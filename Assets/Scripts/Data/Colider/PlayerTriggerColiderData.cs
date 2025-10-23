using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerTriggerColiderData
{
    [field:SerializeField] public BoxCollider GroundCheckColider;

    public Vector3 GroundedCheckColliderExtents;

    public void Initialize()
    {
         GroundedCheckColliderExtents = GroundCheckColider.bounds.extents;
    }
}
