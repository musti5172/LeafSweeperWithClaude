using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private int leafCount = 600;
    [SerializeField] private float areaSize = 9f;
    [SerializeField] private float spawnHeight = 1f;

    private void Start()
    {
        for (int i = 0; i < leafCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-areaSize, areaSize),
                spawnHeight,
                Random.Range(-areaSize, areaSize));

            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Instantiate(leafPrefab, pos, rot);
        }
    }
}