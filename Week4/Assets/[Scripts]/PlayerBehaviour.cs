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

    [Header("Animation Properties")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayerMask);

        if (isGrounded)
        {
            float run = Input.GetAxisRaw("Horizontal");
            float jump = Input.GetAxisRaw("Jump");
            float crouch = Input.GetAxisRaw("Crouch");

            //check if the player is moving
            if (run != 0 && crouch == 0)
            {
                run = Flip(run);

                //can use this to shift the animation
                animator.SetInteger("AnimationState", 1); //Run State
            }
            else if (run == 0 && jump==0)
            {
                animator.SetInteger("AnimationState", 0); //Idle State
            }

            if (jump > 0 && crouch == 0)
            {
                animator.SetInteger("AnimationState", 2); //Jump State
            }
            else if(run == 0 && crouch < 0)
            {
                animator.SetInteger("AnimationState", 3); //Crouch State
            }

            if (crouch == 0)
            {
                Vector2 move = new Vector2(run * horizontalForce, jump * verticalForce);
                rigidbody2D.AddForce(move);
            }
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
