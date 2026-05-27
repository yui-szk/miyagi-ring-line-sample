using UnityEngine;

public class StartSceneBGM : MonoBehaviour
{
    [SerializeField] AudioClip startBGM;

    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(startBGM, true);
        }
    }
}