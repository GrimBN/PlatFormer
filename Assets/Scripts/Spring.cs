using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    /*BoxCollider2D springCollider;
    Animator springAnimator;
    Vector2 launchVelocity;
    Rigidbody2D characterRigidbody;
    //Tim timCharacter;
    [SerializeField] Vector2 slowedVelocity;    
    [SerializeField] float verticalLaunchVelocity;

    void Start()
    {
        springCollider = GetComponent<BoxCollider2D>();
        springAnimator = GetComponent<Animator>();
        launchVelocity.y = verticalLaunchVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(springCollider.IsTouchingLayers(LayerMask.GetMask("Tim")))
        {
            springAnimator.SetBool("onSpring",true);
            characterRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            timCharacter = collision.gameObject.GetComponent<Tim>();
            timCharacter.OnSpring();
            if(characterRigidbody != null)
            {
                launchVelocity.x = characterRigidbody.velocity.x;
                characterRigidbody.velocity = slowedVelocity;
            }
        }
    }

    public void Bounce(Rigidbody2D collidingRigidbody)
    {
        if (springCollider.IsTouchingLayers(LayerMask.GetMask("Tim")))
        {
            characterRigidbody = collidingRigidbody;
            springAnimator.SetBool("onSpring", true);                       
            if (characterRigidbody != null)
            {
                launchVelocity.x = characterRigidbody.velocity.x;
                characterRigidbody.velocity = slowedVelocity;
            }
        }
    }

    public void Launch()
    {
        if (characterRigidbody != null)
        {
            characterRigidbody.velocity += launchVelocity;
            Debug.Log(characterRigidbody.velocity);
            springAnimator.SetBool("onSpring", false);
            characterRigidbody.gameObject.GetComponent<Tim>().NotOnSpring();
        }
    }*/
}
