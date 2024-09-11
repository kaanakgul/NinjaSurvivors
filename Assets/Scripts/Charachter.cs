using UnityEngine;

public class Charachter : MonoBehaviour
{
    public float health = 100.0f;
    public bool isDead = false;

    private void FixedUpdate()
    {
        // Check if health is less than or equal to 0 and the character is not already dead
        if (health <= 0 && !isDead)
        {
            health = 0; // Clamp health to 0 to avoid negative values
            Die();
        }
    }

    // Method to handle character death
    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false); // Deactivate the character when dead
        Debug.Log("Player is dead.");
    }
}


