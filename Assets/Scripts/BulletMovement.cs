using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifetime = 5f;

    public void SetDirection(Vector3 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
        // Automatically destroys the bullets after 5 seconds of travel time (optimisations)
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
    // Destroys the bullet on impact with any collider
}

// Script may be negligible as the shooting scripts handle the bullet instantiation, direction and speed.
