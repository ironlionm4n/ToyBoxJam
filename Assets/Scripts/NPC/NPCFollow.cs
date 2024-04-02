using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 8f;
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


    private Transform player;

    private Rigidbody2D rb;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        _collisionDataRetrieving = GetComponent<CollisionDataRetrieving>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.position, transform.position) > followDistance)
        {
            Vector2 _direction = player.position - transform.position;
            _direction.y = 0;
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetrieving.Friction, 0f);
            Debug.Log(_collisionDataRetrieving.Friction);
        }
    }

    private void FixedUpdate()
    {
        _onGround = _collisionDataRetrieving.OnGround;
        _currentVelocity = rb.velocity;
        _acceleration = _onGround ? maxGroundAcc : maxAirAcc;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _currentVelocity.x = Mathf.MoveTowards(_currentVelocity.x, _desiredVelocity.x, _maxSpeedChange);

        rb.velocity = _currentVelocity;
    }
}
