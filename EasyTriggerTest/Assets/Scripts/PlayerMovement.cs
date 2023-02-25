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
    public int health = 10;
    private bool shooting;
    public GameObject bullet;
    private bool meleeing;
    private bool hiding;

    void Update()
    {
        // Check if player is on ground
        onGround = (velocity.y == 0);

        // Animations
        velocity = rb.velocity;

        if (health <= 0 && animator.GetInteger("state") != 6)
        {
            animator.SetInteger("state", 6);
        }

        if (health > 0)
        {
            if (shooting)
            {
                animator.SetInteger("state", 5);
            }
            else if (meleeing)
            {
                animator.SetInteger("state", 7);
            }
            else if (hiding)
            {
                animator.SetInteger("state", 8);
            }
            else if (Input.GetKey(KeyCode.S) && velocity.y == 0)
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
            if (Input.GetAxis("Horizontal") > 0 && !shooting && !meleeing && !hiding)
            {
                sr.flipX = false;
            }
            else if (Input.GetAxis("Horizontal") < 0 && !shooting && !meleeing && !hiding)
            {
                sr.flipX = true;
            }

            // Move player
            if ((!Input.GetKey(KeyCode.S) || velocity.y != 0) && animator.GetInteger("state") != 5 && !hiding)
            {
                rb.transform.position += new Vector3(movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0);
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.W) && onGround && !shooting && !meleeing && !hiding)
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

            // Shoot
            if (Input.GetMouseButtonDown(0) && onGround && !shooting && !meleeing)
            {
                shooting = true;
                StartCoroutine(AttackCooldown());

                // Create bullet
                Bullet newBullet = Instantiate(bullet, transform.position + new Vector3(10.0f, 26.68f), Quaternion.identity, null).GetComponent<Bullet>();
                newBullet.shooter = gameObject;
                newBullet.facingRight = !sr.flipX;
            }
            /*else if (Input.GetMouseButtonDown(1) && onGround && !shooting && !meleeing)
            {
                meleeing = true;
                StartCoroutine(AttackCooldown());
            }*/

            // Come out of hiding
            if (Input.GetKeyUp(KeyCode.Space))
            {
                hiding = false;
                sr.sortingOrder = 4;
                animator.SetInteger("state", 0);
            }
        }

        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            meleeing = false;
            shooting = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HideZone" && Input.GetKeyDown(KeyCode.Space))
        {
            // Hide
            hiding = true;
            sr.sortingOrder = 2;
        }
    }
}
