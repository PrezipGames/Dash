using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalInput;
    private float speed = 10;
    private float jumpForce = 13;
    private bool facingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashSpeed = 25;
    private float dashDuration = 0.25f;
    private float dashCooldown = 0.75f;

    void Update()
    {
        if(isDashing == true)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (Input.GetKeyDown("space") && isOnGround())
        {
            Jump();                               
        }

        ChangeLookDirection();

        if(Input.GetMouseButtonDown(0) && canDash == true)
        {
            StartCoroutine(Dash());
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private bool isOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }

    private void ChangeLookDirection()
    {
        if(facingRight && horizontalInput <0 || !facingRight && horizontalInput > 0)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.velocity = (mousePos - rb.position).normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = defaultGravity;
        rb.velocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
