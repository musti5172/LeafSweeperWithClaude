using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private int leafCount = 600;

    [SerializeField] private GameObject goldenLeafPrefab;
    [SerializeField] private int goldenLeafCount = 20;

    [SerializeField] private float areaSize = 9f;
    [SerializeField] private float spawnHeight = 1f;

    private void Start()
    {
        Spawn(leafPrefab, leafCount);
        Spawn(goldenLeafPrefab, goldenLeafCount);
    }

    private void Spawn(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-areaSize, areaSize),
                spawnHeight,
                Random.Range(-areaSize, areaSize));

            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Instantiate(prefab, pos, rot);
        }
    }
}