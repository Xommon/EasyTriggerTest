using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int health;
    public Animator animator;
    public Rigidbody2D rb;
    public bool dead;

    void Start()
    {
        
    }

    void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            animator.SetInteger("state", 6);
            Destroy(gameObject, 3.5f);
        }
        else if (health > 0)
        {
            if (rb.velocity.y > 0)
            {
                // Jumping
                animator.SetInteger("state", 2);
            }
            else if (rb.velocity.y < 0)
            {
                // Falling
                animator.SetInteger("state", 3);
            }
            else
            {
                // Idling
                animator.SetInteger("state", 0);
            }
        }
    }
}
