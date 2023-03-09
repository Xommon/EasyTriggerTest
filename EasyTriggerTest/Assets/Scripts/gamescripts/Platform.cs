using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : GeneralObject
{
    BoxCollider2D bc;
    Player player;
    GameObject gameObject;
    int timer = 0;

    public Platform(Main inMain, float posX, float posY, float sizeX, float sizeY, int inX, int inY, Player _player)
    {
        SetGeneralVars(inMain, inX, inY);

        gameObject = gfx.MakeGameObject("Platform", null, x, y);

        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.offset = new Vector2(posX, posY);
        bc.size = new Vector2(sizeX, sizeY);

        player = _player;

        gameObject.layer = 6;
    }

    public override bool FrameEvent()
    {
        if (player.health <= 0)
        {
            bc.enabled = true;
            return isOK;
        }

        if (-player.y <= this.gameObject.transform.localPosition.y && timer >= 0)
        {
            timer = 0;
            bc.enabled = false;
        }
        else
        {
            timer++;
        }

        if (timer > 10)
        {
            bc.enabled = true;
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && (Input.GetKeyDown(KeyCode.W)) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.M))
        {
            bc.enabled = false;
            timer = -10;
        }

        return isOK;
    }
}
