using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Movement")]
    public float horizontalForce;
    public float verticalForce;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    [Header("Animation Properties")]
    public Animator animator;

    [Header("Cameras")]
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera doorCamera;

    [Header("UI and Interactables")]
    public GameObject doorSign;
    public bool isInteracting;

    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Interact();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Interact()
    {
        isInteracting = Input.GetKey(KeyCode.E);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("DoorCamera"))
        {
            playerCamera.Priority = 5;
            doorCamera.Priority = 10;
            doorSign.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        playerCamera.Priority = 10;
        doorCamera.Priority = 5;
        doorSign.SetActive(false);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (isInteracting && other.gameObject.CompareTag("Door"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
