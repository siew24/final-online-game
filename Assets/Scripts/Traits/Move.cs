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
    [SerializeField] float pushPower;
    [SerializeField] float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    PlayerInput playerInput;
    CharacterController characterController;

    Vector2 _move;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector2 direction = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontalInput = direction.x;
        float verticalInput = direction.y;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        */
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

        characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    void OnMove(InputValue inputValue)
    {
        _move = inputValue.Get<Vector2>();
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
