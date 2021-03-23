using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSmoothing = 0.1f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float jumpAmount = 3f;
    [SerializeField] private Transform cam;
    
    private Vector3 movement;
    private CharacterController myController;
    private float rotateVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.y -= gravity * Time.deltaTime;

        Vector3 moveV = new Vector3(0f, movement.y, 0f);

        myController.Move(moveV * Time.deltaTime);
        
        if (movement.x == 0f && movement.z == 0f) return;

        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateVelocity, rotationSmoothing);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        myController.Move(moveDir * (speed * Time.deltaTime));
    }

    public void Move(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (myController.isGrounded)
        {
            movement.y = jumpAmount;
        }
    }
}
