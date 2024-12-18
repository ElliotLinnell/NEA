using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float verticalLookLimit = 80f;
    public float smoothTime = 0.1f; // Time for smoothing

    private float xRotation = 0f;
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentRotation = transform.eulerAngles;
    } // Locks the cursor and hides it

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float targetRotationY = transform.eulerAngles.y + mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        Vector3 targetRotation = new Vector3(xRotation, targetRotationY, 0f);

        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationVelocity, smoothTime);
        transform.eulerAngles = currentRotation;
    } // Updates the camera's rotation based on mouse input
}
