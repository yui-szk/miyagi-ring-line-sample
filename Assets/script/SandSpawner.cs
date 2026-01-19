using UnityEngine;
using System.Collections;

public class SandSpawner : MonoBehaviour
{
    [Header("砂Prefab")]
    public GameObject sandPrefab;

    [Header("生成位置")]
    public Transform vessel;       // 通常砂の基準
    public Transform bonusZone;    // ボーナス砂の基準
    public float spawnHeight = 6f;

    [Header("通常砂設定")]
    public float normalSpeed = 6f;
    public int normalPerFrame = 1;
    public float normalRangeX = 3f;
    public float normalRangeZ = 1f;

    [Header("ボーナス砂設定")]
    public float bonusSpeed = 10f;
    public int bonusPerFrame = 2;
    public float bonusRangeX = 2f;   // ★ 横幅調整
    public float bonusRangeZ = 1f;   // ★ 奥行き調整
    public float bonusInterval = 5f;
    public float bonusDuration = 0.5f;

    private bool isSpawning = false;
    private Coroutine normalCoroutine;
    private Coroutine bonusCoroutine;

    void Start()
    {
        isSpawning = false; // 最初は停止
    }

    public void StartSpawn()
    {
        if (isSpawning) return;

        isSpawning = true;
        normalCoroutine = StartCoroutine(NormalSandLoop());
        bonusCoroutine = StartCoroutine(BonusSandLoop());
    }

    public void StopSpawn()
    {
        if (!isSpawning) return;

        isSpawning = false;
        if (normalCoroutine != null) StopCoroutine(normalCoroutine);
        if (bonusCoroutine != null) StopCoroutine(bonusCoroutine);
    }

    // =============================
    IEnumerator NormalSandLoop()
    {
        while (isSpawning)
        {
            for (int i = 0; i < normalPerFrame; i++)
            {
                SpawnSand(vessel, normalRangeX, normalRangeZ, spawnHeight, normalSpeed);
            }
            yield return null;
        }
    }

    IEnumerator BonusSandLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(bonusInterval);

            float endTime = Time.time + bonusDuration;
            while (Time.time < endTime)
            {
                for (int i = 0; i < bonusPerFrame; i++)
                {
                    SpawnSand(bonusZone, bonusRangeX, bonusRangeZ, spawnHeight, bonusSpeed);
                }
                yield return null;
            }
        }
    }

    // =============================
    void SpawnSand(Transform basePoint, float rangeX, float rangeZ, float height, float forwardSpeed)
    {
        if (sandPrefab == null || basePoint == null) return;

        Vector3 spawnPos = new Vector3(
            basePoint.position.x + Random.Range(-rangeX, rangeX),
            basePoint.position.y + height,
            basePoint.position.z + Random.Range(-rangeZ, rangeZ)
        );

        GameObject sand = Instantiate(sandPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = sand.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.back * forwardSpeed;
            rb.linearVelocity += new Vector3(Random.Range(-0.2f, 0.2f), 0, 0); // 少し横揺れ
        }
    }
}
