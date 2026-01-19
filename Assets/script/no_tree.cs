using UnityEngine;
using System.Collections.Generic;

public class MassPlaceTreesAvoidMovingTrain : MonoBehaviour
{
    [Header("Terrain & Tree Settings")]
    public Terrain terrain;
    public int numberOfTrees = 1000;

    [Header("Train Settings")]
    public GameObject train;
    public float trainPadding = 2f;

    private BoxCollider trainCollider;
    private TerrainData terrainData;
    private List<int> treeIndicesToRemove = new List<int>();

    void Start()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
            if (terrain == null)
            {
                Debug.LogError("Scene Terrain not found");
                return;
            }
        }

        terrainData = terrain.terrainData;

        if (train != null)
        {
            trainCollider = train.GetComponent<BoxCollider>();
            if (trainCollider == null)
            {
                Debug.LogError("Train BoxCollider not found");
                return;
            }
        }

        PlaceTrees();
    }

    void Update()
    {
        if (trainCollider != null)
        {
            RemoveTreesInsideTrain();
        }
    }

    void PlaceTrees()
    {
        int placedCount = 0;
        int attempts = 0;

        while (placedCount < numberOfTrees && attempts < numberOfTrees * 10)
        {
            attempts++;

            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y;
            Vector3 worldPos = new Vector3(x, y, z) + terrain.transform.position;

            if (trainCollider != null)
            {
                Bounds paddedBounds = trainCollider.bounds;
                paddedBounds.Expand(trainPadding);
                if (paddedBounds.Contains(worldPos))
                {
                    continue;
                }
            }

            TreeInstance treeInstance = new TreeInstance();
            treeInstance.position = new Vector3(
                x / terrainData.size.x,
                0,
                z / terrainData.size.z
            );
            treeInstance.prototypeIndex = 0;
            treeInstance.widthScale = 1f;
            treeInstance.heightScale = 1f;
            treeInstance.color = Color.white;
            treeInstance.lightmapColor = Color.white;

            terrain.AddTreeInstance(treeInstance);
            placedCount++;
        }

        Debug.Log($"Placed Trees: {placedCount}");
    }

    void RemoveTreesInsideTrain()
    {
        TreeInstance[] trees = terrain.terrainData.treeInstances;
        treeIndicesToRemove.Clear();

        for (int i = 0; i < trees.Length; i++)
        {
            Vector3 worldPos = Vector3.Scale(trees[i].position, terrainData.size) + terrain.transform.position;
            Bounds paddedBounds = trainCollider.bounds;
            paddedBounds.Expand(trainPadding);

            if (paddedBounds.Contains(worldPos))
            {
                treeIndicesToRemove.Add(i);
            }
        }

        if (treeIndicesToRemove.Count > 0)
        {
            List<TreeInstance> newTrees = new List<TreeInstance>(trees);
            treeIndicesToRemove.Sort((a, b) => b.CompareTo(a));
            foreach (int index in treeIndicesToRemove)
            {
                newTrees.RemoveAt(index);
            }

            terrain.terrainData.treeInstances = newTrees.ToArray();
        }
    }
}
