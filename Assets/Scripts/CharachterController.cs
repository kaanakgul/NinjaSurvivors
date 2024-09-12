using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    const float InputThreshold = 0f;

    const string UpKey = "WalkUp";
    const string DownKey = "WalkDown";
    const string RightKey = "WalkRight";
    const string LeftKey = "WalkLeft";
    const string IdleKey = "Idle";

    string currentAnimation, newAnimation;

    public Animator animator;

    public Transform up, down, right, left;

    enum Direction { Left, Right, Up, Down, LeftDown, LeftUp, RightDown, RightUp };

    public LayerMask obstacleMask, doorMask;


    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        Animate();
    }

    void Move()
    {
        bool right = Input.GetKey(KeyCode.D);
        bool left = Input.GetKey(KeyCode.A);
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);

        if (right && left)
        {
            right = false;
            left = false;
        }

        if (up && down)
        {
            up = false;
            down = false;
        }

        Vector3 movement = Vector3.zero;


        if (up)
        {
            if (IsObstacleAhaed(Direction.Up)) return;

            ChangeAnimation(UpKey);

            movement += Vector3.up * speed * Time.deltaTime;
        }
        if (down)
        {
            if (IsObstacleAhaed(Direction.Down)) return;

            ChangeAnimation(DownKey);

            movement += Vector3.down * speed * Time.deltaTime;
        }
        if (right)
        {
            if (IsObstacleAhaed(Direction.Right)) return;

            ChangeAnimation(RightKey);

            movement += Vector3.right * speed * Time.deltaTime;
        }
        if (left)
        {
            if (IsObstacleAhaed(Direction.Left)) return;

            ChangeAnimation(LeftKey);

            movement += Vector3.left * speed * Time.deltaTime;
        }

        if (!down && !up && !right && !left)
        {
            ChangeAnimation(IdleKey);
        }

        transform.position += movement;
    }

    void ChangeAnimation(string animationName)
    {
        newAnimation = animationName;
    }

    void Animate()
    {
        if (newAnimation == currentAnimation) return;


        currentAnimation = newAnimation;

        animator.SetTrigger(currentAnimation);

    }

    bool IsObstacleAhaed(Direction dir)
    {
        Transform check = up;

        if (dir == Direction.Left)
        {
            check = left;
        }
        else if (dir == Direction.Right)
        {
            check = right;
        }
        else if (dir == Direction.Up)
        {
            check = up;
        }
        else { check = down; }

      
        Collider2D cols = Physics2D.OverlapBox(check.position, Vector3.one / 5f, 0, obstacleMask);



        if (cols == null)
        {
            return false;
        }



        return true;
    }

  
    
}


