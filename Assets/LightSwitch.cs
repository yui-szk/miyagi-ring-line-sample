using UnityEngine.InputSystem; 
using UnityEngine;

    
public class LightSwitch : MonoBehaviour
{
    public GameObject flashLight;

    void Update()
    {
        // Zキーを押した時（条件）
        if(Keyboard.current.zKey.wasPressedThisFrame)
        {
            // ライトをアクティブ状態にする・・・＞ライトが点く
            flashLight.SetActive(true);
        }
        // Xキーを押した時（条件）
        else if(Keyboard.current.xKey.wasPressedThisFrame)
        {
            // ライトをノン・アクティブ状態にする・・・＞ライトが消える
            flashLight.SetActive(false);
        }
    }
}

