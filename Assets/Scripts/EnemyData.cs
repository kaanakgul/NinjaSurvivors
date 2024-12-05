using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;     // The enemy prefab
    public float health;          // Health of the enemy
    public float speed;           // Movement speed
    public float damage;          // Damage dealt to the player
    public float attackSpeed;     // Attack cooldown duration

    // Rewards
    public int minGold = 10;      // Minimum gold reward
    public int maxGold = 50;     // Maximum gold reward
    public int minExp = 5;       // Minimum EXP reward
    public int maxExp = 20;       // Maximum EXP reward
}
