using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("场景设置")]
    public string targetSceneName = "ui"; // 要跳转的场景名称

    [Header("触发设置")]
    public string playerTag = "Player"; // 玩家标签

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是玩家
        if (other.CompareTag(playerTag))
        {
            // 跳转到目标场景
            SceneManager.LoadScene(targetSceneName);
        }
    }
}