using UnityEngine;

public class SakuraScript : MonoBehaviour
{
    [Header("タグ設定")]
    public string vesselTag = "Vessel";
    public string groundTag = "Ground";

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit : " + other.name + " tag = " + other.tag);

        if (other.CompareTag(vesselTag))
        {
            Debug.Log("器に当たった（桜）");

            Destroy(gameObject);

            // ★ GameManager2 を使う
            if (GameManager2.Instance != null)
            {
                GameManager2.Instance.AddScore(1);
            }
        }
        else if (other.CompareTag(groundTag))
        {
            Debug.Log("地面に落ちた（桜）");
            Destroy(gameObject);
        }
    }
}
