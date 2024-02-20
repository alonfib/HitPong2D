using UnityEngine;

public class BatScript : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation, adjust as needed
    public float scalingFactor = 0.1f; // Adjust this factor to scale the raycast length change based on rotation speed
    public float maxRotationSpeed = 200f; // Maximum rotation speed, adjust as needed
    public bool isDevelopment = false;

    private Vector2 startTouchPosition;
    private bool isDragging = false;
    private float rotationAmount = -25f; // Current rotation amount based on input
    private float maxRotation = 180f;
    private Rigidbody2D rb; // Reference to the Rigidbody2D
    private Vector2 previousPosition;
    private Vector3 previousRotation;
    private bool disabled = false;

    void Start()
    {
        if(isDevelopment)
        {
            rotationSpeed = 250f;
            maxRotationSpeed = 600f;
        }
        rb = GetComponent<Rigidbody2D>();
        previousRotation = transform.eulerAngles;
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the bat GameObject.");
        }
    }

    void Update()
    {
        HandleTouchInput();
    }

    void FixedUpdate()
    {
        SmoothlyRotateBat();

        // Calculate the rotation speed of the bat
        float rotationSpeed = Mathf.Abs((transform.eulerAngles - previousRotation).magnitude) / Time.fixedDeltaTime;
        previousRotation = transform.eulerAngles;

        // Adjust the raycast length based on rotation speed
        Vector2 direction = (rb.position - previousPosition).normalized;
        float raycastLength = Vector2.Distance(previousPosition, rb.position) + rotationSpeed * scalingFactor;

        RaycastHit2D hit = Physics2D.Raycast(previousPosition, direction, raycastLength);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ball")) // Make sure your ball has the tag "Ball"
        {
            // Handle the collision
            disabled = true;
            HandleCollision(hit);
        }

        previousPosition = rb.position;
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        float targetRotation = rotationAmount + CalculateRotationChange();
                        // Clamp the target rotation to enforce maximum rotation speed
                        float rotationDelta = Mathf.Clamp(targetRotation - rotationAmount, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);
                        rotationAmount += rotationDelta;
                        startTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }

    private float CalculateRotationChange()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float deltaPosition = touch.position.y - startTouchPosition.y;
            float rotationChange = deltaPosition / Screen.height * maxRotation;
            return Mathf.Clamp(rotationChange, -maxRotation, maxRotation);
        }
        return 0f;
    }

    private void SmoothlyRotateBat()
    {
        if (isDragging && !disabled)
        {
            // Apply the rotation through Rigidbody
            if (rb != null)
            {
                rb.MoveRotation(rotationAmount);
            }
            else
            {
                // Apply the rotation directly if there's no Rigidbody2D
                transform.rotation = Quaternion.Euler(0, 0, rotationAmount);
            }
        }
    }

    private void HandleCollision(RaycastHit2D hit)
    {
        // Reflect the ball's velocity based on the hit normal and some restitution factor
        Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, hit.normal);
        //rb.velocity = reflectedVelocity * 0.7f; // Adjust 0.9f for restitution effect
        rb.velocity = reflectedVelocity; // Adjust 0.9f for restitution effect
        // Optionally, reposition the ball to the hit point for a more accurate bounce effect
        rb.position = hit.point;
    }
}
