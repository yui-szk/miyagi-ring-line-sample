using UnityEngine.InputSystem; 
using UnityEngine;
using System.Linq;

    
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
            
                // 親オブジェクトを取得（存在する場合）
                Transform targetTransform = flashLight.transform.parent != null 
                    ? flashLight.transform.parent 
                    : flashLight.transform;
            
                // 親オブジェクトごと回転を初期状態に戻す
                targetTransform.rotation = Quaternion.identity;
            }
        }

        // // Debug: マウス操作でライトの角度を調整
        // if(flashLight != null && flashLight.activeSelf)
        // {
        //     Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
        //     // マウスの移動量に感度を掛けて回転量を計算
        //     currentRotation.y += mouseDelta.x * mouseSensitivity;
        //     currentRotation.x -= mouseDelta.y * mouseSensitivity;
            
        //     // X軸の回転を-90度から90度に制限（上下の回転制限）
        //     currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
            
        //     // 親オブジェクトを取得（存在する場合）
        //     Transform targetTransform = flashLight.transform.parent != null 
        //         ? flashLight.transform.parent 
        //         : flashLight.transform;
            
        //     // 親オブジェクトごと回転を適用
        //     targetTransform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
        // }

        if (flashLight == null || !flashLight.activeSelf) return;

        Vector2 stickInput = Vector2.zero;
        bool deviceFound = false;

        foreach (var device in InputSystem.devices)
        {
            if (device.name.Contains("Joy-Con"))
            {
                // あなたの環境では "hat" という名前でスティック入力が届いています
                var hatControl = device["hat"] as InputControl<Vector2>;
                if (hatControl != null)
                {
                    stickInput = hatControl.ReadValue();
                    
                    // ジョイコン(L)特有の「軸の回転」を補正
                    float tempX = stickInput.x;
                    stickInput.x = stickInput.y;
                    stickInput.y = -tempX;
                    
                    deviceFound = true;
                    break;
                }
            }
        }

        // 入力がある場合の回転処理
        if (deviceFound && stickInput.magnitude > 0.05f)
        {
            // スティック操作は deltaTime と強めの倍率を掛けるのがコツです
            float speed = mouseSensitivity * Time.deltaTime * 30f; 
            
            currentRotation.y += stickInput.x * speed;
            currentRotation.x -= stickInput.y * speed; // 上下反転させたい場合はここを += に
            
            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            Transform targetTransform = flashLight.transform.parent != null 
                ? flashLight.transform.parent 
                : flashLight.transform;
                
            targetTransform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
        }
    }

}

