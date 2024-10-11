using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyTypes; // List of different enemy types
    public Transform player; // Reference to the player
    public float spawnRadius = 500f; // Distance from the player at which enemies spawn
    public float initialSpawnRate = 2f; // Initial time between spawns
    public int maxEnemies = 20; // Maximum number of enemies to spawn
    private Camera mainCamera; // Reference to the main camera
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    public float maxX, minY, maxY, minX;
    private int availableEnemyIndex = 0; // Tracks how many enemy types are unlocked
    private float nextIntervalTime = 30f; // Initial time to unlock the next enemy type

    // Variables to control spawn rate scaling
    public float timeBeforeScaling = 30f; // How much time (in seconds) before spawn rate starts scaling
    public float scalingRate = 0.95f; // Multiplier to decrease spawn rate (95% of the previous spawn rate)
    public float minSpawnRate = 0.5f; // Minimum spawn rate (to avoid spawns being too fast)
    private float currentSpawnRate; // Current spawn rate

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to the main camera
        currentSpawnRate = initialSpawnRate; // Set the initial spawn rate
        StartCoroutine(SpawnEnemies());
        StartCoroutine(UnlockEnemyTypes());
        StartCoroutine(ScaleSpawnRateOverTime()); // Coroutine to scale the spawn rate over time
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                Vector3 spawnPosition = GetSpawnPositionOutsideCamera();

                EnemyData enemyToSpawn = enemyTypes[Random.Range(0, availableEnemyIndex + 1)];

                GameObject enemy = Instantiate(enemyToSpawn.prefab, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                enemy.GetComponent<EnemyBehaviour>().SetTarget(player);
                enemy.GetComponent<EnemyBehaviour>().SetStats(enemyToSpawn.health, enemyToSpawn.speed, enemyToSpawn.damage, enemyToSpawn.attackSpeed);
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

            nextIntervalTime += 60f; // Increase the interval (you can adjust this as needed)
        }
    }

    // Coroutine to scale spawn rate over time
    private IEnumerator ScaleSpawnRateOverTime()
    {
        // Wait for a set amount of time before scaling begins
        yield return new WaitForSeconds(timeBeforeScaling);

        while (currentSpawnRate > minSpawnRate)
        {
            // Scale down the spawn rate
            currentSpawnRate *= scalingRate;

            // Ensure that spawn rate doesn't fall below the minimum
            if (currentSpawnRate < minSpawnRate)
            {
                currentSpawnRate = minSpawnRate;
            }

            Debug.Log("Spawn rate scaled to: " + currentSpawnRate);

            // Wait for another interval before scaling again (e.g., every 30 seconds)
            yield return new WaitForSeconds(30f); // You can adjust the interval for scaling
        }
    }

    // Method to get a random spawn position outside the camera's view and within a radius around the player
    private Vector3 GetSpawnPositionOutsideCamera()
    {
        Vector3 spawnPosition;
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float xMin = cameraPosition.x - cameraWidth / 2 - spawnRadius;
        float xMax = cameraPosition.x + cameraWidth / 2 + spawnRadius;
        float yMin = cameraPosition.y - cameraHeight / 2 - spawnRadius;
        float yMax = cameraPosition.y + cameraHeight / 2 + spawnRadius;

        int side = Random.Range(0, 4); // 0=left, 1=right, 2=top, 3=bottom

        switch (side)
        {
            case 0: // Left
                spawnPosition = new Vector3(xMin, Random.Range(yMin, yMax), 0);
                break;
            case 1: // Right
                spawnPosition = new Vector3(xMax, Random.Range(yMin, yMax), 0);
                break;
            case 2: // Top
                spawnPosition = new Vector3(Random.Range(xMin, xMax), yMax, 0);
                break;
            case 3: // Bottom
                spawnPosition = new Vector3(Random.Range(xMin, xMax), yMin, 0);
                break;
            default:
                spawnPosition = new Vector3(xMin, yMin, 0); // Default to bottom-left
                break;
        }

        return spawnPosition;
    }
}
