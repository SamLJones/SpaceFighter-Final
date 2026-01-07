using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private WingRotation wingRotation;

    [Header("Movement")]
    [SerializeField] private float minimumSpeed = 10f;
    [SerializeField] private float defaultSpeed = 20f;
    [SerializeField] private float maximumSpeed = 50f;
    [SerializeField] private float speedAccelTime = 0.5f;
    [SerializeField] private float turnSpeed = 90f;
    public AudioClip SpaceshipHum;

    [Header("Roll visuals")]
    [SerializeField] private float maxRollAngle = 45f;
    [SerializeField] private float rollAccelTime = 0.1f;

    [Header("Death References")]
    public int PlayerHealth = 3;
    [SerializeField] private GameObject deathExplosion;
    [SerializeField] private AudioClip deathSound;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActions;

    private float rollVelocity;
    private float currentSpeed;
    private float speedVelocity;
    private Vector2 moveInput;
    private InputAction moveAction;
    private bool isDead = false;
    private AudioSource audioSource;

    void Start()
    {
        currentSpeed = defaultSpeed;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = SpaceshipHum;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.pitch = 1.0f;
        audioSource.Play();
        // Plays a constant spaceship hum on loop, which indicates the player speed.
    }

    void Awake()
    {
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        // Sets up player input actions (modern unity input system)
    }

    void Update()
    {
        if (isDead) return;
        // Prevents player from interacting with the game when dead.

        moveInput = moveAction.ReadValue<Vector2>();

        float yaw = moveInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f, Space.Self);
        // Uses A / D to rotate the player left and right on the Y axis.

        float targetRoll = moveInput.x * maxRollAngle;
        float currentRoll = playerBody.localEulerAngles.z;
        float newRoll = Mathf.SmoothDampAngle(currentRoll, targetRoll, ref rollVelocity, rollAccelTime);
        playerBody.localEulerAngles = new Vector3(0f, 0f, newRoll);
        // When yawwing, rotates the player model, using SmoothDampAngle for a smooth transition when changing directions.

        float targetSpeed = defaultSpeed;
        if (moveInput.y > 0f)
            targetSpeed = Mathf.Lerp(defaultSpeed, maximumSpeed, moveInput.y);
        else if (moveInput.y < 0f)
            targetSpeed = Mathf.Lerp(defaultSpeed, minimumSpeed, -moveInput.y);
        // Uses W / S to accelerate and decelerate the player
        // Lerp is used to smoothly accelerate and transition between the speeds.

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, speedAccelTime);
        wingRotation.currentSpeed = currentSpeed;   // Pushes the current speed to the wing rotation script.

        audioSource.pitch = 0.5f + (currentSpeed / maximumSpeed) * 1.5f;    // Adjusts the pitch of the spaceship hum based on current speed.
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Vector3 movement = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        transform.position += movement * currentSpeed * Time.fixedDeltaTime;
        // Constantly moves the player model forward on the x and z planes.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            KillPlayer();
    }   // Kills the player on collision with terrain.

    private void OnTriggerEnter(Collider other)
    {
        KillPlayer();
    }   // Kills the player when an item with a trigger collider is touched, e.g. the enemy blasters.

    private void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        defaultSpeed = 0f;
        currentSpeed = 0f;
        maximumSpeed = 0f;
        minimumSpeed = 0f;

        playerBody.gameObject.SetActive(false);
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        StartCoroutine(ReloadAfterDelay(1f));

        // Hides the player body model, replacing it with an explosion effect
        // Reloads the scene after a 1 second delay.
    }

    private IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Game");
    }
}
