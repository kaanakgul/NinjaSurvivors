using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public Transform player; // Reference to the player
    public float spawnRadius = 20f; // Distance from the player at which enemies spawn
    public float spawnRate = 2f; // Time between spawns
    public int maxEnemies = 20; // Maximum number of enemies to spawn
    private Camera mainCamera; // Reference to the main camera

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    private void Start()
    {
        mainCamera = Camera.main; // Get reference to the main camera
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                // Calculate a random position outside the camera's view and within a certain radius from the player
                Vector3 spawnPosition = GetSpawnPositionOutsideCamera();

                // Instantiate the enemy at the spawn position and keep track of it
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                // Set the target (player) for the enemy to move towards
                enemy.GetComponent<EnemyBehaviour>().SetTarget(player);
            }

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(spawnRate);
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

        // Calculate the camera bounds
        float xMin = cameraPosition.x - cameraWidth / 2;
        float xMax = cameraPosition.x + cameraWidth / 2;
        float yMin = cameraPosition.y - cameraHeight / 2;
        float yMax = cameraPosition.y + cameraHeight / 2;

        int maxAttempts = 100; // Safeguard: max number of attempts to find a valid spawn point
        int attempts = 0;

        do
        {
            // Get a random direction in a circle
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Calculate a spawn position in a circle around the player
            spawnPosition = player.position + (Vector3)(randomDirection * spawnRadius);

            attempts++;

            // Break out of the loop if too many attempts have been made to find a valid position
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Max attempts reached for finding a valid spawn position.");
                break;
            }

        } while (spawnPosition.x > xMin && spawnPosition.x < xMax && spawnPosition.y > yMin && spawnPosition.y < yMax);

        return spawnPosition;
    }

}
