using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public float speed = 2f;  // 1秒あたり何ユニット動くか

    void Update()
    {
        // ワールド座標系で X 方向に平行移動
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
}
