using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Istate 
{
    public void Enter();
    public void Update();
    public void PhysicsUpdate();
    public void Exit();
    public void HandleInput();
    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAniTransationEvent();
    public void OnTriggerEnter(Collider collider);
    public void OnTriggerExit(Collider collider);
}
