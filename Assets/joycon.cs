using UnityEngine;
using System.Collections.Generic;

public class JoyconFirstPersonWalk : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 120f;
    public Transform cameraTransform;

    float xRotation = 0f;
    Joycon joycon;

    void Start()
    {
        List<Joycon> joycons = JoyconManager.Instance.j;

        Debug.Log("Joycons count = " + joycons.Count);

        if (joycons.Count > 0)
        {
            joycon = joycons[0];
            Debug.Log("✅ Joycon assigned");
        }
        else
        {
            Debug.LogError("❌ Joy-Con not found");
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        

        // ==============================
        // 🔽 ここを追加（位置復元）
        // ==============================
      //  if (GameStateManager.Instance != null)
        //{
          //  transform.position = GameStateManager.Instance.playerPosition;
          //  Debug.Log("🚃 プレイヤー位置を復元: " + transform.position);
        //}

    }

    void Update()
    {
        if (joycon == null) return;

        // ===== スティック（float[]）=====
        float[] stick = joycon.GetStick();
        float stickX = stick[0];
        float stickY = stick[1];

        // ===== ZR =====
        bool rotateMode = joycon.GetButton(Joycon.Button.SHOULDER_2);

        // ===== 移動 =====
        if (!rotateMode)
        {
            Vector3 dir =
                transform.right * stickX +
                transform.forward * stickY;

            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        // ===== 視点操作 =====
        else
        {
            float lookX = stickX * rotateSpeed * Time.deltaTime;
            float lookY = stickY * rotateSpeed * Time.deltaTime;

            xRotation -= lookY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation =
                Quaternion.Euler(xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * lookX);
        }
    }
}
