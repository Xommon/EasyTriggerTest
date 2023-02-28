using UnityEngine;

public class Player {

    Main main;
    Game game;
    Gfx  gfx;
    Snd  snd;

    Sprite[] sprites;

    GameObject gameObject;
    Vector3 playerPosition;

    float x;
    float y;

    public Player (Main inMain) {

        main = inMain;
        game = main.game;
        gfx  = main.gfx;
        snd  = main.snd;

        sprites = gfx.GetLevelSprites("Players/Player1");

        x = 370;
        y = 624;

        gameObject = gfx.MakeGameObject("Player", sprites[22], x, y,"Player");
        playerPosition = gameObject.transform.localPosition;
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot) {


        // Player logic here


        // temp logic :)
        //------------------------------------------------------------
        x = x + inMoveX;
        if (inMoveY==-1) { main.Trace("Jump!"); }
        if (inMoveY== 1) { main.Trace("Duck!"); }
        //------------------------------------------------------------



        UpdatePos();

        if (inShoot) {
            snd.PlayAudioClip("Gun");
        }

    }


    void UpdatePos() {

        playerPosition.x = x;
        playerPosition.y = -y;

        gameObject.transform.localPosition = playerPosition;
    }







}