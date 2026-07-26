using UnityEngine;
using System.Collections;

public class SakuraSpawner : MonoBehaviour
{
    [Header("Normal Petal")]
    public GameObject petalPrefab;

    [Header("Special Petal")]
    public GameObject specialPetalPrefab;
    public float specialInterval = 5f;

    [Header("Spawn Area")]
    public Transform vessel;
    public float vesselRangeX = 1f;
    public float vesselRangeZ = 1f;
    public float minSpawnHeight = 5f;
    public float maxSpawnHeight = 8f;

    [Header("Normal Spawn")]
    public int normalPerFrame = 1;

    [Header("Bonus Rush")]
    public int bonusPerFrame = 5;
    public float bonusInterval = 5f;
    public float bonusDuration = 1f;

    bool isSpawning;

    Coroutine normalCoroutine;
    Coroutine bonusCoroutine;
    Coroutine specialCoroutine;

    public void StartSpawn()
    {
        if (isSpawning) return;

        isSpawning = true;

        normalCoroutine = StartCoroutine(NormalLoop());
        bonusCoroutine = StartCoroutine(BonusLoop());
        specialCoroutine = StartCoroutine(SpecialLoop());
    }

    public void StopSpawn()
    {
        isSpawning = false;

        if (normalCoroutine != null)
            StopCoroutine(normalCoroutine);

        if (bonusCoroutine != null)
            StopCoroutine(bonusCoroutine);

        if (specialCoroutine != null)
            StopCoroutine(specialCoroutine);
    }

    IEnumerator NormalLoop()
    {
        while (isSpawning)
        {
            for (int i = 0; i < normalPerFrame; i++)
                SpawnPetal();

            yield return null;
        }
    }

    IEnumerator BonusLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(bonusInterval);

            float end = Time.time + bonusDuration;

            while (Time.time < end)
            {
                for (int i = 0; i < bonusPerFrame; i++)
                    SpawnPetal();

                yield return null;
            }
        }
    }

    IEnumerator SpecialLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(specialInterval);
            SpawnSpecialPetal();
        }
    }

    void SpawnPetal()
    {
        float h = Random.Range(minSpawnHeight, maxSpawnHeight);

        Vector3 pos = vessel.position + new Vector3(
            Random.Range(-vesselRangeX, vesselRangeX),
            h,
            Random.Range(-vesselRangeZ, vesselRangeZ)
        );

        GameObject petal = Instantiate(petalPrefab, pos, Random.rotation);

        Rigidbody rb = petal.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;

        rb.AddForce(new Vector3(
            Random.Range(-0.3f, 0.3f),
            0,
            Random.Range(-0.3f, 0.3f)
        ), ForceMode.VelocityChange);
    }

    void SpawnSpecialPetal()
    {
        float h = Random.Range(minSpawnHeight, maxSpawnHeight);

        Vector3 pos = vessel.position + new Vector3(
            Random.Range(-vesselRangeX, vesselRangeX),
            h,
            Random.Range(-vesselRangeZ, vesselRangeZ)
        );

        GameObject petal = Instantiate(specialPetalPrefab, pos, Random.rotation);

        Rigidbody rb = petal.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;

        rb.AddForce(new Vector3(
            Random.Range(-0.3f, 0.3f),
            0,
            Random.Range(-0.3f, 0.3f)
        ), ForceMode.VelocityChange);
    }
}