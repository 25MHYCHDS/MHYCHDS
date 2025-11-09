using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthContainer : HealthContainer
{
    public static PlayerHealthContainer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void InitHealth()
    {
        SetHealth(Player.instance.moveStateMachine.ReuseableData.PlayerDefaultHealth);
    }
    public override bool SetHealth(int value)
    {
        return base.SetHealth(value);
    }
    protected override void Die()
    {
        Debug.Log("Player is dead");
        gameObject.SetActive(false);
        //PauseMenu.GetComponent<PauseMenu>().Pause();

        SoundManager.instance.PlaySfx("Victory");
    }

}
