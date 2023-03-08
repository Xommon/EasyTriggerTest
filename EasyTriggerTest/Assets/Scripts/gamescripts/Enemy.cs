using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GeneralObject {

    GameObject gameObject;
    Animator animator;
    BoxCollider2D bc;
    int health = 3;
    LayerMask bulletLayerMask;
    Player player;
    bool shooting;


    public Enemy(Main inMain, int inX, int inY, Player _player) {

        SetGeneralVars(inMain, inX, inY);

        sprites = gfx.GetLevelSprites("Enemies/Enemy3_2");

        gameObject = gfx.MakeGameObject("Enemy", sprites[22], x, y);

        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animations/Enemy") as RuntimeAnimatorController;

        SetDirection(-1);

        bc = gameObject.AddComponent<BoxCollider2D>();

        player = _player;

        gameObject.layer = 3;
        bulletLayerMask = 7;
    }



    public override bool FrameEvent() 
    {
        // Show death
        if (health <= 0)
        {
            animator.SetInteger("state", 6);
            return isOK;
        }

        // Hit by bullet
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size / 2, 0.0f, Vector2.right * direction, 10f, bulletLayerMask))
        {
            health--;
            Debug.Log("Enemy hit");
        }

        // Phases and Behaviours
        float distanceFromPlayer = Vector2.Distance(new Vector2(x, y), new Vector2(player.x, player.y));
        if (distanceFromPlayer > 120)
        {
            // Patrol
            animator.SetInteger("state", 1);
            x = x + 0.8f * direction;
            if ((direction == 1 && x > 600) || (direction == -1 && x < 480))
            {
                SetDirection(-direction);
            }
        }
        else if (distanceFromPlayer <= 120 && distanceFromPlayer >= 100)
        {
            // Investigate
            animator.SetInteger("state", 0);
            SetDirection((int)((player.x - x) / Mathf.Abs(player.x - x)));
            x += 0;
        }
        else if (distanceFromPlayer < 100)
        {
            // Combat
            //animator.SetInteger("state", 0);

            SetDirection((int)((player.x - x) / Mathf.Abs(player.x - x)));
            x += 0;
            Shoot();
        }

        // Shooting cooldown
        if (shooting && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1))
        {
            shooting = false;
        }


        UpdatePos();


        return isOK;
    }

    void Shoot()
    {
        if (!shooting)
        {
            shooting = true;
            snd.PlayAudioClip("Gun", true);
            animator.SetInteger("state", 5);
        }
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