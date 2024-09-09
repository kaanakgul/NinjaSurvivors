using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterController : MonoBehaviour
{
    public float speed = 12.0f;
    private Rigidbody2D rb;
    private Vector2 dir;




    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        rb.velocity = dir * speed;   
    }



}
