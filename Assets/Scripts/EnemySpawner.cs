using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyTypes; // List of different enemy types
    public Transform player; // Reference to the player
    public float spawnRadius = 500f; // Distance from the player at which enemies spawn
    public float spawnRate = 2f; // Time between spawns
    public int maxEnemies = 20; // Maximum number of enemies to spawn
    private Camera mainCamera; // Reference to the main camera
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    public float maxX, minY, maxY, minX;
    private int availableEnemyIndex = 0; // Tracks how many enemy types are unlocked
    private float nextIntervalTime = 30f; // Initial time to unlock the next enemy type

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to the main camera
        StartCoroutine(SpawnEnemies());
        StartCoroutine(UnlockEnemyTypes());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                // Calculate a random position outside the camera's view and within a certain radius from the player
                Vector3 spawnPosition = GetSpawnPositionOutsideCamera();

                // Choose a random enemy from the unlocked types
                EnemyData enemyToSpawn = enemyTypes[Random.Range(0, availableEnemyIndex + 1)];

                // Instantiate the enemy at the spawn position and keep track of it
                GameObject enemy = Instantiate(enemyToSpawn.prefab, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                // Set the target (player) for the enemy to move towards
                enemy.GetComponent<EnemyBehaviour>().SetTarget(player);

                // Set the stats for the enemy
                enemy.GetComponent<EnemyBehaviour>().SetStats(enemyToSpawn.health, enemyToSpawn.speed, enemyToSpawn.damage, enemyToSpawn.attackSpeed);
            }

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(spawnRate);
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

            // Increase the interval between enemy unlocks (you can adjust this as needed)
            nextIntervalTime += 60f; // Increase the interval (for example, add 60 seconds for each new unlock)
        }
    }

    // Method to get a random spawn position outside the camera's view and within a radius around the player
    private Vector3 GetSpawnPositionOutsideCamera()
    {
        Vector3 spawnPosition;

        // Get the camera's bounds in world coordinates
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Expand the bounds slightly so enemies spawn outside the visible area
        float xMin = cameraPosition.x - cameraWidth / 2 - spawnRadius;
        float xMax = cameraPosition.x + cameraWidth / 2 + spawnRadius;
        float yMin = cameraPosition.y - cameraHeight / 2 - spawnRadius;
        float yMax = cameraPosition.y + cameraHeight / 2 + spawnRadius;

        // Ensure spawn occurs outside of the camera's view, by placing it beyond the bounds
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

        int num = 0;

        while (spawnPosition.x > maxX || spawnPosition.y > maxY || spawnPosition.x < minX || spawnPosition.y < minY)
        {
            side = Random.Range(0, 4); // 0=left, 1=right, 2=top, 3=bottom

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

            num++;

            if (num > 100) return Vector3.zero;
        }

        return spawnPosition;
    }
}
