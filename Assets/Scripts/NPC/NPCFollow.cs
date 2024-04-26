using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 5f;

    public float FollowDistance { get { return followDistance; } }

    [SerializeField][Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField][Range(0f, 100f)] private float maxGroundAcc = 35f;
    [SerializeField][Range(0f, 100f)] private float maxSlowdownAcc = 15f;
    [SerializeField][Range(0f, 100f)] private float maxAirAcc = 20f;
    
    public float MaxSlowdownAcc { get { return maxSlowdownAcc; } }
    public float MaxAirAcc { get { return maxAirAcc; } }

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;

    private Transform player;

    private Rigidbody2D rb;

    private bool _following = false;
    private bool _slowing = false;
    private NPCJump jumpCommands;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();

    }


    // Update is called once per frame
    void Update()
    {

        if (!_following)
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
        if (!_following)
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

        if (_slowing && _currentVelocity.x == 0)
        {
            StopFollowing();
        }

        rb.velocity = _currentVelocity;
    }

        public void StartFollowing(Transform target)
    {
        player = target;
        _following = true;
    }

    public void StopFollowing()
    {
        _following = false;
    }

    public bool CheckIfNeedFollow(Transform _player, NPCJump npcJump)
    {
        if ((Vector2.Distance(_player.position, transform.position) > followDistance || _following) && !npcJump.CheckIfNeedToJump(_player))
        {
            return true;
        }

        return false;
    }
}
