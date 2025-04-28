using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 5f;

    private bool _slowing = false;
    public float FollowDistance { get { return followDistance; } }

    [SerializeField][Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField][Range(0f, 100f)] private float maxGroundAcc = 35f;

    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;

    private Transform player;

    private Rigidbody2D rb;

    private bool _following = false;

    private NPCSlowdown _slowdown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
        _slowdown = GetComponent<NPCSlowdown>();

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
            _slowdown.StopSlowingdown();
            _slowing = false;
            Vector2 _direction = player.position - transform.position;

            _desiredVelocity = new Vector2(_direction.normalized.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetrieving.Friction, 0f);
        }
        else
        {
            _slowdown.StartSlowingdown();
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
            _acceleration = _onGround ? maxGroundAcc : _slowdown.MaxAirAcc;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);

            rb.velocity = _currentVelocity;
        }
        else if (_currentVelocity.x == 0)
        {
            StopFollowing();
        }


    }

    public void StartFollowing(Transform target)
    {
        player = target;
        _slowing = false;
        _slowdown.StopSlowingdown();

        _following = true;

        Debug.Log("following");
    }

    public void StopFollowing()
    {
        _following = false;
        Debug.Log("stop following");
    }

    public bool CheckIfNeedFollow(Transform _player, NPCJump npcJump)
    {
        if ((Vector2.Distance(_player.position, transform.position) > followDistance || _following))
        {
            return true;
        }

        return false;
    }
}
