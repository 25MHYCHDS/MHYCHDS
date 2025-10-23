using Unity.Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class PlayerCameraUtility 
{
    [field: SerializeField] CinemachineCamera CineCamera;
    [field: SerializeField] float DefaultHorizontalRecenteringtime = 4f;
    [field: SerializeField] float DefaultHorizontalWaitTime = 0f;

    private CinemachinePanTilt CinemachinePamTilt;

    public void Initialize()
    {
        CinemachinePamTilt = CineCamera.gameObject.GetComponent<CinemachinePanTilt>();
    }
    public void EnableCameraRecentering(float RecenteringTime = -1f, float WaitTime = -1f
        ,float baseMovementSpeed =1f ,float MovementSpeed =1f)
    {
        CinemachinePamTilt.PanAxis.Recentering.Enabled = true;

        if (RecenteringTime == -1f) 
        {
            RecenteringTime = DefaultHorizontalRecenteringtime;
        }
        if (WaitTime == -1f)
        {
            WaitTime = DefaultHorizontalWaitTime;
        }
        RecenteringTime = RecenteringTime * baseMovementSpeed / MovementSpeed;

        CinemachinePamTilt.PanAxis.Recentering.Time = RecenteringTime;

        CinemachinePamTilt.PanAxis.Recentering.Wait = WaitTime;
    }

    public void DisableCameraRecentering()
    {
        CinemachinePamTilt.PanAxis.Recentering.Enabled = false;
    }
}
