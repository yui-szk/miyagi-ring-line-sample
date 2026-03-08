using UnityEngine;
using System.Collections.Generic;

public class JoyconFirstPersonWalk : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 120f;
    public Transform cameraTransform;

    public AudioSource footstepSource; // 👣足音

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

        if (footstepSource != null)
        {
            footstepSource.loop = true;
            footstepSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (joycon == null) return;

        float[] stick = joycon.GetStick();
        float stickX = stick[0];
        float stickY = stick[1];

        bool rotateMode = joycon.GetButton(Joycon.Button.SHOULDER_2);

        // ===== 移動 =====
        if (!rotateMode)
        {
            Vector3 dir =
                transform.right * stickX +
                transform.forward * stickY;

            transform.position += dir * moveSpeed * Time.deltaTime;

            // 👣 足音判定
            bool isMoving = Mathf.Abs(stickX) > 0.1f || Mathf.Abs(stickY) > 0.1f;

            if (isMoving)
            {
                if (!footstepSource.isPlaying)
                {
                    footstepSource.Play();
                }
            }
            else
            {
                if (footstepSource.isPlaying)
                {
                    footstepSource.Stop();
                }
            }
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

            // 視点操作中は足音止める
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }
}