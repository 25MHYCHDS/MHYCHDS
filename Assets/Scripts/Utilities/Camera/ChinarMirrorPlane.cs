using UnityEngine;
 
/// <summary>
/// Plane管理脚本 —— 挂载新建的Camera上
/// </summary>
[ExecuteInEditMode] //编辑模式中执行
public class ChinarMirrorPlane : MonoBehaviour
{
    public  GameObject mirrorPlane; //镜子Plane
    public  bool       estimateViewFrustum    = true;
    public  bool       setNearClipPlane       = true;   //是否设置近剪切平面
    public  float      nearClipDistanceOffset = -0.01f; //近剪切平面的距离
    private Camera     mirrorCamera;                    //镜像摄像机
    private Vector3    ViewNormal;                      //屏幕的法线
    private float      CameraToPlanLeftEdgeDistance;    //到屏幕左边缘的距离
    private float      CameraToPlanRightEdgeDistance;   //到屏幕右边缘的距离
    private float      CameraToPlanUpEdgeDistance;      //到屏幕下边缘的距离
    private float      CameraToPlanDownEdgeDistance;    //到屏幕上边缘的距离
    private float      MirrorCameraToPlanDistance;      //从镜像摄像机到屏幕的距离
    private float      MirrorCmeraCloseFace;            //镜像摄像机的近剪切面的距离
    private float      MirrorCmeraFarFace;              //镜像摄像机的远剪切面的距离
    private Vector3    PlanLeftDownPostion;             //世界坐标系的左下角
    private Vector3    PlanRightDownPostion;            //世界坐标系的右下角
    private Vector3    PlanLeftUpPostion;               //世界坐标系的左上角
    private Vector3    MirrorCmaeraPostion;             //镜像观察角度的世界坐标位置
    private Vector3    MirrorCameraToPlanLeftDPoint;    //从镜像摄像机到左下角
    private Vector3    MirrorCameraToPlanRightUPoint;   //从镜像摄像机到右下角
    private Vector3    MirrorCameraToPlanLeftUPoint;    //从镜像摄像机到左上角
    private Vector3    ViewRightRightRotationVector;    //屏幕的右侧旋转轴
    private Vector3    ViewLeftDownRotationVector;      //屏幕的上侧旋转轴
    private Matrix4x4  p  = new Matrix4x4();
    private Matrix4x4  rm = new Matrix4x4();
    private Matrix4x4  tm = new Matrix4x4();
    private Quaternion CamraQuaternion  = new Quaternion();
 
 
    private void Start()
    {
        mirrorCamera = GetComponent<Camera>();
    }
 
 
    private void Update()
    {
        if (null == mirrorPlane || null == mirrorCamera) return;

        //世界坐标系的左下角
        PlanLeftDownPostion = mirrorPlane.transform.TransformPoint(new Vector3(-5.0f, 0.0f, -5.0f));
        //世界坐标系的右下角
        PlanRightDownPostion = mirrorPlane.transform.TransformPoint(new Vector3(5.0f,  0.0f, -5.0f));
        //世界坐标系的左上角
        PlanLeftUpPostion = mirrorPlane.transform.TransformPoint(new Vector3(-5.0f, 0.0f, 5.0f));
        //镜像观察角度的世界坐标位置
        MirrorCmaeraPostion = transform.position;
        //镜像摄像机的近剪切面的距离
        MirrorCmeraCloseFace = mirrorCamera.nearClipPlane;
        //镜像摄像机的远剪切面的距离
        MirrorCmeraFarFace = mirrorCamera.farClipPlane;
        //从镜像摄像机到左下角
        MirrorCameraToPlanLeftDPoint = PlanLeftDownPostion - MirrorCmaeraPostion;
        //从镜像摄像机到右下角
        MirrorCameraToPlanRightUPoint = PlanRightDownPostion - MirrorCmaeraPostion;
        //从镜像摄像机到左上角
        MirrorCameraToPlanLeftUPoint = PlanLeftUpPostion - MirrorCmaeraPostion;
        //屏幕的右侧旋转向量
        ViewRightRightRotationVector = PlanRightDownPostion - PlanLeftDownPostion;
        //屏幕的上侧旋转向量
        ViewLeftDownRotationVector = PlanLeftUpPostion - PlanLeftDownPostion;                            

        //如果看向镜子的背面

        if (Vector3.Dot(-Vector3.Cross(MirrorCameraToPlanLeftDPoint, MirrorCameraToPlanLeftUPoint), MirrorCameraToPlanRightUPoint) < 0.0f)
        {
            ViewLeftDownRotationVector = -ViewLeftDownRotationVector;
            PlanLeftDownPostion = PlanLeftUpPostion;
            PlanRightDownPostion = PlanLeftDownPostion + ViewRightRightRotationVector;
            PlanLeftUpPostion = PlanLeftDownPostion + ViewLeftDownRotationVector;
            MirrorCameraToPlanLeftDPoint = PlanLeftDownPostion - MirrorCmaeraPostion;
            MirrorCameraToPlanRightUPoint = PlanRightDownPostion - MirrorCmaeraPostion;
            MirrorCameraToPlanLeftUPoint = PlanLeftUpPostion - MirrorCmaeraPostion;
        }

        ViewRightRightRotationVector.Normalize();
        ViewLeftDownRotationVector.Normalize();

        //两个向量的叉乘，最后在取负，因为Unity是使用左手坐标系

        ViewNormal = -Vector3.Cross(ViewRightRightRotationVector, ViewLeftDownRotationVector); 
        ViewNormal.Normalize();

        MirrorCameraToPlanDistance = -Vector3.Dot(MirrorCameraToPlanLeftDPoint, ViewNormal);

        if (setNearClipPlane)
        {
            MirrorCmeraCloseFace = MirrorCameraToPlanDistance + nearClipDistanceOffset;

            mirrorCamera.nearClipPlane = MirrorCmeraCloseFace;
        }
        

        CameraToPlanLeftEdgeDistance = Vector3.Dot(ViewRightRightRotationVector, MirrorCameraToPlanLeftDPoint) * MirrorCmeraCloseFace / MirrorCameraToPlanDistance;

        CameraToPlanRightEdgeDistance = Vector3.Dot(ViewRightRightRotationVector, MirrorCameraToPlanRightUPoint) * MirrorCmeraCloseFace / MirrorCameraToPlanDistance;

        CameraToPlanUpEdgeDistance = Vector3.Dot(ViewLeftDownRotationVector, MirrorCameraToPlanLeftDPoint) * MirrorCmeraCloseFace / MirrorCameraToPlanDistance;

        CameraToPlanDownEdgeDistance = Vector3.Dot(ViewLeftDownRotationVector, MirrorCameraToPlanLeftUPoint) * MirrorCmeraCloseFace / MirrorCameraToPlanDistance;
 
 
        //投影矩阵
        p[0, 0] = 2.0f * MirrorCmeraCloseFace / (CameraToPlanRightEdgeDistance - CameraToPlanLeftEdgeDistance);
        p[0, 1] = 0.0f;
        p[0, 2] = (CameraToPlanRightEdgeDistance + CameraToPlanLeftEdgeDistance) / (CameraToPlanRightEdgeDistance - CameraToPlanLeftEdgeDistance);
        p[0, 3] = 0.0f;
 
        p[1, 0] = 0.0f;
        p[1, 1] = 2.0f * MirrorCmeraCloseFace / (CameraToPlanDownEdgeDistance - CameraToPlanUpEdgeDistance);
        p[1, 2] = (CameraToPlanDownEdgeDistance + CameraToPlanUpEdgeDistance) / (CameraToPlanDownEdgeDistance - CameraToPlanUpEdgeDistance);
        p[1, 3] = 0.0f;
 
        p[2, 0] = 0.0f;
        p[2, 1] = 0.0f;
        p[2, 2] = (MirrorCmeraFarFace + MirrorCmeraCloseFace) / (MirrorCmeraCloseFace - MirrorCmeraFarFace);
        p[2, 3] = 2.0f * MirrorCmeraFarFace * MirrorCmeraCloseFace / (MirrorCmeraCloseFace - MirrorCmeraFarFace);
 
        p[3, 0] = 0.0f;
        p[3, 1] = 0.0f;
        p[3, 2] = -1.0f;
        p[3, 3] = 0.0f;
 
        //旋转矩阵
        rm[0, 0] = ViewRightRightRotationVector.x;
        rm[0, 1] = ViewRightRightRotationVector.y;
        rm[0, 2] = ViewRightRightRotationVector.z;
        rm[0, 3] = 0.0f;
 
        rm[1, 0] = ViewLeftDownRotationVector.x;
        rm[1, 1] = ViewLeftDownRotationVector.y;
        rm[1, 2] = ViewLeftDownRotationVector.z;
        rm[1, 3] = 0.0f;
 
        rm[2, 0] = ViewNormal.x;
        rm[2, 1] = ViewNormal.y;
        rm[2, 2] = ViewNormal.z;
        rm[2, 3] = 0.0f;
 
        rm[3, 0] = 0.0f;
        rm[3, 1] = 0.0f;
        rm[3, 2] = 0.0f;
        rm[3, 3] = 1.0f;
 
        tm[0, 0] = 1.0f;
        tm[0, 1] = 0.0f;
        tm[0, 2] = 0.0f;
        tm[0, 3] = -MirrorCmaeraPostion.x;
 
        tm[1, 0] = 0.0f;
        tm[1, 1] = 1.0f;
        tm[1, 2] = 0.0f;
        tm[1, 3] = -MirrorCmaeraPostion.y;
 
        tm[2, 0] = 0.0f;
        tm[2, 1] = 0.0f;
        tm[2, 2] = 1.0f;
        tm[2, 3] = -MirrorCmaeraPostion.z;
 
        tm[3, 0] = 0.0f;
        tm[3, 1] = 0.0f;
        tm[3, 2] = 0.0f;
        tm[3, 3] = 1.0f;
 
 
        mirrorCamera.projectionMatrix    = p; //矩阵组
        mirrorCamera.worldToCameraMatrix = rm * tm;

        if (!estimateViewFrustum) return;

        CamraQuaternion.SetLookRotation((0.5f * (PlanRightDownPostion + PlanLeftUpPostion) - MirrorCmaeraPostion), ViewLeftDownRotationVector); //旋转摄像机
        mirrorCamera.transform.rotation = CamraQuaternion;            //聚焦到屏幕的中心点
 
        //估值 —— 三目简写
        mirrorCamera.fieldOfView = mirrorCamera.aspect >= 1.0 ? Mathf.Rad2Deg * Mathf.Atan(((PlanRightDownPostion - PlanLeftDownPostion).magnitude + (PlanLeftUpPostion - PlanLeftDownPostion).magnitude) / MirrorCameraToPlanLeftDPoint.magnitude) : Mathf.Rad2Deg / mirrorCamera.aspect * Mathf.Atan(((PlanRightDownPostion - PlanLeftDownPostion).magnitude + (PlanLeftUpPostion - PlanLeftDownPostion).magnitude) / MirrorCameraToPlanLeftDPoint.magnitude);
        //在摄像机角度考虑，保证视锥足够宽
    }
}