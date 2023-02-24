using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject shooter;
    public bool facingRight;

    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    private void Update()
    {
        if (facingRight)
        {
            transform.position += new Vector3(10, 0);
        }
        else
        {
            transform.position += new Vector3(-10, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
}
