using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field:Header("Collision")]
    [field: SerializeField] public CapsuleColiderUtility coliderFloat;

    [field: SerializeField] public PlayerLayerData playerLayerData;

    [field:Header("References")]
    [field:SerializeField] public PlayerSO Data;

    [field: Header("Cameras")]
    [field: SerializeField] public PlayerCameraUtility cameraUtility;

    [field: SerializeField] public PlayerAnimationData animationData;

    public PlayerInput Input;

    public Rigidbody Rigidbody;

    public Animator Animator;

    public Transform MainCameraTransform;

    public PlayerMoveStateMachine moveStateMachine;

    public ParticleSystem AtteackEffect;

    public static Player instance;
    private void Awake()
    {
        instance = this;

        Animator = GetComponentInChildren<Animator>();

        cameraUtility.Initialize();
        animationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        moveStateMachine = new PlayerMoveStateMachine(this);
        MainCameraTransform = Camera.main.transform;
    }
    private void OnValidate()
    {

    }
    private void Start()
    {
        moveStateMachine.ChangeState(moveStateMachine.IdlingState);
    }
    private void Update()
    {
        moveStateMachine.HandleInput();
        moveStateMachine.Update();
    }
    private void FixedUpdate()
    {
        moveStateMachine.PhysicsUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        moveStateMachine.OntriggerEnter(collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        moveStateMachine.OntriggerExit(collider);
    }

    public void OnAnimationEnterEvent()
    {
        moveStateMachine.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        moveStateMachine.OnAnimationExitEvent();
    }

    public void OnAnimationTransationEvent()
    {
        moveStateMachine.OnAinTransationEvent();
    }
}
