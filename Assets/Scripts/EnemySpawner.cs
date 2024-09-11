using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The enemy prefab to spawn
    public Transform player;        // Reference to the player's Transform
    public float spawnInterval = 2f; // Time between each spawn
    public int maxEnemies = 10;      // Maximum number of enemies
    public float spawnRadius = 5f;   // Radius around the player to spawn enemies

    private int enemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyCount < maxEnemies)
        {
            SpawnEnemyAroundPlayer();
            enemyCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemyAroundPlayer()
    {
        if (player != null)
        {
            // Generate a random position within a radius around the player
            Vector2 randomPosition = (Vector2)player.position + Random.insideUnitCircle * spawnRadius;

            // Instantiate the enemy at the random position
            GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            // Assign the player's Transform to the enemy's script
            newEnemy.GetComponent<EnemyBehaviour>().SetTarget(player);
        }
        else
        {
            Debug.LogWarning("Player reference is missing!");
        }
    }
}
