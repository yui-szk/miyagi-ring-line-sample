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
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        switch (currentScene)
        {
            case "train0":
                nextScene = "Talk_Take";
                break;
            case "train1":
                nextScene = "Talk_Garasu";
                break;
            case "train2":
                nextScene = "Talk_Washi";
                break;
            case "train3":
                nextScene = "Talk_Fude_Suzuri";
                break;
            default:
                Debug.LogWarning("未定義のシーンからの遷移です: " + currentScene);
                return;
        }

        if (!string.IsNullOrEmpty(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}