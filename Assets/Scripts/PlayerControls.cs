using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{   
    // movement variables (adjusted in the inspector)
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = -9.81f;


    private CharacterController controller;  // Controls player movement and collision
    private Vector3 velocity;               // Tracks up/down movement (jumping/falling)
    private float movementX;                // Left/right input from controller
    private float movementY;                // Forward/back input from controller
    private bool isRunning;                 // is player running or walking
    private bool wasGrounded;               // Was the player on ground last frame?
    public Animator animator;               // Controls player animations

    public AudioSource footstepSource;   // Plays footstep sounds
    public AudioClip[] footstepClips;    // Array of different footstep sounds
    public float footstepInterval = 0.5f; // Time between footsteps (adjust as needed)
    private float footstepTimer = 0f;


    public AudioClip landingClip;
    public Vector2 landingPitchRange = new Vector2(0.95f, 1.05f); // slight variation
    public float landingFootstepLockout = 0.1f; // prevent a footstep the same frame as landing

    void Start()
    {
        // get character controller componement that is attached to the player game object
        controller = GetComponent<CharacterController>();

        // get Animator component attached to the player gameobject
        animator = GetComponentInChildren<Animator>();
    }


    // called when player makes movement through keyboard or through game controller
    void OnMovement(InputValue value)
    {
        // Get the 2D input (x = left/right, y = forward/back)
        Vector2 input = value.Get<Vector2>();

        // store left and right movement
        movementX = input.x;

        // store forward and back movement
        movementY = input.y;

    }

    // Called when player presses the run button
    void OnRun(InputValue value)
    {
        if(value.isPressed)
        {
            // Toggle running each time button is pressed
            isRunning = !isRunning;
        }
    }


    // Called when player presses the jump button
    void OnJump(InputValue value)
    {
        //  jump if button was pressed AND player is on the ground
        if (value.isPressed && controller.isGrounded)
        {
            // Calculate upward velocity for the jump 
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);

            // Animator plays the jump animation 
            animator.SetBool("IsJumping", true);
        }
    }

    void Update()
    {

        // HORIZONTAL MOVEMENT

        // Choose speed based on whether player is running or walking
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Create movement direction based on player input
        Vector3 move = transform.right * movementX + transform.forward * movementY;

        // Move the player horizontally
        controller.Move(move * currentSpeed * Time.deltaTime);

        //  VERTICAL MOVEMENT
        // keep player on the ground (prevents player game object  from falling)
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }
        // Apply gravity when the player is jumping to bring them down
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically so they either jump or fall
        controller.Move(velocity * Time.deltaTime);

        // FOOTSTEP SOUND
        bool isMoving = (movementX != 0f || movementY != 0f);
        if (controller.isGrounded && isMoving)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                // Adjust interval based on walking/running
                footstepTimer = isRunning ? footstepInterval / 1.5f : footstepInterval;
            }
        }
        else
        {
            // reset timer when not moving or in air
            footstepTimer = 0f;
        }

        //  ANIMATION UPDATES 

        //  how much the player is moving (0 = not moving, 1 = full speed)
        float inputMag = Mathf.Clamp01(new Vector3(movementX, 0f, movementY).magnitude);

        // Convert to animation speed value (0=idle, 1=walk, 3=run)
        float targetSpeed = inputMag * (isRunning ? 3f : 1f);

        // Use animation depending on speed value
        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        //  Landing detection
        // executed if player just landed
        if (!wasGrounded && controller.isGrounded)
        {
            // stop the jumping animation
            animator.SetBool("IsJumping", false);
            PlayLanding();
            footstepTimer = landingFootstepLockout;

        }

        //check is player is grounded in upcoming frames
        wasGrounded = controller.isGrounded;
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int index = Random.Range(0, footstepClips.Length);
            footstepSource.PlayOneShot(footstepClips[index]);
        }
    }

    void PlayLanding()
    {
        if (footstepSource != null && landingClip != null)
        {
            // subtle pitch variation feels more natural
            if (landingPitchRange.x > 0f && landingPitchRange.y > 0f)
            {
                footstepSource.pitch = Random.Range(landingPitchRange.x, landingPitchRange.y);
            }
            else
            {
                footstepSource.pitch = 1f;
            }
            footstepSource.PlayOneShot(landingClip);
        }
    }

}
