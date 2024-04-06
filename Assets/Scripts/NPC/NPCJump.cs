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
    private CollisionDataRetrieving _ground;
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

    private NPCBrain brain;

    [SerializeField]
    private bool jumping = false;

    StateObject jumpingState;
    private void Awake()
    {
        _npcRigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<CollisionDataRetrieving>();
    }

    // Update is called once per frame
    void Update()
    {


        if (!jumping)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;

        if (!jumping)
        {
            return;
        }

        Debug.Log(_npcRigidbody.velocity.ToString());

        if (!_isJumping)
        {
            _isJumping = true;
            CaclulateJumpForce();
        }

        if (_onGround && _npcRigidbody.velocity.y == 0)
        {
            StopJumping();
        }
    }


    private void CaclulateJumpForce()
    {

        float gravity = Physics2D.gravity.magnitude;

        // Calculate the jump height considering gravity
        float jumpHeight = Mathf.Abs(player.position.y - transform.position.y);

        // Get the mass of the character
        float mass = _npcRigidbody.mass;

        // Calculate the force needed to reach the desired height
        float forceY = Mathf.Sqrt(2.1f * jumpHeight * gravity * mass);

        _npcRigidbody.AddForce(new Vector2(0, forceY), ForceMode2D.Impulse);
    }

    private void CheckForGround()
    {

    }

    public void StartJumping(Transform _player)
    {
        player = _player;
        playerJump = _player.GetComponent<Jump>();
        jumping = true;
    }

    public void StopJumping()
    {
        jumping = false;
        _isJumping = false;
    }

    public bool CheckIfNeedToJump(Transform _player)
    {
        playerJump = _player.GetComponent<Jump>();
        Debug.Log(Mathf.Abs(_player.position.y - transform.position.y));

        //Y values are different and the player is not currently jumping OR the player is currently jumping
        if (jumping || (_onGround && Mathf.Abs(_player.position.y - transform.position.y) > minYDifForJump && !playerJump.IsJumping))
        {
            return true;
        }

        return false;
    }

}
