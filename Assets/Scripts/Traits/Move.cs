using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    [SerializeField] Transform forwardFacingTransform;

    [SerializeField] int speed;

    PlayerInput playerInput;
    new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Vector2 direction = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontalInput = direction.x;
        float verticalInput = direction.y;

        Vector3 right = forwardFacingTransform.right;
        Vector3 forward = Vector3.Cross(right, Vector3.up);

        Vector3 direction3D = right * horizontalInput + forward * verticalInput;

        rigidbody.AddForce(direction3D * speed, ForceMode.Acceleration);

        float yDirection = rigidbody.velocity.y;
        Vector3 newVelocity = Vector3.ClampMagnitude(rigidbody.velocity, speed);

        rigidbody.velocity = new(newVelocity.x, yDirection, newVelocity.z);
    }
}
