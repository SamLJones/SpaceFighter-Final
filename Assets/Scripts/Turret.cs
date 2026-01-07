using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float RotationSpeed = 5f;
    [SerializeField] private float FireRate = 2f;
    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] private float Range = 200f;
    [SerializeField] private GameObject BlasterBolt;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject gun;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private AudioClip blasterSound;
    [SerializeField] private float bulletSpeed = 50f;

    private TurretController turretController;

    [SerializeField] private int Health = 4;
    [SerializeField] private GameObject Explosion;

    void Start()
    {
        turretController = FindFirstObjectByType<TurretController>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer <= Range)
        {
            RotateTurret();
            RotateGun();
            Shoot();
        }
        // Checks the distance to the player
        // If the player is withing range, then if they are, will rotate the guns to the player and shoot.
    }

    void RotateTurret()
    {
        Vector3 direction = Player.position - turret.transform.position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion offsetRotation = lookRotation * Quaternion.Euler(0f, 180f, 0f);
        turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, offsetRotation, Time.deltaTime * RotationSpeed);
        // Rotates the turret on the horizontal axis towards the player when in range
    }

    void RotateGun()
    {
        Vector3 direction = Player.position - gun.transform.position;
        float horizontalDistance = new Vector3(direction.x, 0f, direction.z).magnitude;
        float verticalAngle = Mathf.Atan2(direction.y, horizontalDistance) * Mathf.Rad2Deg;
        float currentXRotation = gun.transform.localEulerAngles.x;
        if (currentXRotation > 180f)
        {
            currentXRotation -= 360f;
        }
        
        float newXRotation = Mathf.Lerp(currentXRotation, verticalAngle, Time.deltaTime * RotationSpeed);
        gun.transform.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }
    // Vertically rotates the gun in the turret to aim at the player
    // May not be necessary as the fighter now only travels on a horizontal plane

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(BlasterBolt, bulletSpawn.position, bulletSpawn.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = bulletSpawn.forward * bulletSpeed;
            AudioSource.PlayClipAtPoint(blasterSound, transform.position);
            nextFireTime = Time.time + 1f / FireRate;
        }
        // Shoots a blaster bolt at a set fire rate.
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage();
    }


    public void TakeDamage()
    {
        Health -= 1;

        if (Health <= 0)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
