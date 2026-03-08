using UnityEngine;

public class GameBGMStarter : MonoBehaviour
{
    [SerializeField] AudioClip gameBGM;

    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(gameBGM);
        }
    }
}