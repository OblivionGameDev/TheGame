using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionScript : MonoBehaviour
{
    [HideInInspector] public Animator playerAnimator;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
