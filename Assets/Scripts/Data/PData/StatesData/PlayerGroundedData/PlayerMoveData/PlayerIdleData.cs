using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.Characters.Player.PData.States.PlayerGroundedData.PlayerMoveData
{
    [Serializable]
    public class PlayerIdleData 
    {
        [field: SerializeField] public List<PlayerRecenteringCameraData> BackWayCameraData;

    }
}