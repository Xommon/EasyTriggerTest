using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlayerMovement player;
    public BoxCollider2D bc;
    private bool manual;
    
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        manual = false;
    }

    void Update()
    {
        if (!manual)
        {
            if (player.rb.transform.position.y < transform.position.y && !Input.GetKeyDown(KeyCode.S))
            {
                bc.isTrigger = true;
            }
            else
            {
                bc.isTrigger = false;
            }
        }
    }

    public IEnumerator DropThroughPlatform()
    {
        manual = true;
        bc.isTrigger = true;
        yield return new WaitForSeconds(1.0f);
        manual = false;
    }
}
