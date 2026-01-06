using UnityEngine;
using System.Collections;

public class ImageManager : MonoBehaviour
{
    public GameObject imagePrefab; // 表示したい画像のプレハブ
    public float spawnInterval = 3.0f; // 何秒ごとに表示するか
    public Vector2 spawnRangeX = new Vector2(-5.5f, 5.5f); // 表示範囲 X
    public Vector2 spawnRangeY = new Vector2(0.7f, 6.0f); // 表示範囲 Y

    void Start()
    {
        StartCoroutine(SpawnImageRoutine());
    }

    IEnumerator SpawnImageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            // ランダムな位置を計算
            Vector3 randomPos = new Vector3(
                Random.Range(spawnRangeX.x, spawnRangeX.y),
                Random.Range(spawnRangeY.x, spawnRangeY.y),
                -0.45f); // 壁のZ座標に合わせて調整してください

            Instantiate(imagePrefab, randomPos, Quaternion.identity);
        }
    }
}