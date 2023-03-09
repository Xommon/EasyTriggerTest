using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GeneralObject
{

    GameObject gameObject;
    Animator animator;
    BoxCollider2D bc;
    int health = 3;
    LayerMask bulletLayerMask;
    Player player;
    int cooldown = 0;
    int targetCooldown = 0;

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
        bulletLayerMask = LayerMask.GetMask("Bullet");
    }



    public override bool FrameEvent() 
    {
        // Death
        if (health <= 0)
        {
            animator.SetInteger("state", 6);
            Kill();
            return isOK;
        }

        // Increase cooldown
        cooldown++;

        // Hit by bullet
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.zero, 0.0f, bulletLayerMask))
        {
            health--;
            Debug.Log("Enemy hit");
        }

        // Phases and Behaviours
        float distanceFromPlayer = Vector2.Distance(new Vector2(x, y), new Vector2(player.x, player.y));
        if (targetCooldown > 0 && cooldown - targetCooldown >= 10)
        {
            cooldown = 0;
            targetCooldown = 0;
        }

        if (animator.GetInteger("state") == 5 && cooldown - targetCooldown < 10)
        {
            x += 0;
        }
        else if (distanceFromPlayer <= 120 && Mathf.Abs(player.y - y) < 10 && cooldown > 250)
        {
            // Combat
            SetDirection((int)((player.x - x) / Mathf.Abs(player.x - x)));
            Shoot();
            x += 0;
        }
        else
        {
            // Patrol
            animator.SetInteger("state", 1);
            x = x + 0.8f * direction;
            if ((direction == 1 && x > 600) || (direction == -1 && x < 480))
            {
                SetDirection(-direction);
            }
        }

        UpdatePos();


        return isOK;
    }

    public void Shoot()
    {
        if (animator.GetInteger("state") < 5)
        {
            game.AddLevelObject(new Bullet(main, (int)(x + (16 * direction)), (int)(y - 27), false, direction));
            targetCooldown = cooldown;
            snd.PlayAudioClip("Gun", false);
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



    public override void Kill() 
    {
        animator.SetInteger("state", 6);
    }






}