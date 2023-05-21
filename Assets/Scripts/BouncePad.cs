using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    Animator animator;
    [SerializeField] float timBounceSpeed;
    [SerializeField] float tumBounceSpeed;
    float bounceSpeed;
    Tim tim;
    BoxCollider2D launchTrigger;

    void Start()
    {
        animator = GetComponent<Animator>();
        launchTrigger = GetComponent<BoxCollider2D>();
        //GameMode mode = FindObjectOfType<GameMode>();
        bounceSpeed = GameMode.instance.p_Character == GameMode.Character.Tim ? timBounceSpeed : tumBounceSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        launchTrigger.enabled = false;
        animator.SetTrigger("Launch");
        tim = collision.gameObject.GetComponentInParent<Tim>();

    }

    private void Launch()
    {
        if (tim != null)
        {
            tim.Jump(bounceSpeed);
        }
    }

    private void EnableLaunchTrigger()
    {
        launchTrigger.enabled = true;
    }   
}
