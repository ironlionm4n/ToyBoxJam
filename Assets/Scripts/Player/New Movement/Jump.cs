using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController inputController = null;

    [SerializeField, Range(0f, 100f), Tooltip("Max height of the jump")]
    private float jumpHeight = 3f;

    [SerializeField, Range(0f, 5f), Tooltip("Max number of jumps in air")]
    private int maxAirJumps = 0;
    public int MaxAirJumps { get { return maxAirJumps; } set { maxAirJumps = value; } }

    [SerializeField, Range(0f, 10f), Tooltip("Gravity scale effecting the player")]
    private float downwardMovementMultiplier = 3f;

    [SerializeField, Range(0f, 10f), Tooltip("How fast player will reach the peak of jump")]
    private float upwardMovementMultiplier = 1.7f;

    [SerializeField, Range(0f, .5f), Tooltip("How long after leaving ground you can perform a jump")]
    private float coyoteTime = .25f;

    [SerializeField, Range(0f, .5f), Tooltip("Detection window for jump input")]
    private float jumpBuffer = .25f;

    [Header("Player Animation Section")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField, Tooltip("Name of jump animation state in animator")] private string playerJump = "Player_Jump";

    [SerializeField] private bool canJump = true;
    //[SerializeField] private PlayerSFXManager playerSfxManager;

    private Rigidbody2D _playerRigidbody;
    private CollisionDataRetrieving _ground;
    private Vector2 _velocity;
    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _jumpSpeed;
    private float _coyoteCounter;
    private float _jumpBufferCounter;
    private bool _tryingToJump;
    private bool _onGround;
    private bool _isJumping;
    private bool _canFall;

    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<CollisionDataRetrieving>();
        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        if (!canJump) return;

        // Bitwise OR assignment operator, tryingToJump will remain set in new updates until manually changed
        _tryingToJump |= inputController.RetrieveJumpInput();

        var playerAnimStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (playerAnimStateInfo.IsName("Player_Idle") || playerAnimStateInfo.IsName("Player_Run"))
        {
            if (_playerRigidbody.velocity.y < 0 && _canFall)
            {
                _canFall = false;
                playerAnimator.SetFloat("VerticalMovement", _playerRigidbody.velocity.y);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!canJump) return;

        _onGround = _ground.OnGround;
        _velocity = _playerRigidbody.velocity;

        if (_onGround && _playerRigidbody.velocity.y == 0)
        {
            _jumpPhase = 0;
            _coyoteCounter = coyoteTime;
            _isJumping = false;
        }
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }

        if (_tryingToJump)
        {
            _tryingToJump = false;
            _jumpBufferCounter = jumpBuffer;
        }
        else if (!_tryingToJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }

        if (inputController.RetrieveJumpInputHeld() && _playerRigidbody.velocity.y > 0)
        {
            _playerRigidbody.gravityScale = upwardMovementMultiplier;
            //playerAnimator.ChangeAnimationState(AnimationState.JumpUp, playerJump);
        }
        else if (!inputController.RetrieveJumpInputHeld() || _playerRigidbody.velocity.y < 0)
        {
            _playerRigidbody.gravityScale = downwardMovementMultiplier;
        }
        else if (_playerRigidbody.velocity.y == 0)
        {
            _playerRigidbody.gravityScale = _defaultGravityScale;
        }

        _canFall = _playerRigidbody.velocity.y >= 0;
        _playerRigidbody.velocity = _velocity;
    }

    void JumpAction()
    {
        if (_coyoteCounter > 0f || (_jumpPhase < maxAirJumps && _isJumping))
        {
            if (_isJumping)
            {
                _jumpPhase++;
            }
            _jumpBufferCounter = 0f;
            _coyoteCounter = 0f;
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            _isJumping = true;
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_playerRigidbody.velocity.y);
            }

            _velocity.y += _jumpSpeed;
            playerAnimator.SetFloat("VerticalMovement", _playerRigidbody.velocity.y);
            playerAnimator.SetBool("Jumping", true);
            SFX.instance.Jump();
        }
    }

    public void EnableJumping()
    {
        canJump = true;
    }

    public void DisableJumping()
    {
        canJump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            SFX.instance.Land();
            playerAnimator.SetBool("Jumping", false);

        }
    }
}
