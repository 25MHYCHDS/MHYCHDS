using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    public Transform TargetTransForm;
    public Vector3 Offset;
    private void Start()
    {

    }
    private void Update()
    {
        //transform.position = Camera.main.WorldToScreenPoint(TargetTransForm.position) + Offset;

        gameObject.GetComponent<Slider>().value = Stamina.instance.stamina / 10;
    }
}