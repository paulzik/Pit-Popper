using UnityEngine;

/// <summary>
/// Moves the ball and handles the collision with walls and other balls.
/// </summary>
public class BallMover : MonoBehaviour
{
    /// <summary>
    /// Speed of the ball movement.
    /// </summary>
    public float speed = 5f;

    private Vector3 direction;

    private void Start()
    {
        direction = transform.forward;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Ball"))
        {
            Vector3 wallNormal = other.transform.forward;
            direction = Vector3.Reflect(direction, wallNormal).normalized;

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
