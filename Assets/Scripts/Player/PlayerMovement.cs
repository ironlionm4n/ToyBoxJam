using System;
using System.Collections;
using Unity.VisualScripting;
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

    [SerializeField] private InputController inputController = null;
    [SerializeField][Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField][Range(0f, 100f)] private float maxGroundAcc = 35f;
    [SerializeField][Range(0f, 100f)] private float maxAirAcc = 20f;

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;

    [Header("Running")]
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

    [Header("Respawn Point")]
    [SerializeField] private GameObject currentRespawnPoint;

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
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
    }

    private void OnEnable()
    {
        isDead = false;
    }

    public float GetHorizontalMoveDirection()
    {
        return _currentVelocity.x;
    }

    private void Update()
    {
        if (inCutscene) { 
            
            playerAnimator.SetFloat("VerticalMovement", 0);
            
            return; 
        }

        if (isDead)
        {
            SFX.instance.StopRunning();

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

            SFX.instance.Dash();

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
            //groundChecker.enabled = true;
        }

        _direction.x = inputController.RetrieveMovementInput();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetrieving.Friction, 0f);

        //moveHorizontal = Input.GetAxisRaw("Horizontal");
       // moveVertical = Input.GetAxisRaw("Vertical");

        //Sets animator values
        //playerAnimator.SetFloat("VerticalMovement", playerRigidbody.velocity.y);

        if (_direction.x < 0 && !knockbacked)
        {
            spriteRenderer.flipX = true;
            dashDirection = -1;
        }
        if (_direction.x > 0 && !knockbacked)
        {
            spriteRenderer.flipX = false;
            dashDirection= 1;
        }

    }

    private void FixedUpdate()
    {
        if(_currentVelocity.x == 0)
        {
            SFX.instance.StopRunning();
        }

        if (knockbacked)
        {
            return;
        }


        _onGround = _collisionDataRetrieving.OnGround;
        _currentVelocity = playerRigidbody.velocity;
        _acceleration = _onGround ? maxGroundAcc : maxAirAcc;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);

        playerAnimator.SetFloat("HorizontalMovement", _currentVelocity.x);

        playerRigidbody.velocity = _currentVelocity;

        if (_currentVelocity.x != 0 && playerRigidbody.velocity.y == 0)
        {
            SFX.instance.Run();
        }


        /*if (moveHorizontal > 0.1f && !knockbacked)
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
        }*/
        
        
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

            GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("PlatformRespawnPoint");

            if (respawnPoints.Length > 0)
            {
                float smallestDistance = Vector3.Distance(transform.position, respawnPoints[0].transform.position);
                currentRespawnPoint = respawnPoints[0];

                foreach (GameObject currentPoint in respawnPoints)
                {
                    float distance = Vector3.Distance(transform.position, currentPoint.transform.position);

                    if(distance < smallestDistance)
                    {
                        currentRespawnPoint = currentPoint;
                        smallestDistance = distance;
                    }
                }
            }

            //playerRun.Stop();
            isJumping = false;
            playerAnimator.SetBool("Jumping", false);
            //playerLand.Play();
        }
        else if(collision.gameObject.tag == "AngelRespawn")
        {
            transform.position = currentRespawnPoint.transform.position;
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
        _currentVelocity.x = 0f;
        SFX.instance.StopSounds();
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
