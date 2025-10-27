using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightAttackingState : PlayerGroundedState    
{
    private bool IsAiming = false;
    public float MaxDistance = 20;
    public PlayerLightAttackingState(PlayerMoveStateMachine moveStateMachine) : base(moveStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        IsAiming = Vector3.Distance(CameraManager.instance.EnemyLookPoint.transform.position, Player.instance.transform.position) < MaxDistance;

        if (IsAiming)
        {
            UpdateTargetRotateData(0f);
        }

        stateMachine.ReuseableData.DecelerateModifier = GroundedData.BaseStopData.HardDecelarateForce;
        stateMachine.ReuseableData.MovementSpeedModifier = stateMachine.ReuseableData.MovementSpeedModifier / 3;

        stateMachine.ReuseableData.LeftMouseClicks++;
        stateMachine.ReuseableData.LastClickTime = Time.time;
        stateMachine.ReuseableData.CanNextAttack = false;

        StartAnimation(stateMachine.Player.animationData.AttackParameterHash);
        if (stateMachine.ReuseableData.LeftMouseClicks == 1)
        {
            AddForce(20);
        }
        if (stateMachine.ReuseableData.LeftMouseClicks == 2)
        {
            StartAnimation(stateMachine.Player.animationData.Hit2ParameterHash);
            stateMachine.ReuseableData.CanNextAttack = false;
            AddForce(20);
        }
        if (stateMachine.ReuseableData.LeftMouseClicks == 3)
        {
            StartAnimation(stateMachine.Player.animationData.Hit3ParameterHash);
            stateMachine.ReuseableData.LeftMouseClicks = 0;
            stateMachine.ReuseableData.CanNextAttack = false;
            AddForce(20);
        }
    }

    public override void Update()
    {
        IsAiming = Vector3.Distance(CameraManager.instance.EnemyLookPoint.transform.position, Player.instance.transform.position) < MaxDistance;
    }

    

    public override void PhysicsUpdate()
    {
        ERotate();
        Float();
        if (IsAiming)
        {
            UpdateTargetRotateData(0f);
        }

        if (!IsMovingHorizontal())
        {
            return;
        }
        DecelerateHorizontally();
    }

    public override void Exit()
    {
        base.Exit();
        EndAnimation(stateMachine.Player.animationData.AttackParameterHash);
        EndAnimation(stateMachine.Player.animationData.Hit2ParameterHash);
        EndAnimation(stateMachine.Player.animationData.Hit3ParameterHash);
        stateMachine.ReuseableData.CanNextAttack = true;
    }

        public void AddForce(float force)
    {
        Vector3 AttackDir = new Vector3(GetTargetDirection().x, 0f, GetTargetDirection().y).normalized;
        if (!IsAiming)
        {
            AttackDir = stateMachine.Player.transform.forward;
        }
        else
        {
            AttackDir = new Vector3(GetTargetDirection().x, 0f, GetTargetDirection().y).normalized;
        }

        stateMachine.Player.Rigidbody.linearVelocity = AttackDir * force;
    }

    protected override void UpdateTargetRotateData(float TargetAngle)
    {
            if (CameraManager.instance.EnemyLookPoint != null)
            {
                float DirectionAngle = Mathf.Atan2(GetTargetDirection().x, GetTargetDirection().y) * Mathf.Rad2Deg;
                stateMachine.ReuseableData.currenTagetDri.y = DirectionAngle;
                stateMachine.ReuseableData.rotationPassedTime.y = 0f;
            }
            RotateToTagetDri();
    }

    public Vector3 GetTargetDirection()
    {
        Vector3 TargetDirection =  stateMachine.Player.transform.forward;
        if (CameraManager.instance.EnemyLookPoint != null)
        {
            Vector3 thirdDDir = CameraManager.instance.GetTargetDirection(CameraManager.instance.EnemyLookPoint.transform.localToWorldMatrix.GetPosition(),
            Player.instance.transform.position);
            TargetDirection = new Vector3(thirdDDir.x,0f, thirdDDir.z);
        }

        return TargetDirection;
    }

    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
    }

    public override void OnAniTransationEvent()
    {
        stateMachine.ReuseableData.CanNextAttack = true;
    }
    public override void RotateToTagetDri()
    {
        Quaternion TagetRotation = Quaternion.Euler(0f, stateMachine.ReuseableData.currenTagetDri.y, 0f);
        stateMachine.Player.Rigidbody.MoveRotation(TagetRotation);
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
        stateMachine.ReuseableData.CanNextAttack = true;
    }

    protected override void OnLightAttack(InputAction.CallbackContext context)
    {
        InputBuffer.instance.AddInputBuffer(InputType.Attack);

        if (stateMachine.ReuseableData.CanNextAttack == true)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }
}
