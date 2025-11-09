using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 currenTagetDri = Vector3.zero;
    private Vector3 rotationPassedTime = Vector3.zero;
    private float rotationTime = 0.2f;
    private float rotationVelocity = 0f;    

    private void FixedUpdate()
    {
        EnemyRotate();
    }
    //µÐÈËÐý×ª
    public void EnemyRotate()
    {
        UpdateTargetRotateData(GetRotateAngle());
        RotateToTagetDri();
    }
    public float GetRotateAngle()
    {
        Vector2 TargetDirection = new Vector2(transform.position.x
        - Player.instance.transform.position.x, transform.position.z
        - Player.instance.transform.position.z).normalized;

        float DirectionAngle = Mathf.Atan2(TargetDirection.x, TargetDirection.y) * Mathf.Rad2Deg;
        return DirectionAngle;
    }
    public void UpdateTargetRotateData(float TargetAngle)
    {
        currenTagetDri.y = TargetAngle;
        //rotationPassedTime.y = 0f;
    }
    public void RotateToTagetDri()
    {
        float CurrentAngleY = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles.y;
        if (CurrentAngleY == currenTagetDri.y)
        {
            rotationPassedTime.y = 0f;
            return;
        }
        float SmoothingAngleY = Mathf.SmoothDampAngle(CurrentAngleY,
        currenTagetDri.y,
        ref rotationVelocity,
        rotationTime - rotationPassedTime.y);
        rotationPassedTime.y += Time.deltaTime;

        Quaternion TagetRotation = Quaternion.Euler(0f, SmoothingAngleY, 0f);
        gameObject.GetComponent<Rigidbody>().MoveRotation(TagetRotation);
    }
}
