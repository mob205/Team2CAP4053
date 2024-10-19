using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isMovingHash;
    int isDeadHash;
    int reviveHash;
    int IsToolingHash;

    // Speed of rotation when changing direction
    public float rotationSpeed = 10f;

    // Reference to PlayerHealth script
    private PlayerController player;
    private PlayerHealth playerHealth;
    private PlayerInteractor playerInteractor;

    //PARTICLE SYSTEMS

    [Space(20)]
    //[Header("EFFECTS")]
    [Space(10)]
   
    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem SmokeParticleSystem;

    void Start()
    {
        animator = GetComponent<Animator>();
        isMovingHash = Animator.StringToHash("IsMoving");
        isDeadHash = Animator.StringToHash("IsDead");
        reviveHash = Animator.StringToHash("Revive");
        IsToolingHash = Animator.StringToHash("IsTooling");

        // Try to get the PlayerHealth component on the same GameObject
        player = GetComponentInParent<PlayerController>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerInteractor = player.GetComponent<PlayerInteractor>();


        // If not found, attempt to find it in the scene (you may need to customize this)
        if (playerHealth == null)
        {
            Debug.LogError("No player health found");
        }

        // Subscribe to the OnDeath and OnRevive events
        if (playerHealth != null)
        {
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
            playerHealth.OnRevive.AddListener(OnPlayerRevive);
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

        bool isMovingAnim = animator.GetBool(isMovingHash);

        bool isMovingInput = player.MoveInput != Vector2.zero;

        // Set the "IsMoving" parameter based on input
        if (!isMovingAnim && isMovingInput)
        {
            animator.SetBool(isMovingHash, true);
            SmokeParticleSystem.Play();
        }
        if (isMovingAnim && !isMovingInput)
        {
            animator.SetBool(isMovingHash, false);
            SmokeParticleSystem.Stop();
        }

        // If there's any movement, rotate the character to face the movement direction
        if (player.MoveInput != Vector2.zero)
        {
            var movementDirection = new Vector3(player.MoveInput.x, 0, player.MoveInput.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Check if the player is holding a tool and interacting with an EnemySpawner
        bool isTooling = false;
        if (playerInteractor.CurrentInteractable is EnemySpawner)
        {
            isTooling = ((EnemySpawner)playerInteractor.CurrentInteractable).IsInProgress;
        }

        // Set IsTooling in the animator based on the tool interaction status
        animator.SetBool(IsToolingHash, isTooling);
    }

    // Handle death event from PlayerHealth
    void OnPlayerDeath(PlayerHealth player)
    {
        // Stop movement immediately
        animator.SetBool(isMovingHash, false);
        SmokeParticleSystem.Stop();

        // Trigger death animation
        animator.SetTrigger(isDeadHash);  // Ensure 'IsDead' is a trigger in your Animator
    }

    // Handle revive event from PlayerHealth
    void OnPlayerRevive(PlayerHealth player)
    {
        // Reset death trigger and stop death animation (optional, depends on your Animator setup)
        animator.ResetTrigger(isDeadHash);
        animator.SetTrigger(reviveHash);
        //animator.ResetTrigger(reviveHash);
        animator.SetBool(isMovingHash, false);  // Stop moving on revive initially
    }
}