using Unity.Mathematics;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float stamina = 10f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        stamina = Mathf.Clamp(stamina, 0f, 10f);

        stamina += Time.deltaTime;
    }

    public bool CheckCanAction(float Consum)
    {
        if(stamina - Consum > 0)
        {
            stamina -= Consum;
            return true;
        }
        else
        {
            return false;
        }

    }
}
