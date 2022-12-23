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
    [SerializeField] private float maxYVelocity = 10f;
    [SerializeField] private float moveHorizontal;
    [SerializeField] private bool knockbacked = false;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float fallForce = 5f;
    [SerializeField] private float moveVertical;
    [SerializeField] private bool jumpQueued = false;
    [SerializeField] private float velocityY = 0f;

    [Header("Dashing")]
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] float dashDirection = 0;

    [Header("General Variables")]
    [SerializeField] private bool isDead = false;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
    private bool _shouldJump;

    private void Awake()
    {
        playerRigidbody.gravityScale = fallForce;
    }

    private void OnEnable()
    {
        isDead = false;
    }

    private void Update()
    {
        if (inCutscene) { return; }

        if (isDead)
        {
            if(playerRun.isPlaying)
            {
                playerRun.Stop();
            }

            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);

            return;
        }
        
        //Used for debugging
        velocityY = playerRigidbody.velocity.y;

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

        if (moveHorizontal < 0 && !knockbacked)
        {
            spriteRenderer.flipX = true;
            dashDirection = -1;
        }
        if (moveHorizontal > 0 && !knockbacked)
        {
            spriteRenderer.flipX = false;
            dashDirection= 1;
        }
      

        if (moveVertical > 0 && !isJumping && !jumpQueued)
        {
            if (playerRigidbody.velocity.y == 0f)
            {
                playerAnimator.SetBool("Jumping", true);
                _shouldJump = true;
            }
            else
            {
                jumpQueued = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(moveHorizontal == 0 && playerRun.isPlaying)
        {
            playerRun.Stop();
        }

        if (moveHorizontal > 0.1f && !knockbacked)
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

        if (moveHorizontal < -0.1f && !knockbacked)
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

        if(jumpQueued && playerRigidbody.velocity.y == 0)
        {
            jumpQueued = false;
            Jump();
        }
        
        
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
        if (collision.gameObject.tag == "Ground")
        {
            if(inCutscene) { return; }

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

    public void SetKnockbacked(bool kb)
    {
        knockbacked = kb;
    }

    //Used for after cutscene so player can jump
    public void SetIsJumping(bool value)
    {
        isJumping = value;
    }
    
}
