using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionScript : MonoBehaviour
{
    public float gravity;
    public float stepDown;
    public float pushPower;
    [HideInInspector] public Animator playerAnimator;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    private Cinemachine.CinemachineInputProvider inputAxisProvider;
    public Transform cameraLookAt;

    [SerializeField] private float smoothInputSpeed = 0.1f;
    private CharacterController controller;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    private Camera mainCamera; 
    private PlayerControls playerControls;
    private float turnSpeed = 15f;
    private float xValue;
    private float yValue;
    private float walkMin = -0.5f;
    private float walkMax = 0.5f;
    private float runMin = -1f;
    private float runMax = 1f;
    private float newValueX;
    private float newValueY;
    private Vector2 rawInputVector;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private bool isRunning;
    private bool playerIsInAir;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerWeaponSwitchScript = GetComponent<PlayerWeaponSwitchScript>();
        playerControls = new PlayerControls();   
        playerControls.Player.Run.performed += ctx => RunBool();
        playerControls.Player.Crouch.performed += ctx => CrouchingBool();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
        inputAxisProvider = GetComponent<Cinemachine.CinemachineInputProvider>();
        xAxis.SetInputAxisProvider(0, inputAxisProvider);
        yAxis.SetInputAxisProvider(1, inputAxisProvider);
    }
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        if (playerAnimator.GetBool("isCrouching"))
        {
            controller.height = 1.25f;
            controller.center = new Vector3(0, 0.65f, 0);   
            playerWeaponSwitchScript.assaultRiffleInHandLayer.weight = 0f;
            playerWeaponSwitchScript.assaultRiffleWeaponPoseLayer.weight = 0f;
            playerWeaponSwitchScript.assaultRiffleAimLayer.weight = 0f;
            playerWeaponSwitchScript.pistolInHandLayer.weight = 0f;
            playerWeaponSwitchScript.pistolAimLayer.weight = 0f;
            playerWeaponSwitchScript.bodyAimLayer.weight = 0f;
        }
        else if (!playerAnimator.GetBool("isCrouching"))
        {
            playerWeaponSwitchScript.bodyAimLayer.weight = 1f;
            controller.height = 1.85f;
            controller.center = new Vector3(0, 0.92f, 0);   
        }
    }

    void OnAnimatorMove()
    {
        rootMotion += playerAnimator.deltaPosition;
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        rawInputVector = playerControls.Player.Move.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, rawInputVector, ref smoothInputVelocity, smoothInputSpeed);
        
        if (isRunning)
        {
            newValueX = Mathf.Clamp(currentInputVector.x, runMin, runMax);
            newValueY = Mathf.Clamp(currentInputVector.y, runMin, runMax);
        } 
        else
        {
            newValueX = Mathf.Clamp(currentInputVector.x, walkMin, walkMax);
            newValueY = Mathf.Clamp(currentInputVector.y, walkMin, walkMax);
        }
        playerAnimator.SetFloat("xValue", newValueX);
        playerAnimator.SetFloat("yValue", newValueY);
        
        if (playerIsInAir)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            controller.Move(velocity * Time.fixedDeltaTime);
            playerIsInAir = !controller.isGrounded;
            rootMotion = Vector3.zero;
        }
        else 
        {
            controller.Move(rootMotion + Vector3.down * stepDown);
            rootMotion = Vector3.zero;
            if (!controller.isGrounded)
            {
                playerIsInAir = true;
                velocity = playerAnimator.velocity;
                velocity.y = 0f;
            }
        }
    
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);



    }

    void RunBool()
    {
        if (!isRunning)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    void CrouchingBool()
    {
        if (playerAnimator.GetBool("isCrouching"))
        {
            playerAnimator.SetBool("isCrouching", false);
        }
        else if (!playerAnimator.GetBool("isCrouching"))
        {
            playerAnimator.SetBool("isCrouching", true);
        }
    }
    void OnEnable()
    {
        playerControls.Enable();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
