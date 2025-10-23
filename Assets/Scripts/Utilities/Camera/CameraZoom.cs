using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField][Range(0f,10f)] private float CameraDefautDistance = 6.0f;
    [SerializeField][Range(0f,10f)] private float CameraMaxDistance = 6.0f;
    [SerializeField][Range(0f,10f)] private float CameraMinDistance = 3.0f;
    [SerializeField][Range(0f, 10f)] private float Smoothing = 4.0f;
    [SerializeField][Range(0f, 10f)] private float ZoomSensitivity = 1.0f;
    private CinemachinePositionComposer PositionComposer;
    private float CurrentTDistance;

    private void Awake()
    {
        PositionComposer = GetComponent<CinemachinePositionComposer>();
        CurrentTDistance = CameraDefautDistance;
    }
    private void Update()
    {
        Zoom();
    }
    private void Zoom()
    {
        float ZoomValue = Player.instance.Input.gamePlayActions.Zoom.ReadValue<float>() * ZoomSensitivity;
        CurrentTDistance = Mathf.Clamp(CurrentTDistance + ZoomValue, CameraMinDistance, CameraMaxDistance);
        float CurrentDis = PositionComposer.CameraDistance;
        if (CurrentDis == CurrentTDistance)
        {
            return;
        }
        float LerpValue = Mathf.Lerp(CurrentDis, CurrentTDistance, Smoothing * Time.deltaTime);
        PositionComposer.CameraDistance = LerpValue;
    }
}
