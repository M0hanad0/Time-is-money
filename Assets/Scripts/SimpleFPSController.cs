using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{

    [SerializeField] private float speed = 10;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float runMultiplier = 2;
    [SerializeField, Range(0f, 90f)] private float jumpSlopeLimit;

    private CharacterController charController;
    private float jumpMult;
    private float yVelocity;
    private float originalSlopeLimit;
    private bool isGrounded;

    // Mouse look
    private float xRotation = 0f;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform mainCam;
    private TimeManager _timeManager;

    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        charController = GetComponent<CharacterController>();
    }

    private void Start()
    {

        originalSlopeLimit = charController.slopeLimit;
        jumpMult = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Hide mouse and lock to screen center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        // Allow player control of mouse sensitivity
        if (Input.GetKeyDown(KeyCode.Equals)) mouseSensitivity = Mathf.Clamp(mouseSensitivity + 20, 20f, 1000f);
        if (Input.GetKeyDown(KeyCode.Minus)) mouseSensitivity = Mathf.Clamp(mouseSensitivity - 20, 20f, 1000f);

        PlayerMove();
    }

    // Do camera rotation in LateUpdate, or you get stutter
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 70f); // Can't look too far up or down

        mainCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    private void PlayerMove()
    {
        isGrounded = charController.isGrounded;
        if (charController.isGrounded || charController.collisionFlags == CollisionFlags.Above) yVelocity = -0.1f;

        if (charController.isGrounded)
        {
            charController.slopeLimit = originalSlopeLimit;
        }
        else
        {
            charController.slopeLimit = jumpSlopeLimit;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        move = speed * Time.deltaTime * move;

        if (Input.GetKey(KeyCode.LeftShift)) move *= runMultiplier;

        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            yVelocity += jumpMult;
        }

        yVelocity += gravity * Time.deltaTime;

        move.y = yVelocity * Time.deltaTime;

        if (move == Vector3.zero)
        {
            _timeManager.SlowMotion();
        } else
        {
            _timeManager.ResetTime();
        }

        charController.Move(move);
    }


}