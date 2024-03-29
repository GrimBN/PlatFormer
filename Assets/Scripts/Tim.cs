﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tim : MonoBehaviour
{
    [SerializeField] float jumpSpeed;
    [SerializeField] float horizontalMoveSpeed;
    [SerializeField] float deathHorizontalKnockback;
    [SerializeField] float deathVerticalKnockback;
    [SerializeField] float deathTorque;    
    [Range(0,1)][SerializeField] float volume = 0.5f;
    bool isPlaying = false;
    bool isAlive = true;
    bool hasWon = false;

    [SerializeField] BoxCollider2D feetCollider2D;
    [SerializeField] BoxCollider2D bodyCollider2D;
    [SerializeField] PhysicsMaterial2D fullFrictionMaterial;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip runSFX;
    AudioSource audioSource;
    Collider2D mainCollider2D;
    Rigidbody2D timRigidbody2D;
    Animator animator;

    LevelController levelController;

    void Start()
    {        
        timRigidbody2D = GetComponent<Rigidbody2D>();
        mainCollider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        levelController = FindObjectOfType<LevelController>();
    }
    
    private void Move()
    {
        if(!isPlaying ) { return; }
        Vector2 velocity = new Vector2(Mathf.Sign(transform.localScale.x) * horizontalMoveSpeed, timRigidbody2D.velocity.y);
        timRigidbody2D.velocity = velocity;
        animator.SetBool("isRunning", true);        
    }

    private void TurnAround()
    {        
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y,transform.localScale.z);       
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if ((bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) && isAlive)
        {
            TurnAround();
        }        
        else if(collision.gameObject.tag == "Goal" && isAlive && !hasWon)
        {
            HandleWin();
        }        
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (mainCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards")) && isAlive && !hasWon)
        {
            HandleDeath();
        }
        else if (mainCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) || mainCollider2D.IsTouchingLayers(LayerMask.GetMask("Bounce Pad")))
        {
            Move();
        }           
    }

    private void HandleWin()
    {
        isPlaying = false;
        hasWon = true;
        timRigidbody2D.velocity = new Vector2(0f, 0f);
        timRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isRunning", false);
        levelController.Win();
    }

    private void HandleDeath()
    {
        isAlive = false;
        animator.SetTrigger("Death");
        timRigidbody2D.constraints = RigidbodyConstraints2D.None;
        timRigidbody2D.sharedMaterial = fullFrictionMaterial;
        timRigidbody2D.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * deathHorizontalKnockback, deathVerticalKnockback);
        timRigidbody2D.AddTorque(deathTorque);
        levelController.Lose();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
        // OK so for some reason LayerMask.GetMask() returns the mask value in the form of 2^(Mask integer value) so need to compare with the log (or 2^ collision.gameObject.layer)
        if (collision.gameObject.layer!= Mathf.Log(LayerMask.GetMask("Boundary"),2) && 
            collision.gameObject.layer != Mathf.Log(LayerMask.GetMask("Hazards"), 2) && 
            collision.gameObject.tag != "Star" && collision.gameObject.tag != "Bounce Pad" &&
            collision.gameObject.layer != Mathf.Log(LayerMask.GetMask("Interactables"),2) &&
            !feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Bounce Pad")))
        {                        
            Jump(jumpSpeed);
        }
    }

    public void Jump(float jumpSpeed)
    {   
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && !feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Boundary")) && isAlive)
        {            
            timRigidbody2D.velocity = new Vector2(timRigidbody2D.velocity.x, jumpSpeed);
            animator.SetTrigger("Jump");                       
            audioSource.PlayOneShot(jumpSFX, volume/2);
        }
    }   

    public void PlayRunSFX()
    {
        audioSource.PlayOneShot(runSFX, volume);
    }

    public void SetPlaying(bool playStatus)
    {
        isPlaying = playStatus;
        Move();
    }
}
