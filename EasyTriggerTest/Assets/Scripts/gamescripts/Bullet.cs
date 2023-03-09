using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GeneralObject
{

    GameObject gameObject;
    BoxCollider2D bc;
    bool shotByPlayer;
    LayerMask characterLayerMask;
    LayerMask groundLayerMask;

    public Bullet(Main inMain, int inX, int inY, bool _shotByPlayer, int _direction)
    {
        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Players/Player1");

        gameObject = gfx.MakeGameObject("Bullet", sprites[36], x, y);
        gameObject.layer = 7;
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(5, 5);
        shotByPlayer = _shotByPlayer;
        characterLayerMask = LayerMask.GetMask("Character");
        groundLayerMask = LayerMask.GetMask("Ground");

        SetDirection(_direction);

        GameObject.Destroy(gameObject, 4.0f);
    }

    public override bool FrameEvent()
    {
        // Move
        x += 5f * direction;

        // Check for collisions
        if (gameObject != null)
        {
            if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.zero, 0.0f, characterLayerMask))
            {
                GameObject.Destroy(gameObject);
            }
            else if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.zero, 0.0f, groundLayerMask))
            {
                GameObject.Destroy(gameObject);
            }
        }

        UpdatePos();

        return isOK;
    }

    void UpdatePos()
    {
        if (gameObject != null)
        {
            gfx.SetPos(gameObject, x, y);
        }
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