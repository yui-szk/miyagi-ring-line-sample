using UnityEngine;

public class SakuraScript : MonoBehaviour
{
    [Header("タグ設定")]
    public string vesselTag = "Vessel";
    public string groundTag = "Ground";

    [Header("獲得スコア")]
    public int score = 1;   // ← 通常は1点、特別な花びらは50点

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit : " + other.name + " tag = " + other.tag);

        if (other.CompareTag(vesselTag))
        {
            Debug.Log("器に当たった（桜）");

            Destroy(gameObject);

            if (GameManager2.Instance != null)
            {
                GameManager2.Instance.AddScore(score);
            }
        }
        else if (other.CompareTag(groundTag))
        {
            Debug.Log("地面に落ちた（桜）");
            Destroy(gameObject);
        }
    }
}