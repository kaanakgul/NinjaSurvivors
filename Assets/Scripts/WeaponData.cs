using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public GameObject prefab; // The weapon prefab
    public float damageAmount = 50f; // Damage dealt by the weapon
    public float cooldown = 0.5f; // Cooldown duration for this weapon
    public bool isRanged; // Indicates if the weapon is ranged
    public float Speed = 1.0f;
}
