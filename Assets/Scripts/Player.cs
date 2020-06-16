using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 28f;

    //state
    bool isAlive = true;

    //cache
    Rigidbody2D myRigibody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
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

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigibody.velocity += jumpVelocityToAdd;
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
