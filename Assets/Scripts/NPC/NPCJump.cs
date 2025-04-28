using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    [SerializeField]
    private float maxJumpDistance = 5f;

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

    [SerializeField]
    private Collider2D _jumpCollider;

    private NPCSlowdown _slowdown;

    private bool hasLanded = false;

    private void Awake()
    {
        _npcRigidbody = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
        _followingCommands= GetComponent<NPCFollow>();
        _slowdown= GetComponent<NPCSlowdown>();
    }

    // Update is called once per frame
    void Update()
    {

        _onGround = _collisionDataRetrieving.OnGround;

        if (!jumping)
        {
            return;
        }

        _desiredVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {


        if (!jumping)
        {
            return;
        }

        if (!_isJumping)
        {
            _isJumping = true;
            hasLanded = false;  // Reset land check when starting jump
            CaclulateJumpForce();
        }

        _currentVelocity = _npcRigidbody.velocity;

        // Detect landing
        if (_onGround && !_npcRigidbody.IsSleeping() && !hasLanded && Mathf.Abs(_currentVelocity.y) < 0.01f)
        {
            hasLanded = true;

            // Now that landed, start slowing down horizontally
            _slowdown.StartSlowingdown();
        }

        // After slowing down, once we have stopped moving horizontally, end the jump
        if (hasLanded && Mathf.Approximately(_currentVelocity.x, 0f))
        {
            StopJumping();
        }
    }


    private void CaclulateJumpForce()
    {
        //Debug.Log("Calcualting jump force");

        float gravity = Physics2D.gravity.magnitude;

        // Calculate the jump height considering gravity
        float jumpHeight = Mathf.Abs(player.position.y - transform.position.y);

        // Get the mass of the character
        float mass = _npcRigidbody.mass;

        // Calculate the force needed to reach the desired height
        float forceY = Mathf.Sqrt(2.1f * jumpHeight * gravity * mass);

        float jumpDistance = player.position.x - transform.position.x;

        float timeToTarget;

        // Set a default time to reach the target
        timeToTarget = 1f; // Change 1f to the default speed if needed

       // Debug.Log(jumpDistance);

        float v0x = jumpDistance / timeToTarget;

        //Debug.Log(v0x);

        //float launchAngle = Mathf.Rad2Deg * Mathf.Atan(v0y / v0x);
        float forceX = mass * v0x / timeToTarget;

        //_npcRigidbody.velocity = new Vector2(0, 0);

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

        //_slowdown.StopSlowingdown();
    }

    public bool CheckIfNeedToJump(Transform _player)
    {
        playerJump = _player.GetComponent<Jump>();

       // LogJumpVars(_player);

        //Y values are different and the player is not currently jumping OR the NPC is currently jumping
        if (jumping || 
            (_onGround && Mathf.Approximately(_npcRigidbody.velocity.y, 0) && 
            (_player.position.y - transform.position.y) > minYDifForJump && 
            (Mathf.Abs(_player.position.x - transform.position.x) < maxJumpDistance) && 
            !playerJump.IsJumping) )
        {
            return true;
        }

        jumpCompleted = false;
        return false;
    }

    /// <summary>
    /// TESTING
    /// </summary>
    private void LogJumpVars(Transform _player)
    {
        Debug.Log("OnGround: " + _onGround);
        Debug.Log("Y Dif: " + (_player.position.y - transform.position.y));
        Debug.Log("Player Jumping: " + (playerJump.IsJumping));
        Debug.Log("Jump Completed: " + (jumpCompleted));
    }

}
