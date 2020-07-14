using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 28f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(250f, 250f);

    //state
    bool isAlive = true;

    //cache
    Rigidbody2D myRigibody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2d;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2d = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigibody.gravityScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");  // -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigibody.velocity.y);
        myRigibody.velocity = playerVelocity;

        // setting animation to running
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigibody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    public void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myAnimator.SetBool("Climbing", false);
            myRigibody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigibody.velocity.x ,controlThrow * climbSpeed);
        myRigibody.velocity = climbVelocity;
        myRigibody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigibody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigibody.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if (myBodyCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            print("die");
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigibody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigibody.velocity.x), 1f);
        }
    }
}
