using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSlowdown : MonoBehaviour
{
    [Header("Slowing Settings")]
    [SerializeField][Range(0f, 100f)] private float maxSlowdownAcc = 15f;
    [SerializeField][Range(0f, 100f)] private float maxAirAcc = 20f;

    public float MaxAirAcc { get { return maxAirAcc; } }

    private Vector2 _direction;
    private Vector2 _desiredVelocity;
    private Vector2 _currentVelocity;
    private CollisionDataRetrieving _collisionDataRetrieving;
    private float _maxSpeedChange;
    private float _acceleration;
    private bool _onGround;


    private Rigidbody2D rb;

    private bool slowing = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!slowing)
        {
            return;
        }

        _desiredVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!slowing)
        {
            return;
        }

        _onGround = _collisionDataRetrieving.OnGround;
        _currentVelocity = rb.velocity;

        _acceleration = _onGround ? maxSlowdownAcc : maxAirAcc;

        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);

        if (slowing && _currentVelocity.x == 0)
        {
            StopSlowingdown();
        }

        rb.velocity = _currentVelocity;
    }

    public void StartSlowingdown()
    {
        slowing = true;

        Debug.Log("Slowing");
    }

    public void StopSlowingdown()
    {
        slowing = false;
    }

    public bool CheckIfNeedSlowdown(Transform player, float followDistance)
    {
        if (Vector2.Distance(player.position, transform.position) <= followDistance && _currentVelocity.x != 0)
        {
            return true;
        }

        return false;
    }
}
