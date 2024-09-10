using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform targetDestination;
    [SerializeField] float speed;
    Rigidbody2D rb;
    public float damage;
    public float attackSpeed = 0.5f;
    protected bool attacking = false;
    private Coroutine attackroutine;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!attacking)
        {
            attackroutine = StartCoroutine(DealDamage(collision));
            attacking = true;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
      
        StopCoroutine(attackroutine);
        attacking=false;
        Debug.Log("asd");
        
    }

    public IEnumerator DealDamage(Collision2D collision)
    {
        while (collision.gameObject.GetComponent<Charachter>().isDead == false)
        {
            if (collision.gameObject.GetComponent<Charachter>())
            {
                targetDestination.GetComponent<Charachter>().health -= damage;
                Debug.Log("You Take " + damage + " Damage");
                Debug.Log("You have " + targetDestination.GetComponent<Charachter>().health + " health");
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }
   
}
