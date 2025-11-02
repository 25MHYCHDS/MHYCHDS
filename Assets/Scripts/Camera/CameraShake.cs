using Unity.Cinemachine;//虚拟相机命名空间
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private CinemachineBasicMultiChannelPerlin perlin;//相机组件
    public Coroutine coroutine;//协程

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        perlin = gameObject.GetComponent<CinemachineBasicMultiChannelPerlin>();//组件获取方便赋值
    }
    //协程函数间隔调用
    IEnumerator Timer(float timer)
    {
        yield return new WaitForSeconds(timer);//等待timer后调用
        perlin.AmplitudeGain = 0;//振幅
        perlin.FrequencyGain = 0;//频率
    }
    public void StartShake()
    {
        perlin.AmplitudeGain = 1;//设置属性
        perlin.FrequencyGain = 3;
        coroutine = StartCoroutine(Timer(0.1f));//开始协程
    }
}
