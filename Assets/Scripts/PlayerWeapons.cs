using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Weapons (Fire Points)")]
    [SerializeField] private GameObject[] guns = new GameObject[4];

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private AudioClip blasterSound;

    [Header("Fire Rate")]
    [SerializeField] private float fireRate = 0.2f;

    [Header("Presets")]
    [SerializeField] private int currentGun;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset inputActions;

    private float nextFireTime = 0f;
    private bool isFiring = false;
    private InputAction attackAction;

    void Awake()
    {
        attackAction = inputActions.FindActionMap("Player").FindAction("Attack");
        // Sets up player input actions for attacking
    }

    void Update()
    {
        isFiring = attackAction.IsPressed();

        if (isFiring && Time.time >= nextFireTime)
        {
            Shoot(currentGun);
            currentGun = (currentGun + 1) % guns.Length;
            nextFireTime = Time.time + fireRate;
            // Shoots from the current gun after a cooldown.
            // Cycles through the guns array after eachh shot
        }
    }

    void Shoot(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < guns.Length && guns[gunIndex] != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, guns[gunIndex].transform.position, guns[gunIndex].transform.rotation);
            BulletMovement bulletMovement = bullet.GetComponent<BulletMovement>();
            bulletMovement.SetDirection(guns[gunIndex].transform.forward, bulletSpeed);
            AudioSource.PlayClipAtPoint(blasterSound, guns[gunIndex].transform.position);
            // Instantiates the bullet at the current rotation, plays a shooting sound effect
        }
    }
}
