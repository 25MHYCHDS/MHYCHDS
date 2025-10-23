using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAirbroneData 
{
    [field: SerializeField] public PlayerJumpingData playerJumpData;
    [field: SerializeField] public PlayerFallData playerFallData;
}
