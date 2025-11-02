using System.Collections;
using UnityEngine;

public class Level1Check : MonoBehaviour
{
    public GameObject CEffect;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CEffect.SetActive(true);
            StartCoroutine("DisE");
        }
    }

    IEnumerator DisE()
    {
        yield return new WaitForSeconds(0.3f);
        CEffect.SetActive(false);
    }
}
