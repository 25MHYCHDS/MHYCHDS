using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColiderFloat
{
    public CColiderD CColiderD;
    [field: SerializeField] public DColiderD DColider;
    [field: SerializeField] public SlopeData slopeData;

    public void Initialize(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }
        CColiderD = new CColiderD();
        CColiderD.Initialize(gameObject);

    }
}
