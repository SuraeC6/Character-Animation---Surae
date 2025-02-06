using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoximonCyclopsController : MonoBehaviour
{
    // Access Unity APIs for components
    public CharacterController controller;
    public Animator anim;
    public AudioClip runningSound;
    private AudioSource audioSource;

    // Values for rotation, jump height, and running speeds
    public float runningSpeed = 4.0f;
    public float rotationSpeed = 100.0f;
    public float jumpHeight = 6.0f;

    // Declare player input variables
    private float jumpInput;
    private float runInput;
    private float rotateInput;

    // Declare a 3D vector for moving
    public Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        // Get components on the GameObject
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input values
        runInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");

        // Call CheckJump() and Effects() functions
        CheckJump();
        Effects();

        // Set moveDir to new Vector3 based on player input
        moveDir = new Vector3(0, jumpInput * jumpHeight, runInput * runningSpeed);

        // Update the character's direction based on the game world and player input
        moveDir = transform.TransformDirection(moveDir);

        // Move the character using the controller
        controller.Move(moveDir * Time.deltaTime);

        // Rotate the character based on horizontal input
        transform.Rotate(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);
    }

    // Function to check for jump input
    void CheckJump()
    {
        // If space key is pressed, set jumpInput to 1 (jump)
        if (Input.GetKey(KeyCode.Space))
        {
            jumpInput = 1;

            // Stop running sound if the jump input is detected
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // If the character is grounded, set jumpInput back to 0
        if (controller.isGrounded)
        {
            jumpInput = 0;
        }
    }

    // Function to handle animations and sound effects
    void Effects()
    {
        // If the player is running, update animation and play sound
        if (runInput != 0)
        {
            anim.SetBool("Run Forward", true);

            if (audioSource != null && !audioSource.isPlaying && controller.isGrounded)
            {
                audioSource.clip = runningSound;
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("Run Forward", false);

            // Stop the running sound if player is not moving
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}