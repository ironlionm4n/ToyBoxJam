using System;
using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator playerAnimator;

    [Header("Running")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxXVelocity = 12f;
    [SerializeField] private float moveHorizontal;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float fallForce = 5f;
    [SerializeField] private float moveVertical;

    [Header("Dashing")]
    [SerializeField] private float dashWindow = 0.5f;
    [SerializeField] private int numInputs = 0;
    [SerializeField] private bool keyPressed = false;
    [SerializeField] private bool dashAttemptInitiated = false;
    [SerializeField] private float inputTimer = 0f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] float dashDirection = 0;

    private Vector2 _moveDirection = Vector2.zero;
    private bool _shouldJump;

    private void Awake()
    {
        playerRigidbody.gravityScale = fallForce;
    }

    private void Update()
    {
        if(numInputs == 2)
        {
            playerAnimator.SetBool("Dashing", true);

            playerRigidbody.AddForce(new Vector2(dashDirection * dashSpeed, 0f), ForceMode2D.Impulse);

            dashAttemptInitiated = false;
            inputTimer = 0;
            numInputs = 0;

            dashing = true;
            dashTimer = dashCooldown;
            StartCoroutine(Dashing());
        }

        //Manages dash cooldown
        if (dashing)
        {
            dashTimer -= Time.deltaTime;

            if(dashTimer <= 0)
            {
                dashing = false;
            }

        }

        //Timer that runs and resets the number of inputs after the dash window passes
        if (dashAttemptInitiated)
        {
            inputTimer += Time.deltaTime;
        }

        if(inputTimer >= dashWindow)
        {
            numInputs = 0;
            inputTimer = 0;
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Sets animator values
        playerAnimator.SetFloat("HorizontalMovement", moveHorizontal);
        playerAnimator.SetFloat("VerticalMovement", playerRigidbody.velocity.y);

        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true;

            //Makes sure the dash is not on cooldown
            if (dashTimer <= 0)
            {
                keyPressed = true;
                dashDirection = -1;
                dashAttemptInitiated = true;
            }
        }
        if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false;

            //Makes sure the dash is not on cooldown
            if (dashTimer <= 0)
            {
                keyPressed = true;
                dashDirection = 1;
                dashAttemptInitiated = true;
            }
        }
        
        //Adds 1 to the number of inputs once the player is done moving in a direction
        if(keyPressed && moveHorizontal == 0)
        {
            numInputs++;
            keyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnimator.SetBool("Jumping", true);
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (moveHorizontal > 0.1f)
        {
            if (playerRigidbody.velocity.x < maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
            }
        }

        if (moveHorizontal < -0.1f)
        {
            if (playerRigidbody.velocity.x > -maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
            }
        }

        if (_shouldJump && !isJumping) Jump();
        
    }

    private void Jump()
    {
        playerRigidbody.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        _shouldJump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            playerAnimator.SetBool("Jumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }

    public IEnumerator Dashing()
    {
        yield return new WaitWhile(() => !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dashing"));

        yield return new WaitWhile(() => playerAnimator.GetCurrentAnimatorStateInfo(0).length > playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        playerAnimator.SetBool("Dashing", false);
    }
}
