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
    [SerializeField] private LayerMask groundLayerMask;
    public SpriteRenderer sr;
    public Vector2 velocity;
    public Animator animator;
    public BoxCollider2D bc;
    public float offset;
    public int health = 6;
    public GameObject bullet;
    private float previousX;

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

        movementSpeed = 10;
        velocity = Vector2.zero;
        groundLayerMask = LayerMask.GetMask("Ground");
        gameObject.layer = 3;
    }

    public void FrameEvent(int inMoveX, int inMoveY, bool inShoot) {

        // Update previous position
        previousX = x;

        // Check if on ground
        onGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0.0f, Vector2.down, 0.01f, groundLayerMask);

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
        if (Physics2D.BoxCast(bc.bounds.center, bc.bounds.size/2, 0.0f, Vector2.right * direction, 10f, groundLayerMask))
        {
            x = previousX - (0.0025f * direction);
        }
        else if (!ducking && !shooting)
        {
            x = x + inMoveX;
        }

        // Gravity
        if (onGround)
        {
            velocity = Vector2.zero;
        }
        else
        {
            velocity.y += 0.1f;
            y += velocity.y;
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.M)) && onGround && !shooting)
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

        /*// Check if player is on ground
        onGround = (velocity.y == 0);

        // Simulate gravity
        inMoveY += 1;

        // Animations
        velocity = rb.velocity;

        if (health <= 0 && animator.GetInteger("state") != 6)
        {
            animator.SetInteger("state", 6);
        }

        if (health > 0)
        {
            if (shooting)
            {
                animator.SetInteger("state", 5);
            }
            else if (meleeing)
            {
                animator.SetInteger("state", 7);
            }
            else if (hiding)
            {
                animator.SetInteger("state", 8);
            }
            else if (Input.GetAxis("Vertical") < 0 && velocity.y == 0)
            {
                animator.SetInteger("state", 4);
            }
            else if (velocity.y == 0 && Input.GetAxis("Horizontal") == 0 && onGround)
            {
                animator.SetInteger("state", 0);
            }
            else if (velocity.y == 0 && Input.GetAxis("Horizontal") != 0 && onGround)
            {
                animator.SetInteger("state", 1);
            }
            else if (velocity.y > 0)
            {
                animator.SetInteger("state", 2);
            }
            else if (velocity.y < 0)
            {
                animator.SetInteger("state", 3);
            }

            

            // Move player
            if ((!(Input.GetAxis("Vertical") < 0) || velocity.y != 0) && animator.GetInteger("state") != 5 && !hiding)
            {
                //gameObject.transform.position += new Vector3(movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0);

            }

            // Jump
            if ((Input.GetAxis("Vertical") > 0 || Input.GetKeyDown(KeyCode.M)) && Input.GetAxis("Vertical") >= 0 && onGround && !shooting && !meleeing && !hiding)
            {
                //rb.velocity += Vector2.up * 130;
                //y = y + inMoveY - 100;
            }

            // Drop through platform
            if (Input.GetAxis("Vertical") < 0 && onGround && Input.GetKeyDown(KeyCode.M))
            {
                RaycastHit2D hit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0, Vector2.down, offset, groundLayerMask);
                if (hit.collider.tag == "Platform")
                {
                    //StartCoroutine(hit.collider.GetComponent<Platform>().DropThroughPlatform());
                }
            }

            // Shoot
            if (Input.GetKeyDown(KeyCode.N) && onGround && !shooting && !meleeing)
            {
                shooting = true;
                //StartCoroutine(AttackCooldown());

                // Create bullet
                //Bullet newBullet = Instantiate(bullet, transform.position + new Vector3(10.0f, 26.68f), Quaternion.identity, null).GetComponent<Bullet>();
                //newBullet.shooter = gameObject;
                //newBullet.facingRight = !sr.flipX;
            }
            else if (Input.GetMouseButtonDown(1) && onGround && !shooting && !meleeing)
            {
                meleeing = true;
                StartCoroutine(AttackCooldown());
            }

            // Come out of hiding
            if (Input.GetKeyUp(KeyCode.Space))
            {
                hiding = false;
                sr.sortingOrder = 4;
                animator.SetInteger("state", 0);
            }
        }*/

        UpdatePos();
    }

    void UpdatePos() 
    {
        playerPosition.x = x;
        playerPosition.y = -y;

        gameObject.transform.localPosition = playerPosition;
    }







}