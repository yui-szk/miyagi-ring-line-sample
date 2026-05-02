using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Camera Transform")]
    public Vector3 startPosition;
    public Vector3 startRotation;
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    [Header("Move Settings")]
    public float moveDuration = 2.0f;

    [Header("Start Delay")]
    public float startDelay = 1.5f;   // ※今は未使用（消してもOK）

    void Start()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);

        StartCoroutine(CameraSequence());
    }

    IEnumerator CameraSequence()
    {
        // ① カメラ移動
        yield return StartCoroutine(MoveCamera());

        // ② 到着した瞬間にゲーム開始（ここが本質）
        if (GameManager2.Instance != null)
        {
            GameManager2.Instance.StartGame();
        }
    }

    IEnumerator MoveCamera()
    {
        float t = 0f;

        Vector3 fromPos = startPosition;
        Quaternion fromRot = Quaternion.Euler(startRotation);

        Vector3 toPos = targetPosition;
        Quaternion toRot = Quaternion.Euler(targetRotation);

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float rate = t / moveDuration;

            transform.position = Vector3.Lerp(fromPos, toPos, rate);
            transform.rotation = Quaternion.Lerp(fromRot, toRot, rate);

            yield return null;
        }

        transform.position = toPos;
        transform.rotation = toRot;
    }
}