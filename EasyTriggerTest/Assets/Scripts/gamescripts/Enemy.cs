using UnityEngine;

public class Enemy : GeneralObject {

    GameObject gameObject;
    Animator animator;
    int phase = 1; // 0 = idle, 1 = walking, 2 = shooting


    public Enemy(Main inMain, int inX, int inY) {

        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Enemies/Enemy3_2");

        gameObject = gfx.MakeGameObject("Enemy", sprites[22], x, y);

        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animations/Enemy") as RuntimeAnimatorController;

        SetDirection(-1);

    }



    public override bool FrameEvent() {


        // enemy logic here
        animator.SetInteger("state", phase);
        
        //if ()

        // temp logic :)
        //------------------------------------------------------------
        if (phase == 0)
        {
            // Idle

        }
        else if (phase == 1)
        {
            // Walk
            x = x + 0.8f * direction;
            if ((direction == 1 && x > 600) || (direction == -1 && x < 480))
            {
                SetDirection(-direction);
            }
        }
        else if (phase == 2)
        {
            // Shoot

        }
        //------------------------------------------------------------



        UpdatePos();


        return isOK;

    }


    void UpdatePos() {

        gfx.SetPos(gameObject, x, y);

    }



    void SetDirection(int inDirection) {

        direction = inDirection;
        gfx.SetDirX(gameObject, direction);

    }



    public override void Kill() {
       
    }






}