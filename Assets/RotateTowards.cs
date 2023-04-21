using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float maxRotationAngle = 45f;

    private Rigidbody rb;
    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void RotateToward(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
    }

    private void Update()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, maxRotationAngle * rotationSpeed * Time.deltaTime);
        rb.MoveRotation(newRotation);
    }
}