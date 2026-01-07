using UnityEngine;

public class WingRotation : MonoBehaviour
{
    [SerializeField] private GameObject Wing1;
    [SerializeField] private GameObject Wing2;
    
    public float rotationSpeed = 5f; // Speed of the rotation transition
    public float targetRotationAngle = 40f; // Target angle when above threshold
    public float speedThreshold = 105f; // Speed threshold to trigger rotation
    public float currentSpeed;

    private float currentRotationAngle = 0f;
    private Quaternion wing1InitialRotation;
    private Quaternion wing2InitialRotation;

    void Start()
    {
        // Store initial rotations to preserve Wing2's 180-degree rotation
        wing1InitialRotation = Wing1.transform.localRotation;
        wing2InitialRotation = Wing2.transform.localRotation;
    }

    void FixedUpdate()
    {
        // Determine target angle based on speed
        float targetAngle = currentSpeed > speedThreshold ? targetRotationAngle : 0f;
        
        // Smoothly interpolate to target angle
        currentRotationAngle = Mathf.Lerp(currentRotationAngle, targetAngle, Time.fixedDeltaTime * rotationSpeed);
        
        // Apply rotation relative to initial rotation (inverted direction)
        Wing1.transform.localRotation = wing1InitialRotation * Quaternion.Euler(0f, 0f, -currentRotationAngle);
        Wing2.transform.localRotation = wing2InitialRotation * Quaternion.Euler(0f, 0f, -currentRotationAngle);
    }
}
