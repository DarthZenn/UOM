using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    public float runSpeed = 5f;
    public float sprintSpeed = 10f;

    public PlayerStats playerStats; // Reference to the PlayerStats script for stamina

    private Vector3 velocity;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps player grounded
        }

        // Get player input for movement (Horizontal for x, Vertical for z)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //
        bool isRunning = horizontal != 0 || vertical != 0;

        // Check if the player is sprinting and has enough stamina
        bool isSprinting = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && playerStats.currentStamina > 0;

        // Set movement speed based on sprinting status
        float currentSpeed = isSprinting ? sprintSpeed : isRunning ? runSpeed : 0;

        // Calculate movement direction based on input
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Handle vertical running animations
        if (vertical > 0) // Running Forward
        {
            animator.SetBool("isRunningForward", true);
            animator.SetBool("isRunningBackward", false);
        }
        else if (vertical < 0) // Running Backward
        {
            animator.SetBool("isRunningBackward", true);
            animator.SetBool("isRunningForward", false);
        }
        else // Idle or running left/right
        {
            animator.SetBool("isRunningForward", false);
            animator.SetBool("isRunningBackward", false);
        }

        // Handle horizontal running animations
        if (horizontal < 0) // Running Left
        {
            animator.SetBool("isRunningLeft", true);
            animator.SetBool("isRunningRight", false);
        }
        else if (horizontal > 0) // Running Right
        {
            animator.SetBool("isRunningRight", true);
            animator.SetBool("isRunningLeft", false);
        }
        else // Idle or running forward/backward
        {
            animator.SetBool("isRunningLeft", false);
            animator.SetBool("isRunningRight", false);
        }

        // Drain stamina if sprinting
        if (isSprinting)
        {
            // Set the speed in the animator based on movement magnitude
            animator.SetBool("isSprinting", true);
            playerStats.UseStamina(20f * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isSprinting", false);
        }

        // Set the speed in the animator based on movement magnitude
        animator.SetFloat("Speed", currentSpeed);
    }
}
