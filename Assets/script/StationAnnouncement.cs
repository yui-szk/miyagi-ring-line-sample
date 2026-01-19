using UnityEngine;

public class StationAnnouncement : MonoBehaviour
{
    public TrainSpline trainSpline;

    void Start()
    {
        if (trainSpline == null)
        {
            trainSpline = FindObjectOfType<TrainSpline>();
        }

        if (trainSpline == null)
        {
            Debug.LogError("TrainSpline がシーン内に見つかりません", this);
            enabled = false; // これ以上動かさない
            return;
        }

        Debug.Log("✅ TrainSpline を取得しました");
    }
}
