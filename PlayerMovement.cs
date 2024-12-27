using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float initialMoveTime = 1.0f;
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public Animator animator;
    public LayerMask groundLayer;
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 5, -10);
    public float cameraSmoothSpeed = 0.125f;
    public GameObject gameOverMenu;
    public GameObject winPanel;
    public GameObject questionPanel; // Reference to the question panel
    public Transform cameraFrontPosition; // Camera position in front of the player during talk
    public float cameraRotationSpeed = 50f;
    private SoundEffectsManager soundEffectsManager;
    private Rigidbody rb;
    private bool isRunning = false;
    private bool canDoubleJump = false;
    private bool isGrounded = false;
    private int jumpCount = 0;
    private bool isDead = false;
    private bool isWinning = false;
    private bool isTalking = false;
    public ParticleSystem jumpParticleEffect;
    private ParticleSystem.MainModule particleMainModule; // To access the particle system's main settings

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
        gameOverMenu.SetActive(false);
        winPanel.SetActive(false);
        questionPanel.SetActive(false); // Hide the question panel at the start
        if (jumpParticleEffect != null)
        {
            particleMainModule = jumpParticleEffect.main; // Get the main module of the particle system
            jumpParticleEffect.Stop(); // Ensure particle effect is stopped at the start
        }
        // Start the initial sequence
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        // Play idle animation
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(initialMoveTime);

        // Move the camera to the front of the player and show question panel
        cameraTransform.position = cameraFrontPosition.position;
        cameraTransform.rotation = Quaternion.Euler(0, 180, 0); // Set the camera rotation as specified

        // Play talking animation and show question panel
        animator.SetBool("isTalk", true);
        questionPanel.SetActive(true);
        isTalking = true;

        // Wait for the player to finish talking (simulate button click)
        yield return new WaitUntil(() => !isTalking);

        // Hide question panel and play thanking animation
        questionPanel.SetActive(false);
        animator.SetBool("isTalk", false);
        animator.SetBool("isThanks", true);

        // Wait for the thanking animation to play
        yield return new WaitForSeconds(1.0f); // Wait time can be adjusted based on the length of the animation

        // Resume normal gameplay sequence
        animator.SetBool("isThanks", false);

        // Smoothly move the camera back to a slightly elevated position
        Vector3 elevatedPosition = cameraFrontPosition.position + new Vector3(0, 5, -10); // Slightly above and behind player
        Quaternion elevatedRotation = Quaternion.Euler(30, 0, 0); // Tilted downwards to show more of the environment

        // Adjust the transition time for a lighter effect
        float transitionTime = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, elevatedPosition, elapsedTime / transitionTime);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, elevatedRotation, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera is exactly at the target position and rotation
        cameraTransform.position = elevatedPosition;
        cameraTransform.rotation = elevatedRotation;

        // Play idle animation for 2 seconds
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(2.0f);

        // Stop idle animation and start running
        animator.SetBool("isIdle", false);
        StartRunning();
    }



    public void OnFinalButtonClick()
    {
        // Called when the final button on the panel is clicked
        isTalking = false;
        FindObjectOfType<BackgroundMusicManager>().PlayGameplayMusic();

    }

    private void Update()
    {
        if (isDead || isWinning || isTalking) return;

        if (isRunning)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if ((Input.GetMouseButtonDown(0) && IsRightSideOfScreen()) || Input.GetKeyDown(KeyCode.Space))

        {
            if (isGrounded)
            {
                Jump();
            }
            else if (jumpCount == 1 && canDoubleJump)
            {
                DoubleJump();
            }
        }

        FollowPlayer();

        // Ensure the run animation only plays when grounded
        CheckGroundStatus();

        if (isGrounded && isRunning && rb.linearVelocity.y == 0)
        {
            animator.SetBool("isRun", true);
        }
    }

    private void StartRunning()
    {
        isRunning = true;
        soundEffectsManager.PlayRunSound(); // Play run sound when the player starts running

    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset Y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJump", true);
            jumpCount++;
            isGrounded = false;
            canDoubleJump = true;
            animator.SetBool("isRun", false);
            soundEffectsManager.StopRunSound();

            soundEffectsManager.PlayJumpSound(); // Play jump sound

            // Play jump particle effect
            if (jumpParticleEffect != null)
            {
                // Randomize the color of the particle effect after each jump
                particleMainModule.startColor = GetRandomColor();
                jumpParticleEffect.Play();
            }

            StartCoroutine(ResetAnimationBool("isJump"));
        }
    }

    // Function to get a random color
    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }


private void DoubleJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset Y velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isDoubleJump", true);
        jumpCount++;
        canDoubleJump = false; // No triple jump
        soundEffectsManager.StopRunSound();

        soundEffectsManager.PlayDoubleJumpSound(); // Play double jump sound

        StartCoroutine(ResetAnimationBool("isDoubleJump"));
    }

    private IEnumerator ResetAnimationBool(string boolName)
    {
        // Wait for the current animation to complete based on its length
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Ensure the animation lasts at least one second
        float waitTime = Mathf.Max(animationLength, 1.0f);

        // Wait until the animation is completely finished or one second has passed
        yield return new WaitForSeconds(waitTime);

        // Reset the animation bool
        animator.SetBool(boolName, false);
    }

    private bool IsRightSideOfScreen()
    {
        return Input.mousePosition.x > Screen.width / 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Black"))
        {
            Die();
            FindObjectOfType<BackgroundMusicManager>().PlayMenuMusic();

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            canDoubleJump = false;

            // Only set isRun to true after landing on the ground
            if (isRunning)
            {
                animator.SetBool("isRun", true);
            }
        }
        else if (collision.gameObject.CompareTag("Intro"))
        {
            Win();
        }
    }

    private void Die()
    {

        isRunning = false;
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        animator.SetBool("isDead", true);
        soundEffectsManager.StopRunSound();
        soundEffectsManager.PlayDeathSound(); // Play death sound

        Invoke(nameof(ShowGameOverMenu), 2.5f);
    }

    private void Win()
    {
        isRunning = false;
        isWinning = true;

        // Rotate player to face the camera
        transform.rotation = Quaternion.Euler(0, 180, 0);
        soundEffectsManager.PlayWinSound(); // Play death sound

        // Start the win celebration coroutine
        StartCoroutine(WinCelebration());
    }

    private IEnumerator WinCelebration()
    {
        // Play the win animation
        animator.SetBool("isWin", true);

        // Rotate the camera around the player while the win animation plays
        float winAnimationDuration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < winAnimationDuration)
        {
            cameraTransform.RotateAround(transform.position, Vector3.up, cameraRotationSpeed * Time.deltaTime);
            cameraTransform.LookAt(transform.position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Show the win panel after the animation and camera rotation
        ShowWinPanel();
    }

    private void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        FindObjectOfType<BackgroundMusicManager>().StopMusic();


    }

    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
        FindObjectOfType<BackgroundMusicManager>().StopMusic();

    }

    private void FollowPlayer()
    {
        if (isDead || isWinning || isTalking) return;

        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);
        cameraTransform.position = smoothedPosition;

        cameraTransform.LookAt(transform.position);
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);
    }

}
