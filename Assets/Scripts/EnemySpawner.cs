using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyTypes; // List of different enemy ScriptableObjects
    public Transform player;
    public float initialSpawnRate = 2f;
    public int maxEnemies = 20;

    private Camera mainCamera;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public Tilemap backgroundTilemap;
    private BoundsInt tilemapBounds;

    public float recycleDistanceThreshold = 20f;
    private int availableEnemyIndex = 0;
    private float nextIntervalTime = 90f;

    public float timeBeforeScaling = 30f;
    public float scalingRate = 0.95f;
    public float minSpawnRate = 0.5f;
    private float currentSpawnRate;

    public float spawnRadiusFromCamera = 1f;
    public float spawnBuffer = 0.5f;

    private void Start()
    {
        mainCamera = Camera.main;
        currentSpawnRate = initialSpawnRate;
        tilemapBounds = backgroundTilemap.cellBounds;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(UnlockEnemyTypes());
        StartCoroutine(ScaleSpawnRateOverTime());
    }

    private void Update()
    {
        RecycleFarAwayEnemies();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                Vector3 spawnPosition = GetSpawnPositionJustOutsideCamera();

                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    EnemyData enemyToSpawn = enemyTypes[Random.Range(0, availableEnemyIndex + 1)];
                    GameObject enemy = Instantiate(enemyToSpawn.prefab, hit.position, Quaternion.identity);
                    spawnedEnemies.Add(enemy);

                    EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
                    if (behaviour != null)
                    {
                        behaviour.enemyData = enemyToSpawn;
                        behaviour.SetTarget(player);
                    }
                }
                else
                {
                    Debug.LogWarning("Generated position is not on the NavMesh. Skipping spawn.");
                }
            }

            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    private IEnumerator UnlockEnemyTypes()
    {
        while (availableEnemyIndex < enemyTypes.Count - 1)
        {
            yield return new WaitForSeconds(nextIntervalTime);
            availableEnemyIndex++;
            Debug.Log("Unlocked new enemy type: " + enemyTypes[availableEnemyIndex].name);
        }
    }

    private IEnumerator ScaleSpawnRateOverTime()
    {
        yield return new WaitForSeconds(timeBeforeScaling);

        while (currentSpawnRate > minSpawnRate)
        {
            currentSpawnRate *= scalingRate;
            if (currentSpawnRate < minSpawnRate)
            {
                currentSpawnRate = minSpawnRate;
            }
            Debug.Log("Spawn rate scaled to: " + currentSpawnRate);
            yield return new WaitForSeconds(30f);
        }
    }

    private Vector3 GetSpawnPositionJustOutsideCamera()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        float spawnRadius = Mathf.Max(cameraWidth, cameraHeight) / 2 + spawnBuffer;

        float spawnX = cameraPosition.x + spawnRadius * Mathf.Cos(randomAngle);
        float spawnY = cameraPosition.y + spawnRadius * Mathf.Sin(randomAngle);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        if (tilemapBounds.Contains(backgroundTilemap.WorldToCell(spawnPosition)))
        {
            return spawnPosition;
        }

        return GetSpawnPositionJustOutsideCamera();
    }

    private void RecycleFarAwayEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnedEnemies[i].transform.position, player.position);

                if (distanceToPlayer > recycleDistanceThreshold)
                {
                    // Calculate the direction of the enemy relative to the player
                    Vector3 enemyDirection = (spawnedEnemies[i].transform.position - player.position).normalized;

                    // Generate a new spawn position opposite or orthogonal to the current direction
                    Vector3 newSpawnDirection;
                    if (Random.value > 0.5f)
                    {
                        // Flip the direction to spawn on the opposite side
                        newSpawnDirection = -enemyDirection;
                    }
                    else
                    {
                        // Generate a position orthogonal to the enemy's current direction
                        newSpawnDirection = new Vector3(-enemyDirection.y, enemyDirection.x, 0).normalized;
                    }

                    // Calculate a new spawn position using the player's position and spawn radius
                    Vector3 newSpawnPosition = player.position + newSpawnDirection * recycleDistanceThreshold;

                    // Ensure the position is valid (on the NavMesh and within tilemap bounds)
                    if (NavMesh.SamplePosition(newSpawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas) &&
                        tilemapBounds.Contains(backgroundTilemap.WorldToCell(newSpawnPosition)))
                    {
                        spawnedEnemies[i].transform.position = hit.position;
                    }
                    else
                    {
                        // If the position is invalid, fallback to a random valid position
                        spawnedEnemies[i].transform.position = GetSpawnPositionJustOutsideCamera();
                    }
                }
            }
        }
    }
}
