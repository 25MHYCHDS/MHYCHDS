using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerAnimationData 
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string GrounedParameterName = "Grounded";
    [SerializeField] private string MovingParameterName = "Moving";
    [SerializeField] private string StoppingParameterName = "Stopping";
    [SerializeField] private string LandingParameterName = "Landing";
    [SerializeField] private string AirborneParameterName = "Airborne";
    [SerializeField] private string AttackParameterName = "Attack";

   [Header("Grounded Parameter Names")]
    [SerializeField] private string IdleParameterName = "IsIdling";
    [SerializeField] private string DashParameterName = "IsDashing";
    [SerializeField] private string WalkParameterName = "IsWalking";
    [SerializeField] private string RunParameterName = "IsRuning";
    [SerializeField] private string SprintParameterName = "IsSprinting";
    [SerializeField] private string MediumStopParameterName = "IsMediumStopping";
    [SerializeField] private string HardStopParameterName = "IsHardStopping";
    [SerializeField] private string RollParameterName = "IsRolling";
    [SerializeField] private string HardLandingParameterName = "IsHardLanding";
    [SerializeField] private string fallParameterName = "Isfalling";

    [Header("Attack Parameter Names")]

    [SerializeField] private string Hit2ParameterName = "IsHit2";
    [SerializeField] private string Hit3ParameterName = "IsHit3";

    public int GroundedParameterHash;

    public int MovingParameterHash;

    public int StopingParameterHash;

    public int LandingParameterHash;

    public int AirborneParameterHash;

    public int IdleParameterHash;

    public int DashParameterHash;

    public int WalkParameterHash;

    public int RunParameterHash;

    public int SprintParameterHash;

    public int MediumStopParameterHash;

    public int HardStopParameterHash;

    public int RollParameterHash;

    public int HardLandingParameterHash;

    public int fallParameterHash;

    public int AttackParameterHash;

    public int Hit2ParameterHash;

    public int Hit3ParameterHash;

    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(GrounedParameterName);
        MovingParameterHash = Animator.StringToHash(MovingParameterName);
        StopingParameterHash = Animator.StringToHash(StoppingParameterName);
        LandingParameterHash = Animator.StringToHash(LandingParameterName);
        AirborneParameterHash = Animator.StringToHash(AirborneParameterName);
        IdleParameterHash = Animator.StringToHash(IdleParameterName);
        DashParameterHash = Animator.StringToHash(DashParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
        RunParameterHash = Animator.StringToHash(RunParameterName);
        SprintParameterHash = Animator.StringToHash(SprintParameterName);
        MediumStopParameterHash = Animator.StringToHash(MediumStopParameterName);
        HardStopParameterHash = Animator.StringToHash(HardStopParameterName);
        RollParameterHash = Animator.StringToHash(RollParameterName);
        HardLandingParameterHash = Animator.StringToHash(HardLandingParameterName);
        fallParameterHash = Animator.StringToHash(fallParameterName);
        AttackParameterHash = Animator.StringToHash(AttackParameterName);
        Hit2ParameterHash = Animator.StringToHash(Hit2ParameterName);
        Hit3ParameterHash = Animator.StringToHash(Hit3ParameterName);
    }
}
