using UnityEngine;

public class BallArrowScript : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Speed of rotation
    public bool shouldRotate = true;

    private float minRotation = -90.0f; // Minimum rotation angle
    private float maxRotation = 0.0f; // Maximum rotation angle
    private bool rotatingDownward = true;


    void Start()
    {
        // Set initial rotation to 0 degrees
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if(shouldRotate)
        {
            HandleArrowRotation();
        }
    }

    private void HandleArrowRotation()
    {
        // Determine the current rotation and direction of rotation
        float currentRotation = transform.eulerAngles.z;
        currentRotation = (currentRotation > 180) ? currentRotation - 360 : currentRotation; // Adjusting for Unity's rotation representation

        if (rotatingDownward)
        {
            // Rotate downwards
            if (currentRotation > minRotation)
            {
                RotateArrow(-rotationSpeed);
            }
            else
            {
                rotatingDownward = false;
            }
        }
        else
        {
            // Rotate upwards
            if (currentRotation < maxRotation)
            {
                RotateArrow(rotationSpeed);
            }
            else
            {
                rotatingDownward = true;
            }
        }
    }

    private void RotateArrow(float speed)
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetShouldRotate(bool isRotate)
    {
        shouldRotate = isRotate;
    }

    public void HideArrow(bool isHidden = true)
    {
        if(!isHidden)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        gameObject.SetActive(!isHidden);
        shouldRotate = !isHidden;


    }
}
