using Unity.Cinemachine;
using UnityEditorInternal;
using UnityEngine;

public class GroundFall : MonoBehaviour
{
    public static GroundFall instance;

    public float Speed = 20;
    public float acceleration = 3f;
    public float MaxSpeed;
    public float RotateSpeed = 20;

    private float rotationVelocity;
    private float rotationPassedTime;
    private PlayerMoveStateMachine stateMachine;
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

    private void Start()
    {
        stateMachine = Player.instance.moveStateMachine;
    }

    void Update()
    {
        Vector3 targetP = transform.position;
        targetP.z -= Time.deltaTime * Speed;
        transform.position = targetP;

        RotateToTagetDri();

        if (Speed < MaxSpeed)
        {
            Speed += Time.deltaTime * acceleration;
        }
    }

    //平滑旋转到Data中的目标角度
    public void RotateToTagetDri()
    {
        float currenTagetAngle = stateMachine.ReuseableData.currenGroundTagetRotateAngle + transform.rotation.z;

        if (transform.rotation.z == currenTagetAngle)
        {
            return;
        }

        float SmoothingAngle = Mathf.SmoothDampAngle(transform.rotation.z, currenTagetAngle,
            ref rotationVelocity, stateMachine.ReuseableData.rotationTime.y -
            rotationPassedTime);

        rotationPassedTime += Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, SmoothingAngle);
        RenderSettings.skybox.SetFloat("_Rotation", SmoothingAngle);
    }
}
