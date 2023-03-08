using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GeneralObject
{

    GameObject gameObject;
    BoxCollider2D bc;
    bool shotByPlayer;
    LayerMask characterLayerMask;


    public Bullet(Main inMain, int inX, int inY, bool _shotByPlayer, int _direction)
    {

        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Players/Player1");

        gameObject = gfx.MakeGameObject("Bullet", sprites[36], x, y);
        gameObject.layer = 7;
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(5, 5);
        shotByPlayer = _shotByPlayer;
        characterLayerMask = 3;

        SetDirection(_direction);

        //UnityEngine.Object.Destroy(gameObject, 3.0f);
    }



    public override bool FrameEvent()
    {
        // Move
        x += 10f * direction;

        // Check for collisions
        RaycastHit2D hit = Physics2D.Raycast(bc.bounds.center, Vector2.right * direction, 7.0f);
        if (shotByPlayer && hit.collider.gameObject.tag == "Enemy")
        {
            //hit.collider.gameObject.GetComponent<Enemy>().health--;
            //Debug.Log("Hit: " + hit.collider.gameObject.name);
            //UnityEngine.GameObject.Destroy(gameObject);
        }
        //List<RaycastHit2D> results = new List<RaycastHit2D>();
        /*var results = 0;
        //if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size / 2, 0.0f, Vector2.right * direction, 10f, characterLayerMask))
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.right * direction, characterLayerMask, results, Mathf.Infinity))
        {
            Debug.Log(results);
            if (!shotByPlayer)
            {
                Debug.Log("Hit enemy!");
            }
        }*/

        UpdatePos();


        return isOK;
    }


    void UpdatePos()
    {
        gfx.SetPos(gameObject, x, y);
    }



    void SetDirection(int inDirection)
    {

        direction = inDirection;
        gfx.SetDirX(gameObject, direction);

    }



    public override void Kill()
    {

    }






}