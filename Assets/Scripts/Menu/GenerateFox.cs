using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 在指定区域内随机位置生成预制件prefab
 */
public class GenerateFox : MonoBehaviour
{

    public GameObject prefab;
    BoxCollider2D spawnArea; // 指定生成区域的 BoxCollider2D

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
        // 获取BoxCollider2D的范围
        Bounds bounds = spawnArea.bounds;

        // 在范围内生成随机位置
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        
        // 在随机位置生成预制件
        Instantiate(prefab, randomPosition, Quaternion.identity);
    }
}
