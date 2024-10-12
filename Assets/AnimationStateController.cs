using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isMovingHash;
    int isDeadHash;
    int reviveHash;

    // Speed of rotation when changing direction
    public float rotationSpeed = 10f;

    // Reference to PlayerHealth script
    private PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        isMovingHash = Animator.StringToHash("IsMoving");
        isDeadHash = Animator.StringToHash("IsDead");
        reviveHash = Animator.StringToHash("Revive");

        // Try to get the PlayerHealth component on the same GameObject
        playerHealth = GetComponent<PlayerHealth>();

        // If not found, attempt to find it in the scene (you may need to customize this)
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth component not found on this GameObject. Searching in the scene...");
            playerHealth = FindObjectOfType<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component could not be found in the scene.");
                return;
            }
        }

        // Subscribe to the OnDeath and OnRevive events
        if (playerHealth != null)
        {
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
            Debug.Log("OnDeath Listener added");
            playerHealth.OnRevive.AddListener(OnPlayerRevive);
            Debug.Log("OnRev Listener added");
        }
    }

    void Update()
    {
        // If the player is dead, do not allow movement
        if (playerHealth != null && playerHealth.IsDead)
        {
            // Ensure IsMoving is set to false when dead
            animator.SetBool(isMovingHash, false);
            return;
        }

        bool isMoving = animator.GetBool(isMovingHash);

        // Keyboard inputs
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        // Xbox controller inputs (using the left joystick)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if moving using keyboard or Xbox controller
        bool isMovingInput = forwardPressed || backwardPressed || leftPressed || rightPressed ||
                             Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        // Set the "IsMoving" parameter based on input
        if (!isMoving && isMovingInput)
        {
            animator.SetBool(isMovingHash, true);
        }
        if (isMoving && !isMovingInput)
        {
            animator.SetBool(isMovingHash, false);
        }

        // Determine movement direction based on keyboard inputs
        Vector3 movementDirection = Vector3.zero;
        if (forwardPressed)
        {
            movementDirection += Vector3.forward;
        }
        if (backwardPressed)
        {
            movementDirection += Vector3.back;
        }
        if (leftPressed)
        {
            movementDirection += Vector3.left;
        }
        if (rightPressed)
        {
            movementDirection += Vector3.right;
        }

        // Combine controller input and keyboard input direction
        movementDirection += new Vector3(horizontalInput, 0, verticalInput);

        // If there's any movement, rotate the character to face the movement direction
        if (movementDirection.magnitude > 0.1f)
        {
            movementDirection.Normalize(); // Normalize direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Handle death event from PlayerHealth
    void OnPlayerDeath(PlayerHealth player)
    {
        // Stop movement immediately
        animator.SetBool(isMovingHash, false);

        // Trigger death animation
        animator.SetTrigger(isDeadHash);  // Ensure 'IsDead' is a trigger in your Animator
        Debug.Log("Player died and triggered death animation.");
    }

    // Handle revive event from PlayerHealth
    void OnPlayerRevive(PlayerHealth player)
    {
        // Reset death trigger and stop death animation (optional, depends on your Animator setup)
        animator.ResetTrigger(isDeadHash);
        animator.SetTrigger(reviveHash);
        //animator.ResetTrigger(reviveHash);
        animator.SetBool(isMovingHash, false);  // Stop moving on revive initially
        Debug.Log("Player revived.");
    }
}
