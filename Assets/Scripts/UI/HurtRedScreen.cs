using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using UnityEngine;
public class HurtRedScreen : MonoBehaviour
{
    public static HurtRedScreen instance;
    [SerializeField]
    private UIDissolve dissolve;
    [SerializeField]
    private float showTime;
    [SerializeField]
    private float disapperTime;
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

    public void PlayEffect()
    {
        StartCoroutine(DissolveEffect());
    }
    private IEnumerator DissolveEffect()
    {
        float startTime = Time.time;
        dissolve.effectFactor = 1;
        while (dissolve.effectFactor > 0.93f)
        {
            dissolve.effectFactor = 1f - 0.07f * Mathf.Clamp01((Time.time - startTime) / showTime);
            yield return null;
        }
        startTime = Time.time;
        while (dissolve.effectFactor < 1f)
        {
            dissolve.effectFactor = 0.93f + 0.07f * Mathf.Clamp01((Time.time - startTime) / disapperTime);
            yield return null;
        }
        yield return null;
    }
}
