using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine 
{
    public Istate CurrentState;
    public void ChangeState(Istate NewState)
    {
        CurrentState?.Exit();
        CurrentState = NewState;
        CurrentState.Enter();
    }
    public void HandleInput()
    {
        CurrentState?.HandleInput();
    }
    public void PhysicsUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }
    public void Update()
    {
        CurrentState?.Update();
    }
    public void OnAnimationEnterEvent()
    {
        CurrentState?.OnAnimationEnterEvent();
    }
    public void OnAnimationExitEvent()
    {
        CurrentState?.OnAnimationExitEvent();
    }
    public void OnAinTransationEvent()
    {
        CurrentState?.OnAniTransationEvent();
    }
    public void OntriggerEnter(Collider collider)
    {
        CurrentState?.OnTriggerEnter(collider);
    }
    public void OntriggerExit(Collider collider)
    {
        CurrentState?.OnTriggerExit(collider);
    }
}
