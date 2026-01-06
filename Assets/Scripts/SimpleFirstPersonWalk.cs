using UnityEngine;

public class SimpleFirstPersonWalk : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 5f;
    public Transform cameraTransform; // 子の Main Camera をセット

    [Header("Head Bob")]
    public float bobSpeed = 8f;      // 歩行時の揺れスピード
    public float bobAmount = 0.05f;  // 揺れの大きさ

    float xRotation = 0f;
    float defaultCamPosY;
    float bobTimer = 0f;
    Vector3 lastPosition; // 速度計算用

    void Start()
    {
        // Start時にカーソルを非表示/ロックする処理は維持
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        defaultCamPosY = cameraTransform.localPosition.y;
        lastPosition = transform.position;
    }

    void Update()
    {
        // 🚨 修正: HandleMouseLook() は Update() から直接呼び出す
        HandleMouseLook(); 
        
        HandleMove();
        HandleHeadBob();
    }

    void HandleMouseLook()
    {
        // 🚀 修正点 1: 左クリックが押されているかチェック
        if (Input.GetMouseButton(0)) // 0 はマウスの左ボタン
        {
            // 左クリックされている間、カーソルをロック状態に維持
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                 Cursor.lockState = CursorLockMode.Locked;
                 Cursor.visible = false;
            }

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // カメラの上下回転
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            // プレイヤー本体の左右回転
            transform.Rotate(Vector3.up * mouseX);
        }
        // 🚀 修正点 2: 左クリックが離されたらカーソルを解放
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleHeadBob()
    {
        // 現在の速度（移動量）を算出
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // XZ 平面の移動量だけを見る
        Vector2 horizontalVel = new Vector2(velocity.x, velocity.z);
        bool isMoving = horizontalVel.magnitude > 0.1f;

        if (isMoving)
        {
            // 歩行中：Sin波で上下揺れ
            bobTimer += Time.deltaTime * bobSpeed;
            float newY = defaultCamPosY + Mathf.Sin(bobTimer) * bobAmount;

            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = newY;
            cameraTransform.localPosition = localPos;
        }
        else
        {
            // 停止中：元の位置へスムーズに戻す
            bobTimer = 0f;
            Vector3 localPos = cameraTransform.localPosition;
            localPos.y = Mathf.Lerp(localPos.y, defaultCamPosY, Time.deltaTime * bobSpeed);
            cameraTransform.localPosition = localPos;
        }
    }
}