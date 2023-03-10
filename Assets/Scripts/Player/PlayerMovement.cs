using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource playerJump;
    [SerializeField] private AudioSource playerLand;
    [SerializeField] private AudioSource playerRun;
    [SerializeField] private LayerMask oneway;
    [SerializeField] private Collider2D groundChecker;

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
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashTime = 0f; 
    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] float dashDirection = 0;
    [SerializeField] private bool canDash = true;
    [SerializeField] private Image[] dashIndicators;
    [SerializeField] private AudioSource playerDashAudioSource;

    [Header("General Variables")]
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool canPlaySounds = true;

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

    public float GetHorizontalMoveDirection()
    {
        return moveHorizontal;
    }

    private void Update()
    {
        if (inCutscene) { 
            
            playerAnimator.SetFloat("VerticalMovement", 0);
            
            return; }

        if (isDead)
        {
            if(playerRun.isPlaying)
            {
                playerRun.Stop();
            }

            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
            return;
        }

        if(dashTime < dashDuration && dashing)
        {
            dashTime += Time.deltaTime;
        }
        
        //Used for debugging
        velocityY = playerRigidbody.velocity.y;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            canDash = false;
            playerDashAudioSource.Play();
            playerAnimator.SetTrigger("TriggerDash");
            playerAnimator.SetBool("Dashing", true);
            playerRigidbody.AddForce(new Vector2(dashDirection * dashSpeed, 0f), ForceMode2D.Impulse);
            dashing = true;
            dashTimer = dashCooldown;
            StartCoroutine(Dashing());
         
        }

        //Manages dash cooldown
        if (!dashing)
        {
            dashTimer -= Time.deltaTime;

            if(dashTimer <= 0)
            {
                canDash = true;
            }

        }

        if(playerRigidbody.velocity.y <= 0)
        {
            groundChecker.enabled = true;
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

        if (Input.GetKey(KeyCode.Space))
        {
            moveVertical = 1;
        }

        if(moveVertical < 0)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, -transform.up, 0.75f);

            foreach (RaycastHit2D item in hit)
            {

                if (item && item.transform != transform)
                {
                    if (item.transform.GetComponent<PlatformEffector2D>() != null)
                    {
                        Debug.Log("Hit");
                        PlatformEffector2D hitEffector = item.transform.GetComponent<PlatformEffector2D>();
                        item.transform.GetComponent<OneWay>().FallThrough();
                    }
                }
            }
        }      

        if ((moveVertical > 0 && !isJumping && !jumpQueued))
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

    public IEnumerator GoDown(PlatformEffector2D pe)
    {
        pe.surfaceArc = 0;

        yield return new WaitForSeconds(0.2f);

        pe.surfaceArc = 180;
    }

    private void Jump()
    {
        playerRun.Stop();
        playerJump.Play();
        groundChecker.enabled = false;

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
        
        StartCoroutine(Flash());

        spriteRenderer.color = Color.yellow;

        // Makes player invincible while dashing
        gameObject.GetComponent<PlayerStats>().SetInvicible(true);

        yield return new WaitWhile(() => dashTime < dashDuration);

        dashing = false;
        dashTime = 0;
        spriteRenderer.color = Color.white;
        gameObject.GetComponent<PlayerStats>().SetInvicible(false);
        

        StartCoroutine(DashIndicator());
    }

    //Controls dash indicators showing when player can dash again
    public IEnumerator DashIndicator()
    {
        playerAnimator.SetBool("Dashing", false);
        for(int i = 0; i < dashIndicators.Length; i++)
        {
            dashIndicators[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(dashCooldown / dashIndicators.Length);
        }


        for (int i = 0; i < dashIndicators.Length; i++)
        {
            dashIndicators[i].gameObject.SetActive(false);
        }
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

    public void StopSounds()
    {
        moveHorizontal = 0;
        playerRun.Stop();
        playerJump.Stop();
        playerLand.Stop();
    }

    public void Respawned()
    {
        isDead= false;
    }

    public IEnumerator Flash()
    {
        while (dashing)
        {
            spriteRenderer.enabled= false;

            yield return new WaitForSeconds(0.05f);

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(0.05f);
        }

        //Makes sure the rendered is enabled after dashing
        spriteRenderer.enabled = true;
    }
    
}
