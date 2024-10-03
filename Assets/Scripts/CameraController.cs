using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform focus;
    public float smoothTime = 5f;
    Vector3 offset = new Vector3(0, 0, 10);

    void Awake()
    {
        gameObject.transform.position = focus.position - offset;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, focus.position - offset, Time.deltaTime*smoothTime);
    }
}
