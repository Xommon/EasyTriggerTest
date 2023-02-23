using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public Main main;
    public Rigidbody2D rb;
    public float movementSpeed;
    public bool onGround;
    [SerializeField] private LayerMask groundLayerMask;
    public SpriteRenderer sr;
    public Vector2 velocity;
    public Animator animator;
    public BoxCollider2D bc;
    public float offset;

    void Update()
    {
        // Check if player is on ground
        onGround = (velocity.y == 0);

        // Animations
        velocity = rb.velocity;

        if (Input.GetKey(KeyCode.S) && velocity.y == 0)
        {
            animator.SetInteger("state", 4);
        }
        else if (velocity.y == 0 && Input.GetAxis("Horizontal") == 0 && onGround)
        {
            animator.SetInteger("state", 0);
        }
        else if (velocity.y == 0 && Input.GetAxis("Horizontal") != 0 && onGround)
        {
            animator.SetInteger("state", 1);
        }
        else if (velocity.y > 0)
        {
            animator.SetInteger("state", 2);
        }
        else if (velocity.y < 0)
        {
            animator.SetInteger("state", 3);
        }

        // Flip character
        if (Input.GetAxis("Horizontal") > 0)
        {
            sr.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            sr.flipX = true;
        }

        // Move player
        if (!Input.GetKey(KeyCode.S) || velocity.y != 0)
        {
            rb.transform.position += new Vector3(movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && onGround)
        {
            rb.velocity += Vector2.up * 260;
        }

        // Drop through platform
        if (Input.GetKey(KeyCode.S) && onGround)
        {
            RaycastHit2D hit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, offset, groundLayerMask);
            if (hit.collider.tag == "Platform")
            {
                StartCoroutine(hit.collider.GetComponent<Platform>().DropThroughPlatform());
            }
        }
    }
}
