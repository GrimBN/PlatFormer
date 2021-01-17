using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    Animator animator;
    [SerializeField] float timBounceSpeed;
    [SerializeField] float tumBounceSpeed;
    float bounceSpeed;
    // Rigidbody2D timRigidbody2D;
    Tim tim;
    BoxCollider2D launchTrigger;

    void Start()
    {
        animator = GetComponent<Animator>();
        launchTrigger = GetComponent<BoxCollider2D>();
        GameMode mode = FindObjectOfType<GameMode>();
        bounceSpeed = mode != null ? mode.GetCharacter() == GameMode.Character.Tim ? timBounceSpeed : tumBounceSpeed : timBounceSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        launchTrigger.enabled = false;
        animator.SetTrigger("Launch");
        //timRigidbody2D = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        tim = collision.gameObject.GetComponentInParent<Tim>();
        //Launch(collision.gameObject.GetComponent<Rigidbody2D>());
    }

    private void Launch()
    {
        //timRigidbody2D.velocity = new Vector2(timRigidbody2D.velocity.x, timRigidbody2D.velocity.y + bounceSpeed);
        if (tim != null)
        {
            tim.Jump(bounceSpeed);
        }
        //animator.SetTrigger("Launch");
    }

    private void EnableLaunchTrigger()
    {
        launchTrigger.enabled = true;
    }
   
}
