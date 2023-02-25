using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlayerMovement player;
    public BoxCollider2D bc;
    private bool manual;

    // Collisions
    public Collider2D playerCollider;
    
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        manual = false;
        playerCollider = player.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!manual)
        {
            if (player.rb.transform.position.y < transform.position.y && !Input.GetKeyDown(KeyCode.S))
            {
                Physics2D.IgnoreCollision(playerCollider, bc, true);
            }
            else
            {
                Physics2D.IgnoreCollision(playerCollider, bc, false);
            }
        }
    }

    public IEnumerator DropThroughPlatform()
    {
        manual = true;
        Physics2D.IgnoreCollision(playerCollider, bc, true);
        yield return new WaitForSeconds(1.0f);
        //Physics2D.IgnoreCollision(playerCollider, bc, false);
        manual = false;
    }
}
