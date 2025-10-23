using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerLayerData 
{
    [field: SerializeField] public LayerMask GroundLayer;

    public bool LayerContain(LayerMask layerMask,int layer)
    {
        return (1 << layer & layerMask) != 0;
    }

    public bool IsGroundLayer(int Layer)
    {
        return LayerContain(GroundLayer,Layer);
    }
}
