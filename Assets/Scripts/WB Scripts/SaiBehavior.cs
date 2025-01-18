using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaiBehavior : WeaponBehavior
{
    private int forwardProjectiles = 0; // Number of forward projectiles
    private int backwardProjectiles = 0; // Number of backward projectiles
    private float fireDelay = 0.2f; // Delay between firing each Sai

    private Queue<Vector2> forwardShots = new Queue<Vector2>(); // Queue for forward shots
    private Queue<Vector2> backwardShots = new Queue<Vector2>(); // Queue for backward shots

    private float lastForwardFire; // Time when the last forward Sai was fired
    private float lastBackwardFire; // Time when the last backward Sai was fired

    protected override void OnLevelUp(int newLevel)
    {
        if (newLevel % 2 == 1)
        {
            // Odd levels add a forward projectile
            forwardProjectiles++;
            Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}! Forward projectiles: {forwardProjectiles}, Backward projectiles: {backwardProjectiles}");
        }
        else
        {
            // Even levels add a backward projectile
            backwardProjectiles++;
            Debug.Log($"{weaponData.weaponName} leveled up to {newLevel}! Forward projectiles: {forwardProjectiles}, Backward projectiles: {backwardProjectiles}");
        }
    }

    public override void Attack(Vector2 direction, float playerSpeed)
    {
        // Enqueue forward projectiles
        for (int i = 0; i < forwardProjectiles; i++)
        {
            forwardShots.Enqueue(direction);
        }

        // Enqueue backward projectiles
        for (int i = 0; i < backwardProjectiles; i++)
        {
            backwardShots.Enqueue(-direction);
        }
    }

    private void Update()
    {
        // Handle forward shots
        if (forwardShots.Count > 0 && Time.time >= lastForwardFire + fireDelay)
        {
            Vector2 direction = forwardShots.Dequeue();
            FireSai(direction, PlayerStats.Instance.moveSpeed, false);
            lastForwardFire = Time.time;
        }

        // Handle backward shots
        if (backwardShots.Count > 0 && Time.time >= lastBackwardFire + fireDelay)
        {
            Vector2 direction = backwardShots.Dequeue();
            FireSai(direction, PlayerStats.Instance.moveSpeed, true);
            lastBackwardFire = Time.time;
        }
    }

    private void FireSai(Vector2 direction, float playerSpeed, bool isBackward)
    {
        Quaternion rotation;
        if (isBackward)
        {
            rotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees to the left
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, -90); // Rotate 90 degrees to the right
        }
        // Instantiate the Sai
        GameObject sai = Instantiate(weaponData.prefab, firePoint.position, rotation);
        Rigidbody2D rb = sai.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction.normalized * (weaponData.Speed + playerSpeed * 0.5f);
            Debug.Log($"Fired Sai in direction: {direction}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DoDamage(collision); // Apply damage to the enemy
            Destroy(gameObject); // Destroy the Sai after it hits an enemy
        }
    }
}
