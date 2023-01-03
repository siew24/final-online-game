using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    [SerializeField] Transform followTransform;

    [SerializeField] int speed;
    [SerializeField] int runSpeed;
    [SerializeField] int jumpVelocity;
    [SerializeField] float pushPower;
    [SerializeField] float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;
    float previousYVelocity;

    float groundedGravity = .5f;

    PlayerInput playerInput;
    CharacterController characterController;

    Vector2 _move;
    bool isJumping;
    bool isJumpPressed;
    bool isRunPressed;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _move.y + right * _move.x;
        float targetAngle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveDirection += Physics.gravity;

        if (!isRunPressed)
            characterController.Move(moveDirection * speed * Time.deltaTime);
        else
            characterController.Move(moveDirection * runSpeed * Time.deltaTime);
    }

    void OnMove(InputValue inputValue)
    {
        _move = inputValue.Get<Vector2>();

    }

    void OnJump(InputValue inputValue)
    {
        isJumpPressed = true;
    }

    void OnRun(InputValue inputValue)
    {
        isRunPressed = inputValue.Get<float>() > 0;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // No rigidbody
        if (body == null || body.isKinematic) return;

        // Do not push objects below us
        if (hit.moveDirection.y < -0.3) return;

        // Push objects to the sides never up and down
        Vector3 pushDirection = new(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Scale object's pushed distance with the player's move speed
        body.velocity = pushDirection * pushPower;
    }
}
