using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 5f;
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

        // Check if the player is sprinting and has enough stamina
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerStats.currentStamina > 0;

        // Set movement speed based on sprinting status
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Calculate movement direction based on input
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Drain stamina if sprinting
        if (isSprinting)
        {
            playerStats.UseStamina(20f * Time.deltaTime);
        }
    }
}
