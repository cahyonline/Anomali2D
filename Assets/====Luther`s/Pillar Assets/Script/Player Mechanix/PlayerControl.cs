using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{   
    //=================================================================
    public float moveIt = 5f;               public GameObject swordHitBox;
    public float jumpIt = 10f;              
    private bool isGrounded;                
    private bool lookright;                 
    private Rigidbody2D rb;                 
    public Transform groundCheck;
    public LayerMask groundLayer;           public bool canJump = true;
    public float groundCheckRadius = 0.2f;  
    private UnityEngine.Vector2 movement;

    //==================================================================
    //public Animator animator;
    //==================================================================
    void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        //gameOver.SetActive(false);
        lookright = true;
        swordHitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius, groundLayer);
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debugger();
        }

        #region MOVEMENT

        if (Input.GetButtonDown("Jump") && isGrounded && canJump == true) 
        {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpIt);
            //animator.SetBool("isJump?" , true);
            //Debug.Log("JUMPIN");
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            swordHitBox.SetActive(true);
            //Debug.LogWarning("ATTACKING");
        }
        else
        {
            swordHitBox.SetActive(false);
        }
        //====================================================================
        // FLIP TOGGLE
        if(movement.x > 0 && !lookright)
        {
            Flip();
        }
        else if(movement.x < 0 && lookright)
        {
            Flip();
        }
        //====================================================================
    }

    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement * moveIt * Time.fixedDeltaTime);    
        rb.velocity = new UnityEngine.Vector2(movement.x * moveIt, rb.velocity.y);
    }

        #endregion

    private void Flip()
    {
        //FLIP PLAYER 
        lookright = !lookright;
        UnityEngine.Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    //==================================================================
    //MECHANICS ALPHA
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Pillar"))
        {
            //Debug.Log(" Player hit Pillar ");
        }
    }
    //==================================================================
    //NON PRIORITY FOR SPRITE FLIP
    //==================================================================
    void Debugger()
    {
        if(isGrounded == true)
        {
            Debug.Log("IsGrounded");
        }
        else
        {
            Debug.Log("NOTgrounded");
        }
    }
    //==================================================================
}
