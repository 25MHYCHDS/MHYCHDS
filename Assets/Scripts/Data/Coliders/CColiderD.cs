using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class CColiderD 
{
    public CapsuleCollider Pcollider;
    public Vector3 ColliderCenter;
    public Vector3 ColliderVerticalExtent;
    public void Initialize(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }
        Pcollider = new CapsuleCollider();
        Pcollider = gameObject.GetComponent<CapsuleCollider>();

    }
    public void UpdataColliderCenter()
    {
        ColliderCenter = Pcollider.center;
        ColliderVerticalExtent = new Vector3(0f,Pcollider.bounds.extents.y,0f);
    }
}
