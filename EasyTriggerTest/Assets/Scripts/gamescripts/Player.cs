using UnityEngine;

public class Player {

    Main main;
    Game game;
    Gfx  gfx;
    Snd  snd;

    Sprite[] sprites;

    GameObject gameObject;
    Vector3 playerPosition;

    public float x, y;

    public Rigidbody2D rb;
    public float movementSpeed;
    private bool onGround, shooting, ducking, meleeing, hiding;
    private LayerMask groundLayerMask, bulletLayerMask;
    public SpriteRenderer sr;
    public Vector2 velocity;
    public Animator animator;
    public BoxCollider2D bc;
    public float offset;
    public int health = 600000;
    public GameObject bullet;
    private float previousX, previousY;
    private Healthbar healthbar;

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

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Animations/Player") as RuntimeAnimatorController;
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.offset = new Vector2(-0.536037445f, 18.5f);
        bc.size = new Vector2(15.7777481f, 37);
        sr = gameObject.GetComponent<SpriteRenderer>();
        healthbar = UnityEngine.GameObject.FindObjectOfType<Healthbar>();
        healthbar.player = this;

        movementSpeed = 10;
        velocity = Vector2.zero;
        groundLayerMask = LayerMask.GetMask("Ground");
        gameObject.layer = 3;
        bulletLayerMask = LayerMask.GetMask("Bullet");
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot)
    {
        // Check if on ground
        onGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.down, 0.1f, groundLayerMask);
        Debug.Log(velocity.y);
        /*Debug.DrawRay(gameObject.transform.position, Vector2.down * 50, Color.yellow);
        if (Physics2D.Raycast(gameObject.transform.position, Vector2.down * 50, 0, groundLayerMask, 0, 0))
        {
            onGround = true;
        }*/

        // Gravity
        if (onGround)
        {
            velocity = Vector2.zero;
        }
        else
        {
            if (velocity.y < 3.2f)
            {
                velocity.y += 0.1f;
            }
            y += velocity.y;
        }

        // Update previous position
        previousX = x;
        previousY = y;

        // Dead
        if (health <= 0)
        {
            animator.SetInteger("state", 6);
            return;
        }

        // Land
        if (animator.GetInteger("state") == 3 && onGround)
        {
            snd.PlayAudioClip("Land", false);
        }

        // Ducking
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && onGround)
        {
            ducking = true;
        }
        else
        {
            ducking = false;
        }

        // Flip character
        if (Input.GetAxis("Horizontal") > 0 && !shooting && !meleeing && !hiding)
        {
            sr.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0 && !shooting && !meleeing && !hiding)
        {
            sr.flipX = true;
        }

        // Find direction
        int direction;
        if (sr.flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        // Animations
        if (shooting)
        {
            animator.SetInteger("state", 5);
        }
        else if (ducking)
        {
            animator.SetInteger("state", 4);
        }
        else if (inMoveX == 0 && onGround)
        {
            animator.SetInteger("state", 0);
        }
        else if (inMoveX != 0 && onGround)
        {
            animator.SetInteger("state", 1);
        }
        else if (velocity.y < 0 && !onGround)
        {
            animator.SetInteger("state", 2);
        }
        else if (velocity.y > 0 && !onGround)
        {
            animator.SetInteger("state", 3);
        }

        // Check side collisions
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size * 0.92f, 0.0f, Vector2.right * direction, 10f, groundLayerMask))
        {
            x = previousX - (0.0025f * direction);
        }
        else if (!ducking && !shooting)
        {
            x = x + inMoveX;
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.M)) && onGround && !shooting && !ducking)
        {
            velocity.y = -3.75f;
            y += velocity.y;
            snd.PlayAudioClip("Jump", false);
        }

        // Shoot
        if (inShoot && onGround && !ducking && !shooting)
        {
            shooting = true;
            snd.PlayAudioClip("Gun", true);

            // Create bullet
            game.AddLevelObject(new Bullet(main, (int)(x + (16 * direction)), (int)(y - 27), true, direction));
        }

        // Shooting cooldown
        if (shooting && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1))
        {
            shooting = false;
        }

        // Hit by bullet
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.zero, 0.0f, bulletLayerMask))
        {
            if (ducking)
            {
                health--;
            }
            else
            {
                health -= 2;
            }
        }

        UpdatePos();
    }

    void UpdatePos() 
    {
        playerPosition.x = x;
        playerPosition.y = -y;

        gameObject.transform.localPosition = playerPosition;
    }







}