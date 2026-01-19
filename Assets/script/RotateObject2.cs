using UnityEngine;

public class RotateObject2 : MonoBehaviour
{
    public Vector3 rotateSpeed = new Vector3(0, 180f, 0);

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
