
using UnityEngine;
using UnityEngine.EventSystems;

public class BallScript : MonoBehaviour
{
    public LogicManager logic;
    public GameObject arrow;
    public BallArrowScript arrowScript;

    public float maxThrowForce = 5f;
    public float forceChargeRate = 0.5f;
    public float stationaryThreshold = 0.1f; // Velocity below which the ball is considered stationary
    public float maxStationaryTime = 3.0f;
    public float startX = 3.6f;
    public float startY = 3.5f;

    private Rigidbody2D Rigidbody2D;
    private Vector2 previousPosition;
    private float currentThrowForce;
    private bool isChargingThrow;
    private bool isThrown = false;
    private float stationaryTime = 0f;

    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    public bool isFrozen = false;


    void Start()
    {
        Taptic.tapticOn = true;

        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManager>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Rigidbody2D == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject.");
        }
        else
        {
            resetBall();
        }
        previousPosition = Rigidbody2D.position;
        ResetThrowForce();
    }

    void Update()
    {
        handleGameOver();
        HandleTouch();
    }

    void FixedUpdate()
    {
        if (isThrown)
        {
            RaycastHit2D hit = Physics2D.Raycast(previousPosition, (Rigidbody2D.position - (previousPosition * 2)).normalized, Vector2.Distance(previousPosition, Rigidbody2D.position));
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Paddle")) // Make sure your paddle has the tag "Paddle"
            {
                // Handle the collision
                HandleCollision(hit);
            }

            previousPosition = Rigidbody2D.position;
        }
    }

    public void FreezeBall(bool frozen)
    {
        isFrozen = frozen;
        Rigidbody2D.isKinematic = frozen;

        if (isFrozen)
        {
            // Save current velocity and angular velocity
            savedVelocity = Rigidbody2D.velocity;
            savedAngularVelocity = Rigidbody2D.angularVelocity;

            // Stop the ball's movement
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.angularVelocity = 0;
        } else
        {
            // Restore the ball's velocity and angular velocity
            Rigidbody2D.velocity = savedVelocity;
            Rigidbody2D.angularVelocity = savedAngularVelocity;
        }
    }

    public void resetBall()
    {
        // Reset Rigidbody2D properties to stop movement
        Rigidbody2D.velocity = Vector2.zero; // Resets linear velocity
        Rigidbody2D.angularVelocity = 0f;    // Resets angular velocity

        // Reset other properties as before
        Rigidbody2D.gravityScale = 0;
        isThrown = false;
        arrowScript.HideArrow(false);

        // Reset position to the start position
        transform.position = new Vector3(startX, startY, 2);
        previousPosition = new Vector3(startX, startY, 2);
    }

    private void HandleTouch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return; // Exit the method if the touch is on UI
            }

            if (!isThrown)
            {
                if (touch.phase == TouchPhase.Began && arrow != null) // For touch use Input.GetTouch(0).phase == TouchPhase.Began
                {
                    isChargingThrow = true;
                    arrowScript.SetShouldRotate(false);
                }

                if (isChargingThrow)
                {
                    currentThrowForce = Mathf.Min(currentThrowForce + forceChargeRate * Time.deltaTime, maxThrowForce);
                    UpdateArrowColor((currentThrowForce / maxThrowForce) * 3);
                }

                if (touch.phase == TouchPhase.Ended && isChargingThrow)
                {
                    if (!isThrown)
                    {
                        ThrowBall();
                    }
                    ResetThrowForce();
                    arrowScript.SetShouldRotate(true);
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float magnitude = Rigidbody2D.velocity.magnitude;
        if (magnitude > 1f) // Adjust 0.1f threshold as needed
        {
            // Trigger haptic feedback only if the object is moving
            Taptic.Heavy();
        }
        if (magnitude > 0.4f && magnitude <= 1f) // Adjust 0.1f threshold as needed
        {
            // Trigger haptic feedback only if the object is moving
            Taptic.Medium();
        }
        if (magnitude <= 0.4f && magnitude > 2f) // Adjust 0.1f threshold as needed
        {
            Taptic.Light();
        }

        if (collision.collider.CompareTag("Paddle"))
        {
            logic.TouchedPaddle();
        }
    }

    private bool IsBallOnScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        return screenPosition.x > 0 && screenPosition.x < 1 && screenPosition.y > 0;
    }

    private void ThrowBall()
    {
        Rigidbody2D.gravityScale = 3;

        float angle = arrow.transform.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        Vector2 throwDirection = Quaternion.Euler(0, 0, angle) * Vector2.left;
        Vector2 normalizedDirection = throwDirection.normalized * currentThrowForce;

        if (Rigidbody2D != null)
        {
            Rigidbody2D.AddForce(normalizedDirection, ForceMode2D.Impulse);
            isThrown = true;
            arrowScript.HideArrow();
        }
    }

    private void HandleCollision(RaycastHit2D hit)
    {
        // Reflect the ball's velocity based on the hit normal and some restitution factor
        Vector2 reflectedVelocity = Vector2.Reflect(Rigidbody2D.velocity, hit.normal);
        Rigidbody2D.velocity = reflectedVelocity * 0.9f; // Adjust 0.9f for restitution effect
        // Optionally, reposition the ball to the hit point for a more accurate bounce effect
        Rigidbody2D.position = hit.point;
    }

    private void ResetThrowForce()
    {
        currentThrowForce = 0;
        isChargingThrow = false;
        UpdateArrowColor(0f); // Reset arrow color
    }

    private void UpdateArrowColor(float strengthRatio)
    {
        // Update the color of the arrow based on the strength ratio
        Color arrowColor = Color.Lerp(Color.white, Color.red, strengthRatio);
        arrow.GetComponent<SpriteRenderer>().color = arrowColor;
    }

    void handleStationary()
    {
        if (Rigidbody2D.velocity.magnitude < stationaryThreshold)
        {
            stationaryTime += Time.deltaTime;
            if (stationaryTime >= maxStationaryTime)
            {
                logic.GameOver();
            }
        }
        else
        {
            stationaryTime = 0f;
        }

    }

    void handleGameOver()
    {
        if(!isFrozen && !logic.gameWinScreen.activeInHierarchy && !logic.FinishScreen.activeInHierarchy)
        {
            if (!IsBallOnScreen())
            {
                logic.GameOver();
            }

            if (isThrown)
            {
                handleStationary();
            }
        }
    }
}
