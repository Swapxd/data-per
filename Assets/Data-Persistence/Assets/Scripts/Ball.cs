using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private float minVelocity = 1.0f; // Minimum velocity to keep ball moving
    private float maxVelocity = 3.0f; // Maximum velocity to control speed

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        m_Rigidbody.useGravity = false; // Disable gravity
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle the reflection of the ball based on the surface it hits
        ReflectBall(collision);
    }

    private void ReflectBall(Collision collision)
    {
        // Get the ball's current velocity
        Vector3 incomingVelocity = m_Rigidbody.velocity;

        // Get the normal of the surface the ball collided with
        Vector3 surfaceNormal = collision.contacts[0].normal;

        // Reflect the incoming velocity based on the surface's normal
        Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, surfaceNormal);

        // Ensure the ball maintains minimum speed to prevent it from floating or slowing down
        if (reflectedVelocity.magnitude < minVelocity)
        {
            reflectedVelocity = reflectedVelocity.normalized * minVelocity;
        }

        // Limit the ball's maximum velocity to avoid excessive speed
        if (reflectedVelocity.magnitude > maxVelocity)
        {
            reflectedVelocity = reflectedVelocity.normalized * maxVelocity;
        }

        // Apply the reflected velocity to the ball's Rigidbody
        m_Rigidbody.velocity = reflectedVelocity;
    }
}
