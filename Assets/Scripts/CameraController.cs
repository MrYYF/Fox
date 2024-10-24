using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform focus;
    [Tooltip("ƽ���ٶ�")]
    [Range(0f, 10f)]
    public float smoothTime = 5f; //ƽ���ٶ�

    [Header("������˶�����")]
    [Tooltip("���������Сֵ")]
    public float minSize = 5f;   // �����С��С
    [Tooltip("����������ֵ")]
    public float maxSize = 13f;  // �������С
    [Tooltip("�������ӣ��ı�������Ըı������ٶ�")]
    [Range(2,5)]
    public float distanceFactor = 5f; // ��������

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

    //���ƽ������
    void CameraSmoothFollower()
    {
        transform.position = Vector3.Lerp(transform.position, focus.position - offset, Time.deltaTime * smoothTime);
    }

    //������˶���̬����
    void CameraSizeAdjuster()
    {
        float distance = Vector3.Distance(focus.position,transform.position);
        float targetSize = Mathf.Clamp(minSize + (distance - offset.magnitude) * distanceFactor, minSize, maxSize);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetSize, Time.deltaTime * smoothTime);
    }
}
