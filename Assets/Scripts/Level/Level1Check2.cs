using UnityEngine;

public class LevelCheck2 : MonoBehaviour
{
    public GameObject CEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CEffect.SetActive(false);
        }
    }
}
