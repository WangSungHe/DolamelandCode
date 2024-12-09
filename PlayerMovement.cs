using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb1;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask JumpableGround;
    private float dirx = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling }
    [SerializeField] private AudioSource jumpSoundEffect;

 
    private void Start()
    {
        rb1 = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = new Vector2(dirx * moveSpeed, rb1.velocity.y);
        rb1.velocity = velocity;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb1.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.H)) 
        {
            LoadStartScene(); 
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            LoadEndScene();
        }

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;
        if (dirx > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirx < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb1.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb1.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene("Start Screen"); 
    }
    private void LoadEndScene() 
    {
        SceneManager.LoadScene("End Screen");
    }

    private bool IsTouchingWall()
    {
        float rayLength = 0.1f;
        Vector2 direction = Vector2.right * Mathf.Sign(dirx);

        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, direction, rayLength, JumpableGround);


        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return true;
        }
        return false;
    }
}
