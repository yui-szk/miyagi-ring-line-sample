using UnityEngine;

public class joycon_move : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 5f;

    [Header("Move Limit")]
    public float minX = -3f;
    public float maxX = 3f;

    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance == null)
        {
            Debug.LogError("JoyconManager がシーンにありません");
            return;
        }

        if (JoyconManager.Instance.j.Count > 0)
        {
            joycon = JoyconManager.Instance.j[0];
        }
        else
        {
            Debug.LogError("Joy-Con が接続されていません");
        }
    }

    void Update()
    {
        if (joycon == null) return;

        Vector3 accel = joycon.GetAccel();

        // 🔁 縦持ち：上下傾き（反転）
        float moveX = -accel.y * moveSpeed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += moveX;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
