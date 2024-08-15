using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * ��ָ�����������λ������Ԥ�Ƽ�prefab
 */
public class GenerateFox : MonoBehaviour
{

    public GameObject prefab;
    BoxCollider2D spawnArea; // ָ����������� BoxCollider2D

    // Start is called before the first frame update
    void Start()
    {
        spawnArea=GetComponent<BoxCollider2D>();
        InvokeRepeating("SpawnPrefab", 0, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        // ��ȡBoxCollider2D�ķ�Χ
        Bounds bounds = spawnArea.bounds;

        // �ڷ�Χ���������λ��
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        
        // �����λ������Ԥ�Ƽ�
        Instantiate(prefab, randomPosition, Quaternion.identity);
    }
}
