using UnityEngine;

public class SandScript : MonoBehaviour
{
    [Header("タグ設定")]
    public string vesselTag = "Vessel";  // 器のタグ
    public string groundTag = "Ground";  // 地面のタグ

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(vesselTag))
        {
            // 器に触れたら砂を破壊
            Destroy(gameObject);

            // GameManager が存在する場合はスコア加算
            if (GameManager_Sand.Instance != null)
            {
                GameManager_Sand.Instance.AddScore(1);
            }

            Debug.Log("砂をキャッチしました！");
        }
        else if (other.CompareTag(groundTag))
        {
            // 地面に落ちた砂も破壊して重くならないようにする
            Destroy(gameObject);
        }
    }
}
