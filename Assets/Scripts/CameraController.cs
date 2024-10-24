using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform focus;
    [Tooltip("平滑速度")]
    [Range(0f, 10f)]
    public float smoothTime = 5f; //平滑速度

    [Header("相机随运动缩放")]
    [Tooltip("相机缩放最小值")]
    public float minSize = 5f;   // 相机最小大小
    [Tooltip("相机缩放最大值")]
    public float maxSize = 13f;  // 相机最大大小
    [Tooltip("距离因子，改变这个可以改变缩放速度")]
    [Range(2,5)]
    public float distanceFactor = 5f; // 距离因子

    Vector3 offset = new Vector3(0, 0, 10);
    Camera camera;

    void Awake()
    {
        gameObject.transform.position = focus.position - offset;
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        CameraSmoothFollower();
        CameraSizeAdjuster();
    }

    //相机平滑跟随
    void CameraSmoothFollower()
    {
        transform.position = Vector3.Lerp(transform.position, focus.position - offset, Time.deltaTime * smoothTime);
    }

    //相机随运动动态缩放
    void CameraSizeAdjuster()
    {
        float distance = Vector3.Distance(focus.position,transform.position);
        float targetSize = Mathf.Clamp(minSize + (distance - offset.magnitude) * distanceFactor, minSize, maxSize);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, Time.deltaTime * smoothTime);
    }
}
