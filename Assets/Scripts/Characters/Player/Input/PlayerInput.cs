using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Inputactions inputActions;
    public Inputactions.GamePlayActions gamePlayActions;
    private void Awake()
    {
        inputActions = new Inputactions();
        gamePlayActions = inputActions.GamePlay;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    public void DisableInput(InputAction action, float delay)
    {
        StartCoroutine(DisableAction(action,delay));
    }
    IEnumerator DisableAction(InputAction action, float delay)
    {
        action.Disable();
        yield return new WaitForSeconds(delay);
        action.Enable();
    }
}
