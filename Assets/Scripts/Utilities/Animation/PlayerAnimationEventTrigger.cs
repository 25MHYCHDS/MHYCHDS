using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player Player;

    private void Awake()
    {
        Player = GetComponentInParent<Player>();
    }

    public void TriggerEnterAnimationEvent()
    {
        if (IsInAnimationTransation())
        {
            return;
        }

        Player.OnAnimationEnterEvent();
    }
    public void TriggerExitAnimationEvent()
    {
        if (IsInAnimationTransation())
        {
            return;
        }

        Player.OnAnimationExitEvent();
    }
    public void TriggerTransationAnimationEvent()
    {
        if (IsInAnimationTransation())
        {
            return;
        }

        Player.OnAnimationTransationEvent();
    }

    private bool IsInAnimationTransation(int LayerIndex = 0)
    {
        return Player.Animator.IsInTransition(LayerIndex);
    }
}
