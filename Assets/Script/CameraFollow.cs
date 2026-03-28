using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Attibutes
    [Header("Target & Speed")]
    public Transform target;       
    public float followSpeed = 10f; 

    [Header("Dynamic Panning")]
    public float panMultiplier = 0.5f; 

    private Vector3 initialOffset;
    private Quaternion initialRotation;

    void Start()
    {
        // Distance
        if (target != null)
        {
            initialOffset = transform.position - target.position;
            initialRotation = transform.rotation;
        }
    }


    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);


        float carAngleZ = target.eulerAngles.z;
        if (carAngleZ > 180f) carAngleZ -= 360f; 


        Quaternion turnOffset = Quaternion.Euler(0, -carAngleZ * panMultiplier, 0);
        Quaternion targetRotation = initialRotation * turnOffset;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}