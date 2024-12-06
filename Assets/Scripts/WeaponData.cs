using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // Unique name for the weapon
    public GameObject prefab; // The weapon prefab
    public float damageAmount = 50f; // Base damage dealt by the weapon
    public float cooldown = 0.5f; // Base cooldown duration
    public float Speed = 1.0f; // Projectile speed for ranged weapons
    public float rotationSpeed = 360f; // Rotation speed for projectiles (if applicable)
    public float range = 5f; //Attack Range for Melee weapons
    public float travelRange = 50f;  // Maximum distance the weapon can travel
}
