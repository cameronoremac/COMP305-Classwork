using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Movement")]
    public float horizontalForce;
    public float verticalForce;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundLayerMask;
    private Rigidbody2D rigidbody2D;
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask);

        if (isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Jump");

            //check if the player is moving
            if (x != 0)
            {
                x = Flip(x);

                //can use this to shift the animation
            }

            Vector2 move = new Vector2(x * horizontalForce, y * verticalForce);
            rigidbody2D.AddForce(move);
        }
    }

    private float Flip(float x)
    {
        x = (x > 0) ? 1 :- 1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        //Gizmos.DrawLine(transform.position, groundCheck.position);
    }
}
