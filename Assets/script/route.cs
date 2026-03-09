using UnityEngine;

public class TrainSpline : MonoBehaviour
{
    public Transform[] points;
    public float speed = 10f;
    public float lookAhead = 0.02f;
    public float turnSpeed = 5f;

    // 減速の強さ（小さいほどゆっくり止まる）
    public float deceleration = 1.2f;

    bool isStopping = false;

    private float t = 0f;

    void Update()
    {
        if (points.Length < 4) return;

        int count = points.Length;

        // 減速処理
        if (isStopping)
        {
            speed = Mathf.MoveTowards(speed, 0f, deceleration * Time.deltaTime);

            // 最後ピタッと止める
            if (speed < 0.05f)
            {
                speed = 0f;
            }
        }

        // スプライン移動
        t += Time.deltaTime * speed * 0.1f;

        if (t >= count) t -= count;

        int i0 = Mathf.FloorToInt(t);
        float u = t - i0;

        int i1 = (i0 + 1) % count;
        int i2 = (i0 + 2) % count;
        int i3 = (i0 + 3) % count;

        Vector3 p0 = points[i0].position;
        Vector3 p1 = points[i1].position;
        Vector3 p2 = points[i2].position;
        Vector3 p3 = points[i3].position;

        Vector3 pos = CatmullRom(p0, p1, p2, p3, u);
        transform.position = pos;

        // 向き
        float tAhead = t + lookAhead;

        int ai0 = Mathf.FloorToInt(tAhead) % count;
        float au = tAhead - Mathf.Floor(tAhead);

        int ai1 = (ai0 + 1) % count;
        int ai2 = (ai0 + 2) % count;
        int ai3 = (ai0 + 3) % count;

        Vector3 f0 = points[ai0].position;
        Vector3 f1 = points[ai1].position;
        Vector3 f2 = points[ai2].position;
        Vector3 f3 = points[ai3].position;

        Vector3 future = CatmullRom(f0, f1, f2, f3, au);
        Vector3 dir = future - pos;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    // 減速開始
    public void StopTrain()
    {
        isStopping = true;
    }

    // Catmull-Rom
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f *
            ((2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
    }
}