using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyTypes; // List of different enemy types
    public Transform player; // Reference to the player
    public float initialSpawnRate = 2f; // Initial time between spawns
    public int maxEnemies = 20; // Maximum number of enemies to spawn
    private Camera mainCamera; // Reference to the main camera
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    public Tilemap backgroundTilemap; // Reference to the Tilemap used as the background
    private BoundsInt tilemapBounds;

    public float recycleDistanceThreshold = 100f; // Distance threshold to recycle enemies
    private int availableEnemyIndex = 0; // Tracks how many enemy types are unlocked
    private float nextIntervalTime = 90f; // Initial time to unlock the next enemy type

    // Variables to control spawn rate scaling
    public float timeBeforeScaling = 30f; // How much time (in seconds) before spawn rate starts scaling
    public float scalingRate = 0.95f; // Multiplier to decrease spawn rate (95% of the previous spawn rate)
    public float minSpawnRate = 0.5f; // Minimum spawn rate (to avoid spawns being too fast)
    private float currentSpawnRate; // Current spawn rate

    public float spawnRadiusFromCamera = 1f; // Radius just outside the camera to spawn enemies
    public float spawnBuffer = 0.5f; // Buffer to keep enemies just outside view

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to the main camera
        currentSpawnRate = initialSpawnRate; // Set the initial spawn rate
        tilemapBounds = backgroundTilemap.cellBounds; // Get the bounds of the tilemap

        StartCoroutine(SpawnEnemies());
        StartCoroutine(UnlockEnemyTypes());
        StartCoroutine(ScaleSpawnRateOverTime()); // Coroutine to scale the spawn rate over time
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
                    // If the position is on the NavMesh, proceed with spawning
                    EnemyData enemyToSpawn = enemyTypes[Random.Range(0, availableEnemyIndex + 1)];
                    GameObject enemy = Instantiate(enemyToSpawn.prefab, hit.position, Quaternion.identity);
                    spawnedEnemies.Add(enemy);

                    enemy.GetComponent<EnemyBehaviour>().SetTarget(player);
                    enemy.GetComponent<EnemyBehaviour>().SetStats(enemyToSpawn.health, enemyToSpawn.speed, enemyToSpawn.damage, enemyToSpawn.attackSpeed);
                }
                else
                {
                    Debug.LogWarning("Generated position is not on the NavMesh. Skipping spawn.");
                }
            }

            yield return new WaitForSeconds(currentSpawnRate); // Use the current spawn rate
        }
    }

    // Coroutine to unlock new enemy types at set time intervals
    private IEnumerator UnlockEnemyTypes()
    {
        while (availableEnemyIndex < enemyTypes.Count - 1)
        {
            yield return new WaitForSeconds(nextIntervalTime); // Wait for the interval to pass
            availableEnemyIndex++; // Unlock the next enemy type
            Debug.Log("Unlocked new enemy type: " + enemyTypes[availableEnemyIndex].prefab.name);
        }
    }

    // Coroutine to scale spawn rate over time
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

    // Method to get a random spawn position just outside the camera's view within a radius
    private Vector3 GetSpawnPositionJustOutsideCamera()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Random angle to place enemies around the camera
        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        // Adjust radius to be just outside the camera view
        float spawnRadius = Mathf.Max(cameraWidth, cameraHeight) / 2 + spawnBuffer;

        // Calculate spawn position using polar coordinates
        float spawnX = cameraPosition.x + spawnRadius * Mathf.Cos(randomAngle);
        float spawnY = cameraPosition.y + spawnRadius * Mathf.Sin(randomAngle);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        // Ensure the spawn position is within tilemap bounds
        if (tilemapBounds.Contains(backgroundTilemap.WorldToCell(spawnPosition)))
        {
            return spawnPosition;
        }

        // If the position is out of bounds, recalculate until it is valid
        return GetSpawnPositionJustOutsideCamera();
    }

    // Method to recycle enemies that are too far away from the player
    private void RecycleFarAwayEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnedEnemies[i].transform.position, player.position);
                if (distanceToPlayer > recycleDistanceThreshold)
                {
                    // Move the enemy to a new position outside the camera view but closer to the player
                    Vector3 newSpawnPosition = GetSpawnPositionJustOutsideCamera();

                    if (NavMesh.SamplePosition(newSpawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    {
                        spawnedEnemies[i].transform.position = hit.position;
                    }
                }
            }
        }
    }
}
