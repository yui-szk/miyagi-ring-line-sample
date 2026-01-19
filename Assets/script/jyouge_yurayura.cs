using UnityEngine;

public class jyouge_yurayura : MonoBehaviour
{
    [Header("•‚‚­İ’è")]
    public float amplitude = 0.5f; // ã‰º‚ÌU‚ê•
    public float speed = 1.0f;     // —h‚ê‚é‘¬‚³

    private Vector3 startPos;

    void Start()
    {
        // ŠJn‚ÌˆÊ’u‚ğ•Û‘¶
        startPos = transform.position;
    }

    void Update()
    {
        // ƒTƒCƒ“”g‚Åã‰º‚É—h‚ç‚·
        float y = Mathf.Sin(Time.time * speed) * amplitude;

        // Œ³‚ÌˆÊ’u + ã‰º—h‚ê
        transform.position = new Vector3(
            startPos.x,
            startPos.y + y,
            startPos.z
        );
    }
}
