using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private Object objectToSpawn;

    [SerializeField]
    private Vector2 initialSpawnDelay;

    [SerializeField]
    private Vector2 spawnInterval;

    private static int maxCount = 8;
    public static int currentCount = 0;

    private void Awake()
    {
        StartCoroutine(ISpawnObject(Random.Range(initialSpawnDelay.x, initialSpawnDelay.y)));
    }

    IEnumerator ISpawnObject(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentCount < maxCount)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            currentCount++;
        }

        StartCoroutine(ISpawnObject(Random.Range(spawnInterval.x, spawnInterval.y)));
    }
}