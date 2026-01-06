using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MainScene : MonoBehaviour {

    void Update () {
        foreach (var device in InputSystem.devices)
        {
            if (device.name.Contains("Joy-Con"))
            {
                // デバイス内の "button4" を直接取得
                var targetButton = device["button4"] as ButtonControl;

                // button4が存在し、かつ今押された瞬間であれば実行
                if (targetButton != null && targetButton.wasPressedThisFrame)
                {
                    Debug.Log("button4（Aボタン/右ボタン）の入力を検知しました。");
                    ChangeScene();
                }
            }
        }

        // デバッグ用：PCのEnterキーでも動くようにしておくと便利です
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("washi_delete-ghost");
    }
}