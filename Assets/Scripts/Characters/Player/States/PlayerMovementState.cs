using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovementState : Istate
{
    protected PlayerMoveStateMachine stateMachine;
    protected PlayerGroundedData GroundedData;
    protected PlayerAirbroneData AirbroneData;
    public PlayerMovementState(PlayerMoveStateMachine playerMoveStateMachine)
    {
        stateMachine = playerMoveStateMachine;
        SetBaseCameraRecenterData();
    }

    protected void SetBaseCameraRecenterData()
    {
        GroundedData = stateMachine.Player.Data.GroundedData;
        AirbroneData = stateMachine.Player.Data.AirbroneData;
    }
    
    public void InitializeDate()
    {
        SetBaseRotationData();
    }

    public virtual void Enter()
    {
         Debug.Log("State:"+ GetType().Name);

         AddInputCallBack();

         InitializeDate();
    }

    public virtual void Exit()
    {
        RemoveInputCallBack();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        Move();
        ERotate();
    }

    public virtual void Update()
    {

    }
    public virtual void OnAnimationEnterEvent()
    {

    }

    public virtual void OnAnimationExitEvent()
    {

    }

    public virtual void OnAniTransationEvent()
    {

    }
    public void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Player.playerLayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContractWithGround();
            return;
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (stateMachine.Player.playerLayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContractWithGroundExit();
            return;
        }
    }


    //功能区————————————————————————————————————————————————————


    public void UpdateCameraRececnter(Vector2 movementInput)
    {
        if(movementInput == Vector2.zero)
        {
            return;
        }
        if(movementInput == Vector2.up)
        {
            DisableCameraRecenter();
            return;
        }

        //向后走时的相机recenter
        float CameraVirtiacalAngle = stateMachine.Player.MainCameraTransform.eulerAngles.x;

        if(CameraVirtiacalAngle > 270f)
        {
            CameraVirtiacalAngle -= 360f;
        }

        CameraVirtiacalAngle = Math.Abs(CameraVirtiacalAngle);

        if (movementInput == Vector2.down)
        {
            SetCameraReccenteringState(CameraVirtiacalAngle,GroundedData.BackWayCameraData);

            return;
        }
        SetCameraReccenteringState(CameraVirtiacalAngle, GroundedData.SideWayCameraData);
    }

    protected void SetCameraReccenteringState(float CameraVirtiacalAngle, List<PlayerRecenteringCameraData> backWayCameraData)
    {
        foreach (PlayerRecenteringCameraData RecenteringData in backWayCameraData)
        {
            if (!RecenteringData.IsWithInRange(CameraVirtiacalAngle))
            {
                continue;
            }
            EnableCameraRecenter(RecenteringData.RecentingTime, RecenteringData.WaitTime);

            return;
        }
        DisableCameraRecenter();
    }

    public void EnableCameraRecenter(float RecenteringTime,float WaitTime)
    {
        if (CameraManager.instance.CameraAimCoolDown == true)
        {
            float MovementSpeed = GetMoveMentSpeed();
            if (MovementSpeed == 0)
            {
                MovementSpeed = GroundedData.BaseSpeed;
            }

            stateMachine.Player.cameraUtility.EnableCameraRecentering(RecenteringTime, WaitTime, GroundedData.BaseSpeed, MovementSpeed);
        }
    }

    public void DisableCameraRecenter()
    {
        stateMachine.Player.cameraUtility.DisableCameraRecentering();
    }

    public void ERotate()
    {
        stateMachine.ReuseableData.countTime += Time.deltaTime;
        if (stateMachine.Player.Input.gamePlayActions.LRotate.ReadValue<float>() != 0)
        {
            stateMachine.ReuseableData.currenGroundTagetRotateAngle 
            -= GroundedData.RotateSpeed.Evaluate(stateMachine.ReuseableData.countTime) * 0.1f;
            Debug.Log(GroundedData.RotateSpeed.Evaluate(stateMachine.ReuseableData.countTime));
        }

        stateMachine.ReuseableData.countTime += Time.deltaTime;
        if (stateMachine.Player.Input.gamePlayActions.RRotate.ReadValue<float>() != 0)
        {
            stateMachine.ReuseableData.currenGroundTagetRotateAngle
            += GroundedData.RotateSpeed.Evaluate(stateMachine.ReuseableData.countTime) * 0.1f;
        }
        if(stateMachine.Player.Input.gamePlayActions.RRotate.ReadValue<float>() + stateMachine.Player.Input.gamePlayActions.LRotate.ReadValue<float>() 
            == 0f && (stateMachine.Player.Input.gamePlayActions.RRotate.ReadValue<float>() + stateMachine.Player.Input.gamePlayActions.LRotate.ReadValue<float>() != 2f))
        {
            stateMachine.ReuseableData.countTime = 0;
        }
    }

    //是否能Input————————————————————————————————————————————————
    //进入状态添加
    protected virtual void AddInputCallBack()
    {
        stateMachine.Player.Input.gamePlayActions.Move.canceled += OnMovementCanceled;

        stateMachine.Player.Input.gamePlayActions.Look.started += OnMouseMovementStart;

        stateMachine.Player.Input.gamePlayActions.Move.performed += OnMovementPerfermed;

        stateMachine.Player.Input.gamePlayActions.LRotate.performed += OnLRatatePerfermed;
        stateMachine.Player.Input.gamePlayActions.RRotate.performed += OnRRatatePerfermed;
    }
    protected void ReadMovementInput()
    {
        stateMachine.ReuseableData.MovementInput = stateMachine.Player.Input.gamePlayActions.Move.ReadValue<Vector2>();
    }
    //退出状态添加
    protected virtual void RemoveInputCallBack()
    {
        stateMachine.Player.Input.gamePlayActions.Move.canceled -= OnMovementCanceled;

        stateMachine.Player.Input.gamePlayActions.Look.started -= OnMouseMovementStart;

        stateMachine.Player.Input.gamePlayActions.Move.performed -= OnMovementPerfermed;

        stateMachine.Player.Input.gamePlayActions.LRotate.performed -= OnLRatatePerfermed;
        stateMachine.Player.Input.gamePlayActions.RRotate.performed -= OnRRatatePerfermed;

    }
    protected virtual void OnwalkToggleStart(InputAction.CallbackContext context)
    {
        stateMachine.ReuseableData.ShouldWalk = !stateMachine.ReuseableData.ShouldWalk;
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        DisableCameraRecenter();
    }
    private void OnMouseMovementStart(InputAction.CallbackContext context)
    {
        UpdateCameraRececnter(stateMachine.ReuseableData.MovementInput);
    }
    private void OnMovementPerfermed(InputAction.CallbackContext context)
    {
        UpdateCameraRececnter(context.ReadValue<Vector2>());
    }
    private void OnLRatatePerfermed(InputAction.CallbackContext context)
    {
        
    }
    private void OnRRatatePerfermed(InputAction.CallbackContext context)
    {
       
    }

    //旋转————————————————————————————————————————————————————
    public void SetBaseRotationData()
    {
        stateMachine.ReuseableData.rotationTime = GroundedData.BaseRotationData.RotateTime;
    }
    //将四元数转换为向量
    protected Vector3 GetTargetRoatationDir(float directionAngle)
    {
        if(stateMachine.ReuseableData.MovementInput.y > 0.1f)
        {
            return Quaternion.Euler(0f, directionAngle, 0f) * Vector3.up;
        }
        if (stateMachine.ReuseableData.MovementInput.y < -0.1f)
        {
            return Quaternion.Euler(0f, directionAngle, 0f) * Vector3.down;
        }
        else
        {
            return Quaternion.Euler(0f, directionAngle, 0f) * Vector3.forward;
        }

    }
    //获取目标角度并平滑旋转
    protected float Rotate(Vector3 TargetDirection,bool IsRotate = true)
    {
        float DirectionAngle = UpdateTargetRotation(TargetDirection);

        RotateToTagetDri();
        return DirectionAngle; 
    }
    //将移动输入信息转换为目标角度
    public virtual float GetTargetDirectionAngle(Vector3 TargetDirection)
    {
        float DirectionAngle = Mathf.Atan2(TargetDirection.x, TargetDirection.z) * Mathf.Rad2Deg;
        if (DirectionAngle < 0f)
        {
            DirectionAngle += 360f;
        }
        return DirectionAngle;
    }
    //平滑旋转到Data中的目标角度
    public virtual void RotateToTagetDri()
    {
        float CurrentAngleY = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;
        if (CurrentAngleY == stateMachine.ReuseableData.currenTagetDri.y)
        {
            return;
        }
        float SmoothingAngleY = Mathf.SmoothDampAngle(CurrentAngleY,
            stateMachine.ReuseableData.currenTagetDri.y,
            ref stateMachine.ReuseableData.rotationVelocity.y,
            stateMachine.ReuseableData.rotationTime.y -
            stateMachine.ReuseableData.rotationPassedTime.y);
        stateMachine.ReuseableData.rotationPassedTime.y += Time.deltaTime;

        Quaternion TagetRotation = Quaternion.Euler(0f, SmoothingAngleY, 0f);
        stateMachine.Player.Rigidbody.MoveRotation(TagetRotation);
    }
    //将目标角度信息存入Data
    protected virtual void UpdateTargetRotateData(float TargetAngle)
    {
        stateMachine.ReuseableData.currenTagetDri.y = TargetAngle;
        stateMachine.ReuseableData.rotationPassedTime.y = 0f;
    }
    //判断是否考虑相机和左右Offset
    public float UpdateTargetRotation(Vector3 TaegetDirection, bool ShoundConsiderCameraAngle = true)
    {
        float TargetDirectionAngle = GetTargetDirectionAngle(TaegetDirection);

        if (ShoundConsiderCameraAngle)
        {
            TargetDirectionAngle = AddCameraRotation(TargetDirectionAngle);
        }

        TargetDirectionAngle = AddRotateOffset(TargetDirectionAngle);

        if (stateMachine.ReuseableData.currenTagetDri.y != TargetDirectionAngle)
        {
            UpdateTargetRotateData(TargetDirectionAngle); 
        }
        return TargetDirectionAngle;
    }
    //添加相机相对角度
    private float AddCameraRotation(float angle)
    {
        angle += stateMachine.Player.MainCameraTransform.transform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }
        return angle;
    }
    //添加旋转左右移动Offset
    private float AddRotateOffset(float angle)
    {
        if (stateMachine.ReuseableData.MovementInput.y > 0.1f )
        {
            angle = 180 - angle; 
        }
        if (stateMachine.ReuseableData.MovementInput == Vector2.right)
        {
            angle += 30;
        }
        else 
        {
            if (stateMachine.ReuseableData.MovementInput == Vector2.left)
            {
                angle -= 30;
            }
        }

        if (angle > 360f)
        {
            angle -= 360f;
        }
        return angle;
    }

    //移动————————————————————————————————————————————————————

    protected void Move()
    {
        //Debug.Log(stateMachine.CurrentState);
        if (stateMachine.ReuseableData.MovementInput == Vector2.zero)
        {
            return;
        }
        else
        {
            float MovementSpeed = GetMoveMentSpeed();
            Vector3 Direction = GetMovementDir();
            float DirectionAngle = Rotate(Direction);
            //将四元数转换为向量
            Vector3 RotationDir = GetTargetRoatationDir(DirectionAngle);

            Vector3 CurrentV = stateMachine.Player.Rigidbody.linearVelocity;
           
            stateMachine.Player.Rigidbody.AddForce(RotationDir * MovementSpeed - CurrentV, ForceMode.VelocityChange);
        }

    }
    //Layer———————————————————————————————————————————————————
    protected virtual void OnContractWithGround()
    {
    }
    protected virtual void OnContractWithGroundExit()
    {
    }

    //其他————————————————————————————————————————————————————

    protected virtual void StartAnimation(int AnimationHash)
    {
        stateMachine.Player.Animator.SetBool(AnimationHash, true);
    }
    protected virtual void EndAnimation(int AnimationHash)
    {
        stateMachine.Player.Animator.SetBool(AnimationHash, false);
    }

    protected Vector3 GetMovementDir()
    {
        return new Vector3(stateMachine.ReuseableData.MovementInput.x, 0f, stateMachine.ReuseableData.MovementInput.y);
    }

    protected bool IsMovingUp(float Minivocility = 0.1f)
    {
        return GetVerticalVelocity().y > Minivocility;
    }
    protected bool IsMovingDwon(float Minivocility = 0.1f)
    {
        return GetVerticalVelocity().y < Minivocility;
    }
    protected void ReSetVerticalVolocity()
    {
        Vector3 PlayerHorizonVolocity = GetHorizontalVelocity();
        stateMachine.Player.Rigidbody.linearVelocity = PlayerHorizonVolocity;
    }
    protected void ReSetVolocity()
    {
        stateMachine.Player.Rigidbody.linearVelocity = Vector3.zero;
    }
    protected float GetMoveMentSpeed(bool shouldConsiderSlopes =true)
    {
        float movementSpeed = GroundedData.BaseSpeed * stateMachine.ReuseableData.MovementSpeedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed = GroundedData.BaseSpeed * stateMachine.ReuseableData.MovementSpeedModifier 
                          * stateMachine.ReuseableData.SlopeSpeedModifier;
        }

        return movementSpeed;
    }

    protected Vector3 GetVerticalVelocity()
    {
        return new Vector3(0f,stateMachine.Player.Rigidbody.linearVelocity.y, 0f);
    }

    protected Vector3 GetHorizontalVelocity() 
    {
        return new Vector3(stateMachine.Player.Rigidbody.linearVelocity.x, 0f, stateMachine.Player.Rigidbody.linearVelocity.z);
    }

    //缓慢减速
    protected void DecelerateHorizontally() 
    { 
        Vector3 playerHVelocity = GetHorizontalVelocity();
        stateMachine.Player.Rigidbody.AddForce(-playerHVelocity * stateMachine.ReuseableData.DecelerateModifier
        ,ForceMode.Acceleration);
    }
    protected void DecelerateVertically()
    {
        Vector3 playerVVelocity = GetVerticalVelocity();
        stateMachine.Player.Rigidbody.AddForce(-playerVVelocity * stateMachine.ReuseableData.DecelerateModifier
        , ForceMode.Acceleration);
    }
    protected bool IsMovingHorizontal(float minimunMagnitude = 0.1f)
    {
        Vector3 playerHVelocity = GetHorizontalVelocity();
        Vector2 playerHorizontalMovement = new Vector2(playerHVelocity.x, playerHVelocity.z);   

        return playerHorizontalMovement.magnitude > minimunMagnitude;
    }
}
