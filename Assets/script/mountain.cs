using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public Terrain terrain;

    [Header("ノイズ設定")]
    public float hillNoiseScale = 0.002f;   // 丘のノイズ（少なめ）
    public float mountainNoiseScale = 0.0008f; // 大きな山（もっとまばら）
    public float heightScale = 30f;

    [Header("山パッチ設定")]
    public float patchNoiseScale = 0.0015f;   // これで島状にする
    public float patchThreshold = 0.6f;       // パッチができる割合

    [Header("電車ルート設定")]
    public Transform[] trainPathPoints;
    public float flatRadius = 80f;     // 完全に平地
    public float startHillRadius = 150f; // この距離から山を許可（ここ大事！）
    public float smoothSamples = 150;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        TerrainData td = terrain.terrainData;

        int w = td.heightmapResolution;
        int h = td.heightmapResolution;

        float[,] heights = new float[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                Vector3 world = new Vector3(
                    (float)x / w * td.size.x,
                    0,
                    (float)z / h * td.size.z
                );

                // 線路からの最短距離を取得
                float minDist = GetMinDistanceToPath(world);

                // ① 道の近くは完全に平地
                if (minDist < flatRadius)
                {
                    heights[x, z] = 0f;
                    continue;
                }

                // ② startHillRadius までの間は徐々に山が出現
                float t = Mathf.InverseLerp(flatRadius, startHillRadius, minDist);

                // ③ パッチ（島状）ノイズ
                float patch = Mathf.PerlinNoise(
                    world.x * patchNoiseScale,
                    world.z * patchNoiseScale
                );

                // 山が出るかどうか（島状分布）
                bool allowHill = patch > patchThreshold;

                if (!allowHill)
                {
                    heights[x, z] = 0f;
                    continue;
                }

                // ④ 丘の基本ノイズ
                float hill = Mathf.PerlinNoise(
                    world.x * hillNoiseScale,
                    world.z * hillNoiseScale
                );

                // ⑤ もっと遠くは大きな山ノイズも追加
                float mountain = Mathf.PerlinNoise(
                    world.x * mountainNoiseScale,
                    world.z * mountainNoiseScale
                );

                float height = (hill * 0.7f + mountain * 0.3f) * (heightScale / td.size.y);

                // ⑥ 距離によって高さを補正（近いほど低く）
                height *= t;

                heights[x, z] = height;
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);
    }

    // ★ 線路に最も近い距離を求める
    float GetMinDistanceToPath(Vector3 pos)
    {
        float minDist = Mathf.Infinity;

        for (int i = 0; i < trainPathPoints.Length; i++)
        {
            Vector3 p1 = trainPathPoints[i].position;
            Vector3 p2 = trainPathPoints[(i + 1) % trainPathPoints.Length].position;

            Vector3 closest = ClosestPointOnLineSegment(p1, p2, pos);
            float dist = Vector3.Distance(closest, pos);
            if (dist < minDist) minDist = dist;
        }
        return minDist;
    }

    // ★ 線分上の最接近点
    Vector3 ClosestPointOnLineSegment(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ap = p - a;
        Vector3 ab = b - a;
        float t = Vector3.Dot(ap, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }
}
