using UnityEngine;

public class ConversationBGMStarter : MonoBehaviour
{
    [SerializeField] AudioClip conversationBGM;

    void Start()
    {
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(conversationBGM);
        }
    }
}