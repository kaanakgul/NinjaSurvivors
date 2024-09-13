using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    [System.Serializable]
    public class EnemyData
    {
        public GameObject prefab;     // The enemy prefab to instantiate
        public float health;          // Health of the enemy
        public float speed;           // Speed of the enemy
        public float damage;          // Damage the enemy deals
    }
