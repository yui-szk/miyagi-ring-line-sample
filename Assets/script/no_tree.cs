using UnityEngine;
using System.Collections.Generic;

public class NoTreeAlongTrainRoute : MonoBehaviour
{
    [Header("Terrain")]
    public Terrain terrain;

    [Header("Tree Settings")]
    public int numberOfTrees = 2000;
    public float minScale = 0.8f;
    public float maxScale = 1.5f;

    [Header("Train Route (Catmull-Rom)")]
    public Transform[] routePoints;
    public float routeClearRadius = 20f;   // 線路から何m木を消すか
    public float sampleStep = 0.05f;       // 小さいほど正確（0.02〜0.1）

    TerrainData terrainData;
    List<Vector3> sampledRoutePoints = new List<Vector3>();

    void Start()
    {
        if (!terrain)
            terrain = Terrain.activeTerrain;

        if (!terrain || routePoints == null || routePoints.Length < 4)
        {
            Debug.LogError("Terrain または RoutePoints が正しく設定されていません");
            return;
        }

        terrainData = terrain.terrainData;

        SampleRoute();     // ★ 曲線を細かくサンプリング
        PlaceTrees();      // ★ 木を配置（ルート除外）
    }

    // ==================================================
    // ① 曲線サンプリング（Catmull-Rom）
    // ==================================================
    void SampleRoute()
    {
        sampledRoutePoints.Clear();

        int count = routePoints.Length;

        for (int i = 0; i < count; i++)
        {
            Vector3 p0 = routePoints[(i - 1 + count) % count].position;
            Vector3 p1 = routePoints[i].position;
            Vector3 p2 = routePoints[(i + 1) % count].position;
            Vector3 p3 = routePoints[(i + 2) % count].position;

            for (float t = 0; t < 1f; t += sampleStep)
            {
                Vector3 pos = CatmullRom(p0, p1, p2, p3, t);
                sampledRoutePoints.Add(pos);
            }
        }
    }

    // ==================================================
    // ② 木の配置（ルート回避）
    // ==================================================
    void PlaceTrees()
    {
        List<TreeInstance> trees = new List<TreeInstance>();

        int placed = 0;
        int attempts = 0;

        while (placed < numberOfTrees && attempts < numberOfTrees * 10)
        {
            attempts++;

            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            Vector3 worldPos =
                terrain.transform.position +
                new Vector3(x, 0f, z);

            // ★ ルート近くなら配置しない
            if (IsNearRouteXZ(worldPos))
                continue;

            TreeInstance tree = new TreeInstance
            {
                position = new Vector3(
                    x / terrainData.size.x,
                    0f,
                    z / terrainData.size.z
                ),
                prototypeIndex = 0,
                widthScale = Random.Range(minScale, maxScale),
                heightScale = Random.Range(minScale, maxScale),
                color = Color.white,
                lightmapColor = Color.white
            };

            trees.Add(tree);
            placed++;
        }

        terrainData.treeInstances = trees.ToArray();
        Debug.Log($"木配置完了: {placed} 本");
    }

    // ==================================================
    // ③ XZ平面のみで距離判定（重要）
    // ==================================================
    bool IsNearRouteXZ(Vector3 worldPos)
    {
        Vector2 wp = new Vector2(worldPos.x, worldPos.z);

        foreach (var p in sampledRoutePoints)
        {
            Vector2 rp = new Vector2(p.x, p.z);

            if (Vector2.Distance(wp, rp) < routeClearRadius)
                return true;
        }
        return false;
    }

    // ==================================================
    // ④ Catmull-Rom 補間
    // ==================================================
    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f *
            ((2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
    }

    // ==================================================
    // ⑤ ルート可視化（Scene確認用）
    // ==================================================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (sampledRoutePoints == null) return;

        foreach (var p in sampledRoutePoints)
        {
            Gizmos.DrawSphere(p, 0.5f);
        }
    }
}
