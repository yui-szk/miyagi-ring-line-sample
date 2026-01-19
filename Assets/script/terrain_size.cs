using UnityEngine;

public class TerrainScaler : MonoBehaviour
{
    public Terrain terrain;
    public float newSizeX = 1500f;
    public float newSizeZ = 1500f;

    [ContextMenu("Scale Terrain While Keeping Center")]
    void ScaleTerrain()
    {
        if (terrain == null) return;

        TerrainData td = terrain.terrainData;

        // 旧サイズ
        Vector3 oldSize = td.size;

        // 現在の中心位置
        Vector3 center = terrain.transform.position + oldSize / 2f;

        // 新サイズ
        Vector3 newSize = new Vector3(newSizeX, oldSize.y, newSizeZ);

        // TerrainData を更新
        td.size = newSize;

        // 中心位置が変わらないよう逆算
        terrain.transform.position = center - newSize / 2f;

        Debug.Log("Terrain scaled while keeping center fixed!");
    }
}
