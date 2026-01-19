using UnityEngine;
using System.Collections.Generic;

public class VesselController : MonoBehaviour
{
    // JoyconManagerからJoyconのリストを取得
    private List<Joycon> joycons;
    // 接続されているJoy-Conのインデックス（通常は0:L, 1:R）
    public int targetJoyconIndex = 0; // 例: 左Joy-Conを使用
    // 傾きから移動速度への変換係数
    public float moveSpeed = 5f;
    // 器が移動できるX座標の範囲
    public float xBoundary = 4f;

    void Start()
    {
        // JoyconManagerのインスタンスからJoy-Conのリストを取得
        joycons = JoyconManager.Instance.j;

        if (joycons.Count <= targetJoyconIndex)
        {
            Debug.LogError("指定されたJoy-Conが見つかりません。インデックスを確認してください。");
            enabled = false;
        }
    }

    void Update()
    {
        if (joycons.Count > targetJoyconIndex)
        {
            Joycon j = joycons[targetJoyconIndex];

            // --- 傾きによる移動 ---
            // 加速度のX軸成分を取得 (左右の傾きで値が変わる)
            // 傾きに応じて左右の移動量に変換
            Vector3 accel = j.GetAccel();

            // X軸の加速度を利用して移動量を計算
            // Joy-Conを傾けるとaccel.xが負/正に変化する
            float horizontalInput = -accel.x; // 必要に応じて符号を反転

            // 現在のX座標を計算
            float newX = transform.position.x + horizontalInput * moveSpeed * Time.deltaTime;

            // X座標の移動範囲を制限
            newX = Mathf.Clamp(newX, -xBoundary, xBoundary);

            // 器の位置を更新 (Y, Zは固定)
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}