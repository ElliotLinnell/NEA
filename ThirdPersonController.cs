using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float runSpeed = 6f;
    public float rotationSpeed = 720f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public float lookSpeed = 2f;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    private float speed;
    private bool isGrounded;
    public bool isAttacking;
    private float yaw = 0f;
    private float pitch = 0f;
    private PlayerHealth playerHealth;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } //Gets the components and hides the cursor

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleAttacking();
    } //Updates the player's position, rotation, and checks for attacking

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * lookSpeed;
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        gameObject.transform.eulerAngles = new Vector3(0f, yaw, 0f);
    } //Handles the player's rotation based on mouse input

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        } //Resets the player's vertical velocity when grounded

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            bool isSprinting = Input.GetKey(KeyCode.LeftShift) && playerHealth.CanUseStamina(0.1f);
            speed = isSprinting ? runSpeed : moveSpeed;

            float cameraYaw = cinemachineVirtualCamera.transform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraYaw;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref speed, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);

            if (isSprinting)
            {
                playerHealth.UseStamina(playerHealth.staminaConsumptionRate * Time.deltaTime);
            } //Handles sprinting and stamina consumption
        } //Handles player movement
        else
        {
            speed = 0f;
        } //Stops the player if no input is detected

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);

        Vector3 currentPosition = transform.position;
        currentPosition.y = 0f;
        transform.position = currentPosition;
    } //Handles the player's movement based on input

    private void HandleAttacking()
    {
        if (Input.GetMouseButtonDown(0) && playerHealth.CanUseStamina(playerHealth.staminaConsumptionRate))
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", isAttacking);
            playerHealth.UseStamina(playerHealth.staminaConsumptionRate);
        } //Handles attacking and stamina consumption
        else if (Input.GetMouseButtonUp(0))
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", isAttacking);
        } //Stops attacking when the mouse button is released
    }
}
