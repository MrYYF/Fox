using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 实现狐狸乱飞效果
 */
public class FlyFox : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;

    void Start()
    {
        Invoke("DestroyObject", 4f);

        float x = Random.Range(-15f, 15f);
        float y = Random.Range(30f, 50f);
        float force =Random.Range(10f, 20f);
        rigidbody2D.AddForce(new Vector2 (x,y)*force);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
