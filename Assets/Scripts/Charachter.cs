using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charachter : MonoBehaviour
{
    public float health = 100.0f;
    public bool isDead;

    private void FixedUpdate()
    {

        if (health == 0 && isDead == false)
        {
            gameObject.SetActive(false);
            isDead = true;

        }

    }
}

