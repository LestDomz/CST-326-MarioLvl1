using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2025 : MonoBehaviour
{
    public float acceleration = 10f; // Increased for responsiveness
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public bool isGrounded;
    public float jumpForceBoost = 8f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent rotation
    }

    void Update()
    {
        float horizontalAmount = Input.GetAxis("Horizontal");

        // Apply movement force only if the character isn't at max speed
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(Vector3.right * horizontalAmount * acceleration, ForceMode.Acceleration);
        }

        // Ground detection
        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y + 0.01f; // Fixed ground check height

        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        Color color = (isGrounded) ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);

        // Jumping mechanics
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.up * jumpForceBoost, ForceMode.Acceleration);
            }
        }

        // Apply friction when no movement input is given
        if (horizontalAmount == 0f && isGrounded)
        {
            Vector3 decayedVelocity = rb.velocity;
            decayedVelocity.x *= 1f - Time.deltaTime * 4f; // Smooth stopping
            rb.velocity = decayedVelocity;
        }

        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
    }
}
