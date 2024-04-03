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

    private void Awake()
    {
        brain = GetComponent<NPCBrain>();
        _npcRigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<CollisionDataRetrieving>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = brain.Player;
        playerJump = player.GetComponent<Jump>();
    }

    // Update is called once per frame
    void Update()
    {
        //Y values are different and the player is not currently jumping
        if (_onGround && player.position.y != transform.position.y && !playerJump.IsJumping)
        {
            RequestToJump();
        }

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

        if (!_isJumping)
        {
            _isJumping = true;
            CaclulateJumpForce();
        }

        if (_onGround && _npcRigidbody.velocity.y == 0)
        {
            RequestComplete();
        }
    }


    private void CaclulateJumpForce()
    {

        Vector3 target = player.position;

        float gravity = Physics.gravity.magnitude;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);

        if(distance == 0)
        {
            return;
        }

        // Distance along the y axis between objects
        float yOffset = transform.position.y - target.y;
        float xOffset = transform.position.x - target.x;

        if(xOffset == 0 || distance == 0)
        {
            return;
        }

        float angle = Mathf.Atan(yOffset / xOffset) * Mathf.Deg2Rad;

        Debug.Log(angle);

        float initialVelocity = (1f / Mathf.Cos(angle)) * Mathf.Sqrt(Mathf.Abs((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset)));
        Debug.Log(Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2))));
        Debug.Log((0.5f * gravity * Mathf.Pow(distance, 2) / (distance * Mathf.Tan(angle) + yOffset)));
        Debug.Log(initialVelocity);


        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        Debug.Log(finalVelocity);
        _npcRigidbody.velocity = finalVelocity;
    }

    private void CheckForGround()
    {

    }

    public void StartJumping()
    {
        jumping = true;
    }

    public void StopJumping()
    {
        jumping = false;
    }

    private void RequestToJump()
    {
        brain.HandleRequest(NPCStates.Jumping, false);
    }

    private void RequestComplete()
    {
        StopJumping();

        brain.HandleRequest(NPCStates.Jumping, true);

        _isJumping = false;
    }
}
