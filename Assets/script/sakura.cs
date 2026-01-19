using UnityEngine;

public class SakuraWind : MonoBehaviour
{
    public Transform train;  // 電車参照
    public ParticleSystem ps;

    void Update()
    {
        if (!train || !ps) return;

        var vel = ps.velocityOverLifetime;

        // 電車の進行方向に花びらを流す
        Vector3 f = train.forward * 1.5f;
        vel.x = f.x;
        vel.z = f.z;

        // 落下感
        vel.y = -2.0f;
    }
}
