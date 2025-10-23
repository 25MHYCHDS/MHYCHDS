using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]

///<summary>
///‘§ ‰»Îπ‹¿Ì∆˜
/// </summary>
public enum InputType
{
    Attack,
    Jump
}

public class InputBuffer :MonoBehaviour
{
    public static InputBuffer instance;
    public float BufferedTime = 0.12f;

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
        IniteInputBuffer();
    }

    private void Update()
    {
        UpdateBbuffer();
    }

    public Dictionary<InputType, BuffSlot> OnceBuffDict = new();

    public void IniteInputBuffer()
    {
        OnceBuffDict.Clear();
        OnceBuffDict.Add(InputType.Jump, new BuffSlot() { BufferTime = BufferedTime });
        OnceBuffDict.Add(InputType.Attack, new BuffSlot() { BufferTime = BufferedTime });
    }

    public void UpdateBbuffer()
    {
        if(OnceBuffDict.Count <= 0 || OnceBuffDict == null || OnceBuffDict.Values == null)
        {
            foreach(var slot in OnceBuffDict.Values)
            {
                if (!slot.Buffered) { continue; }

                slot.Timer -= Time.time;
                if(slot.Timer <= 0)
                {
                    slot.Buffered = false;
                }
            }
        }
    }

    public void AddInputBuffer(InputType inputType)
    {
        if (!OnceBuffDict.ContainsKey(inputType))
        {
            OnceBuffDict.Add(inputType, new BuffSlot());
        }
        if (OnceBuffDict.TryGetValue(inputType, out BuffSlot slot))
        { 
            slot.Buffered = true;
            slot.Timer = slot.BufferTime;
        }
    }

    public bool ConsumInputBuffer(InputType inputType, UnityAction callback = null )
    {
        if(!OnceBuffDict.ContainsKey(inputType)) {return false;}
        if(OnceBuffDict.TryGetValue(inputType,out var slot) && slot.Buffered)
        {
            slot.Buffered = false;
            callback?.Invoke();
            return true;
        }
        return false;
    }

    public class BuffSlot
    {
        [SerializeField] public float BufferTime = 0.2f;
        private float timer = 0f;
        public bool Buffered = false;
        public float Timer = 0f;
    }

    public void InterruptInputBuffed(InputType inputType)
    {
        if (OnceBuffDict.TryGetValue(inputType, out var slot)) 
        {
            slot.Buffered = false;
            slot.Timer = 0f;
        }
    }
}
