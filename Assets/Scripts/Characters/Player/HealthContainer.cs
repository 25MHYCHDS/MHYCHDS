using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthContainer : MonoBehaviour
{
    public int health = 5;
    private int maxHealth = 5;
    public int Health => health;
    public int MaxHealth => maxHealth;
    /// <summary>
    /// 濒死时执行，若该事件结束之后生命值仍小于等于0，则该生物死亡
    /// </summary>
    public event Action OnDying;
    public event Action<int> OnChangeHealth;
    private void Awake()
    {
        InitHealth();
    }
    /// <summary>
    /// 设定初始生命值
    /// </summary>
    protected abstract void InitHealth();
    public virtual void SetMaxHealth(int value)
    {
        if (value < 0) value = 0;
        maxHealth = value;
    }

    public virtual void AddMaxHealth(int value)
    {
        SetMaxHealth(maxHealth + value);
    }

    public virtual bool SetHealth(int value)
    {
        health = value;
        OnChangeHealth?.Invoke(value);
        if (value > maxHealth)
        {
            health = maxHealth;
        }
        else if (value < 0)
        {
            OnDying?.Invoke();
        }
        if (health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    public virtual bool AddHealth(int value)
    {
        return SetHealth(health + value);
    }

    protected virtual void Die()
    { 

    }
}
