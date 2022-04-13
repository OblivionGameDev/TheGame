using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionScript : MonoBehaviour
{
    [HideInInspector] public Animator playerAnimator;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    private Cinemachine.CinemachineInputProvider inputAxisProvider;
    public Transform cameraLookAt;

    [SerializeField] private float smoothInputSpeed = 0.1f;
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
    private bool isRunning;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls.Player.Run.performed += ctx => RunBool();
        playerControls = new PlayerControls();   
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
        inputAxisProvider = GetComponent<Cinemachine.CinemachineInputProvider>();
        xAxis.SetInputAxisProvider(0, inputAxisProvider);
        yAxis.SetInputAxisProvider(1, inputAxisProvider);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);

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
    }

    void RunBool()
    {
        if (!isRunning)
        {
            isRunning = true;
            Debug.Log("True");
        }
        else
        {
            isRunning = false;
            Debug.Log("False");
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
}
