using UnityEngine;

public class TrainSpline : MonoBehaviour
{
    public Transform[] points;
    public float speed = 10f;
    public float lookAhead = 0.02f;   // 進行方向を見るため
    public float turnSpeed = 5f;

    private float t = 0f;

    void Update()
    {
        if (points.Length < 4) return;

        int count = points.Length;

        // t をループさせる（値は 0〜count の間で動く）
        t += Time.deltaTime * speed * 0.1f;

        if (t >= count) t -= count;

        // 現在区間の index
        int i0 = Mathf.FloorToInt(t);
        float u = t - i0; // 区間内の 0〜1

        // Catmull-Rom 用のインデックス
        int i1 = (i0 + 1) % count;
        int i2 = (i0 + 2) % count;
        int i3 = (i0 + 3) % count;

        // 座標取得
        Vector3 p0 = points[i0].position;
        Vector3 p1 = points[i1].position;
        Vector3 p2 = points[i2].position;
        Vector3 p3 = points[i3].position;

        // 位置
        Vector3 pos = CatmullRom(p0, p1, p2, p3, u);
        transform.position = pos;

        // 向く方向（少し先の位置を見る）
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

    // Catmull-Rom 算出
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f *
            ((2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
    }
}
