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
    private Vector2 rawInputVector;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    // Start is called before the first frame update
    void Awake()
    {
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
        playerAnimator.SetFloat("xValue",currentInputVector.x);
        playerAnimator.SetFloat("yValue", currentInputVector.y);
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
