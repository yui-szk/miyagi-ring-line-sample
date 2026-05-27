using UnityEngine;

public class BlinkUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [SerializeField]
    float speed = 2f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * speed));

        canvasGroup.alpha = alpha;
    }
}