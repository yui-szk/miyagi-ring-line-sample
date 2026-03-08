using UnityEngine;

public class TrainBGMStarter : MonoBehaviour
{
    [SerializeField] AudioClip trainBGM;

    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(trainBGM);
        }
    }
}