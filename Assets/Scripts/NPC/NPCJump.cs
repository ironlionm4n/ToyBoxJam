using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCJump : MonoBehaviour
{
    [Header("Jump Controls")]
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

    [SerializeField]
    private float minYDifForJump = 3f;

    private Rigidbody2D _npcRigidbody;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private Vector2 _velocity;
    private int _jumpPhase;
    private float _defaultGravityScale;
    private float _jumpSpeed;
    private float _coyoteCounter;
    private float _jumpBufferCounter;
    private bool _tryingToJump;
    [SerializeField]
    private bool _onGround;
    private bool _isJumping = false;

    private Transform player;
    private Jump playerJump;
    private float _maxSpeedChange;
    private float _acceleration;

    private bool jumpCompleted = false;

    [SerializeField]
    private bool jumping = false;

    private NPCFollow _followingCommands;
    private Vector2 _currentVelocity;
    Vector2 _desiredVelocity;

    private void Awake()
    {
        _npcRigidbody = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
        _followingCommands= GetComponent<NPCFollow>();
    }

    // Update is called once per frame
    void Update()
    {


        if (!jumping)
        {
            return;
        }

        _desiredVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        _onGround = _collisionDataRetrieving.OnGround;

        if (!jumping)
        {
            return;
        }

        if (!_isJumping)
        {
            _isJumping = true;
            CaclulateJumpForce();
        }

        //Since we are jumping with force we need to slow the AI down manually after they land
        if (_onGround && _npcRigidbody.velocity.y == 0)
        {
            _onGround = _collisionDataRetrieving.OnGround;
            _currentVelocity = _npcRigidbody.velocity;

            _acceleration = _onGround ? _followingCommands.MaxSlowdownAcc : _followingCommands.MaxAirAcc;

            _maxSpeedChange = _acceleration * Time.deltaTime;

            _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);
            _npcRigidbody.velocity = _currentVelocity;

            if (_currentVelocity.x == 0)
            {
                StopJumping();
            }
        }
    }


    private void CaclulateJumpForce()
    {
        Debug.Log("Calcualting jump force");

        float gravity = Physics2D.gravity.magnitude;

        // Calculate the jump height considering gravity
        float jumpHeight = Mathf.Abs(player.position.y - transform.position.y);

        // Get the mass of the character
        float mass = _npcRigidbody.mass;

        // Calculate the force needed to reach the desired height
        float forceY = Mathf.Sqrt(2.1f * jumpHeight * gravity * mass);

        float jumpDistance = player.position.x - transform.position.x;

        float timeToTarget;

        // Check if the horizontal velocity is zero or close to zero
        if (Mathf.Approximately(_npcRigidbody.velocity.x, 0f))
        {
            // Set a default time to reach the target
            timeToTarget =  1f; // Change 1f to the default speed if needed
        }
        else
        {
            timeToTarget = Mathf.Abs(jumpDistance) / Mathf.Abs(_npcRigidbody.velocity.x);
        }

       // Debug.Log(jumpDistance);

        float v0x = jumpDistance / timeToTarget;

        //Debug.Log(v0x);

        //float launchAngle = Mathf.Rad2Deg * Mathf.Atan(v0y / v0x);
        float forceX = mass * v0x / timeToTarget;

        _npcRigidbody.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

    private void CheckForGround()
    {

    }

    public void StartJumping(Transform _player)
    {
        player = _player;
        playerJump = _player.GetComponent<Jump>();
        jumping = true;
        _isJumping = false;
    }

    public void StopJumping()
    {
        jumping = false;
        _isJumping = false;
        jumpCompleted = true;
    }

    public bool CheckIfNeedToJump(Transform _player)
    {
        playerJump = _player.GetComponent<Jump>();

        //Y values are different and the player is not currently jumping OR the NPC is currently jumping
        if (jumping || (_onGround && (_player.position.y - transform.position.y) > minYDifForJump  && !playerJump.IsJumping) && !jumpCompleted)
        {
            return true;
        }

        jumpCompleted = false;
        return false;
    }

}
