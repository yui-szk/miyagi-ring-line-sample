using UnityEngine;

public class TrainSpline : MonoBehaviour
{
    public Transform[] points;
    public float speed = 10f;
    public float lookAhead = 0.02f;
    public float turnSpeed = 5f;

    public float stopAfterSeconds = 10f;
    public float slowDownDuration = 3f;

    float t = 0f;
    float elapsedTime = 0f;
    float currentSpeed;
    bool isStopped = false;

    void Start()
    {
        currentSpeed = speed;

        if (points == null || points.Length < 4)
        {
            Debug.LogError("TrainSpline: points が未設定 or 足りません", this);
            enabled = false;
        }
    }

    void Update()
    {
        if (isStopped) return;

        elapsedTime += Time.deltaTime;

        // 減速 → 停止
        if (elapsedTime >= stopAfterSeconds)
        {
            float t01 = (elapsedTime - stopAfterSeconds) / slowDownDuration;
            currentSpeed = Mathf.Lerp(speed, 0f, t01);

            if (t01 >= 1f)
            {
                currentSpeed = 0f;
                isStopped = true;
            }
        }

        int count = points.Length;

        t += Time.deltaTime * currentSpeed * 0.1f;
        if (t >= count) t -= count;

        int i0 = Mathf.FloorToInt(t);
        float u = t - i0;

        Vector3 p0 = points[i0].position;
        Vector3 p1 = points[(i0 + 1) % count].position;
        Vector3 p2 = points[(i0 + 2) % count].position;
        Vector3 p3 = points[(i0 + 3) % count].position;

        Vector3 pos = CatmullRom(p0, p1, p2, p3, u);
        transform.position = pos;

        // 向き
        float tAhead = t + lookAhead;
        int ai0 = Mathf.FloorToInt(tAhead) % count;
        float au = tAhead - Mathf.Floor(tAhead);

        Vector3 future = CatmullRom(
            points[ai0].position,
            points[(ai0 + 1) % count].position,
            points[(ai0 + 2) % count].position,
            points[(ai0 + 3) % count].position,
            au
        );

        Vector3 dir = future - pos;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f *
            ((2f * p1) +
             (-p0 + p2) * t +
             (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
             (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
    }
}
