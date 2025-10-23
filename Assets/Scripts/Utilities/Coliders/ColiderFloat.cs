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

        OnInitialize();
    }
    protected virtual void OnInitialize()
    {

    }
    public void CalculateDimension()
    {
        SetDColliderR(DColider.Radius);
        SetDColliderH(DColider.Height * (1-slopeData.SlopHighP));
        ReCalculateCenter();
        float halfHight = CColiderD.Pcollider.height/2;
        if (halfHight < CColiderD.Pcollider.radius)
        {
            SetDColliderR(halfHight);
        }
    }

    public void ReCalculateCenter()
    {
        float CenterOff = DColider.Height - CColiderD.Pcollider.height;
        Vector3 NewColliderC = new Vector3(0f, DColider.CenterY + (CenterOff / 2), 0f);
        CColiderD.Pcollider.center = NewColliderC;
    }

    private void SetDColliderR(float radius)
    {
        CColiderD.Pcollider.radius = radius;
    }
    private void SetDColliderH(float Height)
    {
        CColiderD.Pcollider.height = Height;
    }
}
