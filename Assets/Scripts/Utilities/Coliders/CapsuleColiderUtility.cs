using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CapsuleColiderUtility : ColiderFloat
{
    [field:SerializeField] public PlayerTriggerColiderData PlayerTriggerColiderData;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        PlayerTriggerColiderData.Initialize();
    }
}
