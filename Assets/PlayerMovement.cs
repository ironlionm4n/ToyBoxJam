using System;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator playerAnimator;

    [Header("Running")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxXVelocity = 12f;
    [SerializeField] private float moveHorizontal;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float fallForce = 5f;
    [SerializeField] private float moveVertical;

    private Vector2 _moveDirection = Vector2.zero;
    private bool _shouldJump;

    private void Awake()
    {
        playerRigidbody.gravityScale = fallForce;
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Sets animator values
        playerAnimator.SetFloat("HorizontalMovement", moveHorizontal);
        playerAnimator.SetFloat("VerticalMovement", playerRigidbody.velocity.y);

        if (moveHorizontal < 0)
            spriteRenderer.flipX = true;
        if (moveHorizontal > 0)
            spriteRenderer.flipX = false;
        

        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnimator.SetBool("Jumping", true);
            _shouldJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (moveHorizontal > 0.1f)
        {
            if (playerRigidbody.velocity.x < maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
            }
        }

        if (moveHorizontal < -0.1f)
        {
            if (playerRigidbody.velocity.x > -maxXVelocity)
            {
                playerRigidbody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
            }
        }

        if (_shouldJump && !isJumping) Jump();
        
    }

    private void Jump()
    {
        playerRigidbody.AddForce(new Vector2(0f, moveVertical * jumpForce), ForceMode2D.Impulse);
        _shouldJump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            playerAnimator.SetBool("Jumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }
}
