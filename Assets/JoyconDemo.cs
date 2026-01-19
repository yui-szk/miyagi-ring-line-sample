using System.Collections.Generic;
using UnityEngine;

public class JoyconOrKeyboardController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxRange = 3f;
    public float joyconTiltSensitivity = 0.8f;

    private List<Joycon> joycons;
    private Joycon joycon;

    private Quaternion initialRotation; // 初期向きを保存

    void Start()
    {
        // 器の向きを保存（カメラに正しく向いた状態）
        initialRotation = transform.localRotation;

        joycons = JoyconManager.Instance.j;
        if (joycons != null && joycons.Count > 0)
        {
            joycon = joycons[0];
        }
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;

        if (joycon != null && JoyconManager.Instance.j.Count > 0)
        {
            // Joycon 操作
            Vector3 gyro = joycon.GetAccel();
            float moveX = gyro.x * joyconTiltSensitivity;
            float moveZ = gyro.y * joyconTiltSensitivity;

            pos += new Vector3(moveX, 0, -moveZ) * moveSpeed * Time.deltaTime;
        }
        else
        {
            // キーボード操作 ←→↑↓
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            pos += new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        }

        // 範囲制限
        pos.x = Mathf.Clamp(pos.x, -maxRange, maxRange);
        pos.z = Mathf.Clamp(pos.z, -maxRange, maxRange);
        pos.y = 0;

        transform.localPosition = pos;

        // ★ ここがポイント：初期向きに固定 ★
        transform.localRotation = initialRotation;
    }
}
