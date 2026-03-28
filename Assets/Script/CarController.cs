using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    // Car Attibutes
    [Header("Movement Settings")]
    public float forwardSpeed = 15f;
    public float boostSpeed = 30f;
    public float accelerationRate = 3f;
    private float currentForwardSpeed;

    public float sideSpeed = 10f;
    public float maxBoundaryX = 9f;

    [Header("Physics Leaning (Torque)")]
    public float leanTorqueForce = 50f;     
    public float restoreSpringForce = 10f;  
    public float dampingForce = 5f;         

    private Rigidbody rb;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularDamping = 0f;

        // Start Speed
        currentForwardSpeed = forwardSpeed;
    }

    void Update()
    {
        // CarMovement
        horizontalInput = 0f;
        bool isBoosting = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput = 1f;

            if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            {
                isBoosting = true;
            }
        }

        float targetSpeed = isBoosting ? boostSpeed : forwardSpeed;

        // Accelation increase speed to max!
        currentForwardSpeed = Mathf.Lerp(currentForwardSpeed, targetSpeed, Time.deltaTime * accelerationRate);

        Vector3 moveDirection = new Vector3(horizontalInput * sideSpeed, 0, currentForwardSpeed);
        transform.Translate(moveDirection * Time.deltaTime, Space.World);

        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -maxBoundaryX, maxBoundaryX);
        transform.position = currentPos;
    }

    void FixedUpdate()
    {
        // CarPhysic
        float appliedTorque = -horizontalInput * leanTorqueForce;
        rb.AddTorque(Vector3.forward * appliedTorque);

        float currentAngleZ = rb.rotation.eulerAngles.z;
        if (currentAngleZ > 180f) currentAngleZ -= 360f; 

        float restoreTorque = -currentAngleZ * restoreSpringForce;
        rb.AddTorque(Vector3.forward * restoreTorque);

        float angularVelZ = rb.angularVelocity.z;
        float dampingTorque = -angularVelZ * dampingForce;
        rb.AddTorque(Vector3.forward * dampingTorque);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {   
            SceneManager.LoadScene("GameOver");
        }
    }
} 