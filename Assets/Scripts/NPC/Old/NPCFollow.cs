using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 8f;
    [SerializeField][Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField][Range(0f, 100f)] private float maxGroundAcc = 35f;
    [SerializeField][Range(0f, 100f)] private float maxSlowdownAcc = 15f;
    [SerializeField][Range(0f, 100f)] private float maxAirAcc = 20f;

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;
    private bool _slowing = false;

    private Transform player;

    private Rigidbody2D rb;

    private bool following = false;

    private NPCBrain brain;

    StateObject followingState;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
        brain = GetComponent<NPCBrain>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = brain.Player;

        followingState = new StateObject(NPCStates.Following, brain.StatePriorities.GetValueOrDefault(NPCStates.Following));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) > followDistance && !following)
        {
            RequestToFollow();
        }

        if (!following)
        {
            return;
        }

        if (Vector2.Distance(player.position, transform.position) > followDistance)
        {
            _slowing = false;
            Vector2 _direction = player.position - transform.position;

            _desiredVelocity = new Vector2(_direction.normalized.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetrieving.Friction, 0f);
        }
        else
        {
            _slowing = true;
            _desiredVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!following)
        {
            return;
        }

        _onGround = _collisionDataRetrieving.OnGround;
        _currentVelocity = rb.velocity;

        if (!_slowing)
        {
            _acceleration = _onGround ? maxGroundAcc : maxAirAcc;
        }
        else
        {
            _acceleration = _onGround ? maxSlowdownAcc : maxAirAcc;
        }

        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);

        if(_slowing && _currentVelocity.x == 0)
        {
            RequestComplete();
        }

        rb.velocity = _currentVelocity;
    }

    private void RequestToFollow()
    {
        brain.HandleRequest(followingState, false);
    }

    private void RequestComplete()
    {
        StopFollowing();

        brain.HandleRequest(followingState, true);
    }

    public void StartFollowing()
    {
        following = true;
    }

    public void StopFollowing()
    {
        following = false;
    }
}
