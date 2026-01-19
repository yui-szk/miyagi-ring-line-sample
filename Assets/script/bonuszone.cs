using UnityEngine;

public class BonusZoneMover : MonoBehaviour
{
    [Header("‰¡ˆÚ“®”ÍˆÍ")]
    public float minX = -3f;
    public float maxX = 3f;

    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed = 2f;

    [Header("‚‚³ŒÅ’è")]
    public float fixedY = 3f;

    private bool active = false;
    private int direction = 1;

    void Start()
    {
        SetRandomPosition();
    }

    void Update()
    {
        if (!active) return;

        // ‰¡ˆÚ“®
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        // ’[‚Å”½“]
        if (transform.position.x > maxX) direction = -1;
        else if (transform.position.x < minX) direction = 1;

        // ‚‚³ŒÅ’è
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }

    public void StartBonus()
    {
        SetRandomPosition();
        direction = Random.value > 0.5f ? 1 : -1;
        active = true;
    }

    public void EndBonus()
    {
        active = false;
    }

    void SetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        transform.position = new Vector3(randomX, fixedY, transform.position.z);
    }
}
