using UnityEngine;

public class FlashlightSensor : MonoBehaviour
{
    public float range = 10f; // ライトの届く距離

    void Update()
    {
        // 懐中電灯の正面方向にレイを飛ばす
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            // 当たったオブジェクトのタグを確認
            if (hit.collider.CompareTag("TargetImage"))
            {
                // 画像を削除（または非表示）
                Destroy(hit.collider.gameObject);
                Debug.Log("画像を消去しました！");
            }
        }
    }

    // Debug: デバッグ用にシーン画面で線を表示
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(transform.position, transform.forward * range);
    // }
}