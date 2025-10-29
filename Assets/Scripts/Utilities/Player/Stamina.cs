using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float stamina = 10f;
    public static Stamina instance;

    public bool RecoverStamina = true;
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

    // Update is called once per frame
    void Update()
    {
        stamina = Mathf.Clamp(stamina, 0f, 10f);

        if (RecoverStamina)
        {
            stamina += Time.deltaTime;
        }
    }

    public bool CheckCanAction(float Consum)
    {
        if(stamina - Consum > 0)
        {
            stamina -= Consum;

            StartCoroutine("CanRecover");
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator CanRecover()
    {
        yield return new WaitForSeconds(1f);
        RecoverStamina = true;
    }
}
