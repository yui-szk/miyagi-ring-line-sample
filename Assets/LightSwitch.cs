using UnityEngine.InputSystem; 
using UnityEngine;

    
public class LightSwitch : MonoBehaviour
{
    public GameObject flashLight;
    public float mouseSensitivity = 0.5f; // マウスの感度

    private Vector2 currentRotation = Vector2.zero; // 現在の回転角度

    void Update()
    {
        // Zキーを押した時（条件）
        if(Keyboard.current.zKey.wasPressedThisFrame)
        {
            // ライトをアクティブ状態にする
            flashLight.SetActive(true);
        }
        // Xキーを押した時（条件）
        else if(Keyboard.current.xKey.wasPressedThisFrame)
        {
            // ライトをノン・アクティブ状態にする
            flashLight.SetActive(false);
        }
        // Rキーを押した時（条件）
        else if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            if(flashLight != null && flashLight.activeSelf)
            {
                // 回転角度をリセット
                currentRotation = Vector2.zero;
            
                // ライトの回転を初期状態に戻す
                flashLight.transform.rotation = Quaternion.identity;
            }
        }

        // マウス操作でライトの角度を調整
        if(flashLight != null && flashLight.activeSelf)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
            // マウスの移動量に感度を掛けて回転量を計算
            currentRotation.y += mouseDelta.x * mouseSensitivity;
            currentRotation.x -= mouseDelta.y * mouseSensitivity;
            
            // X軸の回転を-90度から90度に制限（上下の回転制限）
            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
            
            // ライトの回転を適用
            flashLight.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
        }
    }

}

