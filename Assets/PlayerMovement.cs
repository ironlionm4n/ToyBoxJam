using System;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private float maxXVelocity = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 _moveDirection = Vector2.zero;
    private bool _shouldJump;

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        
        if (horizontal < 0)
            spriteRenderer.flipX = true;
        if (horizontal > 0)
            spriteRenderer.flipX = false;
        
        _moveDirection = new Vector2(horizontal, 0f);
        if (Input.GetKeyDown(KeyCode.W))
        {
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_shouldJump) Jump();
        playerRigidbody.AddForce(_moveDirection * moveSpeed);

        if (playerRigidbody.velocity.y < 0)
        {
            playerRigidbody.gravityScale = 3f;
        }
        else
        {
            playerRigidbody.gravityScale = 1f;
        }
    }

    private void Jump()
    {
        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _shouldJump = false;
    }
}
