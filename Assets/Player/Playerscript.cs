using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerscript : MonoBehaviour
{
    // Input Actions
    InputAction moveAction;
    InputAction jumpaction;
    InputAction crouchAction;
    InputAction attackAction;

    // Components
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator anim;
    HelperScript helper;

    // Ground
    public LayerMask groundLayerMask;
    bool isGrounded;

    // Movement
    float acceleration = 10f;
    float maxSpeed = 4f;

    // Grenade
    public GameObject Grenade;
    float grenadeSpeed = 15f;


    void Start()
    {
        // Get Input Actions
        moveAction = InputSystem.actions.FindAction("Move");
        jumpaction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        attackAction = InputSystem.actions.FindAction("Attack");

        // Get Components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Ground Layer
        isGrounded = false;
        groundLayerMask = LayerMask.GetMask("Ground");

        // Add HelperScript
        helper = gameObject.AddComponent<HelperScript>();
    }


    void Update()
    {
        CheckGround();

        Move();

        HandleJump();

        HandleAnimations();

        HandleHelper();

        Shoot();
    }


    void CheckGround()
    {
        bool leftRay = RayCollisionCheck(-0.2f, 0);
        bool middleRay = RayCollisionCheck(0, 0);
        bool rightRay = RayCollisionCheck(0.2f, 0);

        isGrounded = leftRay || middleRay || rightRay;
    }


    void Move()
    {
        Vector2 moveVel = moveAction.ReadValue<Vector2>();

        // Face right
        if (moveVel.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Face left
        else if (moveVel.x < 0)
        {
            spriteRenderer.flipX = true;
        }


        // Move right
        if (moveVel.x > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x + acceleration * Time.deltaTime,
                rb.linearVelocity.y
            );

            if (rb.linearVelocity.x > maxSpeed)
            {
                rb.linearVelocity = new Vector2(
                    maxSpeed,
                    rb.linearVelocity.y
                );
            }
        }


        // Move left
        if (moveVel.x < 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x - acceleration * Time.deltaTime,
                rb.linearVelocity.y
            );

            if (rb.linearVelocity.x < -maxSpeed)
            {
                rb.linearVelocity = new Vector2(
                    -maxSpeed,
                    rb.linearVelocity.y
                );
            }
        }
    }


    void HandleJump()
    {
        if (jumpaction.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }
    }


    void HandleAnimations()
    {
        // Walking animation
        if (rb.linearVelocityX != 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }


        // Crouching animation
        if (crouchAction.IsPressed())
        {
            anim.SetBool("crouch", true);
        }
        else
        {
            anim.SetBool("crouch", false);
        }
    }


    void HandleHelper()
    {
        // F = Flip sprite
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            helper.FlipSprite(true);
        }


        // P = Destroy player
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            helper.DestroyObject();
        }
    }


    void Shoot()
    {
        // Attack button pressed
        if (attackAction.WasPressedThisFrame())
        {
            // Create grenade
            GameObject clone = Instantiate(
                Grenade,
                transform.position,
                Quaternion.identity
            );

            // Get grenade Rigidbody2D
            Rigidbody2D grenadeRb = clone.GetComponent<Rigidbody2D>();


            // Grenade position
            clone.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 1f,
                transform.position.z
            );


            // Shoot right
            if (spriteRenderer.flipX == false)
            {
                grenadeRb.linearVelocity = new Vector2(
                    grenadeSpeed,
                    0
                );
            }

            // Shoot left
            else
            {
                grenadeRb.linearVelocity = new Vector2(
                    -grenadeSpeed,
                    0
                );
            }
        }
    }


    public bool RayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;

        bool hitSomething = false;

        Vector3 offset = new Vector3(
            xoffs,
            yoffs,
            0
        );


        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + offset,
            Vector2.down,
            rayLength,
            groundLayerMask
        );


        Color hitColor = Color.red;


        if (hit.collider != null)
        {
            print("Player has collided with Ground layer");

            hitColor = Color.green;
            hitSomething = true;
        }


        Debug.DrawRay(
            transform.position + offset,
            Vector2.down * rayLength,
            hitColor
        );


        return hitSomething;
    }


    void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            7
        );

        isGrounded = false;
    }
}



