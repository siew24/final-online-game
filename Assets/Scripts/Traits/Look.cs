using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    [SerializeField] OnLook onLook;
    [SerializeField] Transform followTransform;

    [Range(0f, 1f)]
    [SerializeField] float mouseSensitivity = 1f;

    [SerializeField] bool lockCursor = true;

    [Header("Shoulder Camera")]
    [SerializeField] CinemachineVirtualCamera[] cinemachineVirtualCameras;

    Vector3 targetDirection;

    void Start()
    {
        Camera.main.transform.position = followTransform.position;

        Cursor.visible = !lockCursor;

        // Set target direction to the camera's initial orientation.
        targetDirection = followTransform.rotation.eulerAngles;
    }

    void OnEscape()
    {
        lockCursor = !lockCursor;

        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;


        if (Cursor.lockState == CursorLockMode.Locked)
        {
            onLook.Raise();

            GetComponent<PlayerInput>().actions["Look"].Enable();
            return;
        }

        GetComponent<PlayerInput>().actions["Look"].Disable();
    }

    void Update()
    {
        followTransform.position = transform.position + new Vector3(0f, 0.5f, 0f);
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mouseDelta = inputValue.Get<Vector2>();

        // transform.rotation *= Quaternion.AngleAxis(mouseDelta.x, Vector3.up);

        followTransform.rotation *= Quaternion.AngleAxis(mouseDelta.x, Vector3.up);

        followTransform.rotation *= Quaternion.AngleAxis(-mouseDelta.y, Vector3.right);

        Vector3 angles = followTransform.localEulerAngles;
        angles.z = 0;

        if (angles.x > 180 && angles.x < 340)
        {
            angles.x = 340;
        }
        else if (angles.x < 180 && angles.x > 40)
        {
            angles.x = 40;
        }

        followTransform.localEulerAngles = angles;

        Quaternion targetRotation = Quaternion.Euler(0, followTransform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);

        onLook.Raise();
    }

    void OnShoulderSwitch()
    {
        cinemachineVirtualCameras[0].gameObject.SetActive(!cinemachineVirtualCameras[0].gameObject.activeInHierarchy);
        cinemachineVirtualCameras[1].gameObject.SetActive(!cinemachineVirtualCameras[1].gameObject.activeInHierarchy);
    }
}