using System;
using System.Collections;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource playerJump;
    [SerializeField] private AudioSource playerLand;
    [SerializeField] private AudioSource playerRun;

    [Header("Running")]
    [Range(0f, 1f)]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxXVelocity = 12f;
    [SerializeField] private float moveHorizontal;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float fallForce = 5f;
    [SerializeField] private float moveVertical;

    [Header("Dashing")]
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] float dashDirection = 0;

    private bool _shouldJump;

    private void Awake()
    {
        playerRigidbody.gravityScale = fallForce;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing)
        {
            playerAnimator.SetBool("Dashing", true);

            playerRigidbody.AddForce(new Vector2(dashDirection * dashSpeed, 0f), ForceMode2D.Impulse);

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

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Sets animator values
        playerAnimator.SetFloat("HorizontalMovement", moveHorizontal);
        playerAnimator.SetFloat("VerticalMovement", playerRigidbody.velocity.y);

        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true;
            dashDirection = -1;
        }
        if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false;
            dashDirection= 1;
        }
      

        if (moveVertical > 0 && !isJumping)
        {
            playerAnimator.SetBool("Jumping", true);
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if(moveHorizontal == 0 && playerRun.isPlaying)
        {
            playerRun.Stop();
        }

        if (moveHorizontal > 0.1f)
        {
            if (playerRigidbody.velocity.x < maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed , 0f), ForceMode2D.Impulse);
            }

            if (!playerRun.isPlaying && playerRigidbody.velocity.y == 0)
            {
                playerRun.Play();
            }
        }

        if (moveHorizontal < -0.1f)
        {
            if (playerRigidbody.velocity.x > -maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
                //cameraPos.x = Mathf.Clamp(cameraPos.x, minPos.x, maxPos.x);
            }

            if (!playerRun.isPlaying && playerRigidbody.velocity.y == 0)
            {
                playerRun.Play();
            }
        }

        if (_shouldJump && !isJumping) Jump();
        
        
    }

    private void Jump()
    {
        playerRun.Stop();
        playerJump.Play();
        playerRigidbody.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        _shouldJump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && playerRigidbody.velocity.y <= 0)
        {
            playerRun.Stop();
            isJumping = false;
            playerAnimator.SetBool("Jumping", false);
            playerLand.Play();
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
