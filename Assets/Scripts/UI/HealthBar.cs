using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float iconGap;
    [SerializeField]
    private GameObject heartIcon;
    private Player PlayerInstance
    {
        get
        {
            if (Player.instance != null)
            {
                return Player.instance;
            }
            else
            {
                Debug.LogError("找不到玩家实例!");
                return null;
            }    
        }
    }
    private int heartNum = 0;
    private void Start()
    {
        PlayerInstance.GetComponent<PlayerHealthContainer>().OnChangeHealth += SetHeartNum;
        SetHeartNum(PlayerInstance.moveStateMachine.ReuseableData.PlayerDefaultHealth);
    }
    public void SetHeartNum(int value)
    {
        if (value < 0)
        {
            heartNum = 0;
        }
        else
        {
            heartNum = value;
        }
        Player.instance.moveStateMachine.ReuseableData.PlayerDefaultHealth = value;
        UpdateUI();
    }
    public void AddHeartNum(int value)
    {
        SetHeartNum(heartNum + value);
    }
    private void UpdateUI()
    {
        float xCoor = 0;
        RectTransform icon;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.childCount <= i)
            {
                Debug.LogError("可能由于删除子物体导致索引溢出，请检查");
                return;
            }
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < heartNum; i++)
        {
            if (heartIcon == null)
            {
                Debug.LogError("缺少生命值图标");
                return;
            }
            icon = Instantiate(heartIcon).GetComponent<RectTransform>();
            icon.transform.SetParent(transform, false);
            icon.anchoredPosition = new Vector2(xCoor, icon.anchoredPosition.y);
            xCoor += iconGap;
        }
    }
}
