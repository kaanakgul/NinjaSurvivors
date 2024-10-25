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
        public float attackSpeed;     // How often the enemy attacks


        // Reward ranges
        public int minGold = 1;
        public int maxGold = 5;
        public int minExp = 10;
        public int maxExp = 20;


}
