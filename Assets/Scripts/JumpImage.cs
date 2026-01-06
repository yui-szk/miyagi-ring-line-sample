using UnityEngine;
using UnityEngine.UI; // UI要素を操作するために必要

/// <summary>
/// Canvas上のImageオブジェクトを上下にジャンプさせるスクリプト
/// </summary>
public class JumpImage : MonoBehaviour
{
    [Header("ジャンプ設定")]
    [Tooltip("上下移動の速さ")]
    public float jumpSpeed = 2f;

    [Tooltip("上下移動の振幅（ジャンプの高さ）")]
    public float jumpHeight = 2f;

    [Tooltip("初期位置からのオフセット")]
    public float offset = 0f;
    
    // オブジェクトの初期Y座標（ワールド座標ではなくローカル座標を使用）
    private float startY; 
    
    // RectTransformはUI要素の位置とサイズを制御するコンポーネント
    private RectTransform rectTransform;

    void Start()
    {
        // 必須のRectTransformコンポーネントを取得
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("このスクリプトはRectTransformを持つUI要素にアタッチしてください。");
            enabled = false; // スクリプトを無効化
            return;
        }

        // 最初のフレームでオブジェクトのローカルY座標を保存
        startY = rectTransform.anchoredPosition.y;
    }

    void Update()
    {
        // Time.time * jumpSpeed により、時間と共に増加する値を生成
        // Mathf.Sin() はこの値を -1.0 から 1.0 の間で滑らかに変化させる
        float sinValue = Mathf.Sin(Time.time * jumpSpeed + offset);

        // sinValue (-1.0 ~ 1.0) に jumpHeight を掛けて振幅を決定
        float movement = sinValue * jumpHeight;

        // 新しいY座標を計算: 初期Y座標 + 移動量
        float newY = startY + movement;

        // RectTransformのアンカーされた位置（ローカル座標）を更新
        // X座標は変更しない
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}