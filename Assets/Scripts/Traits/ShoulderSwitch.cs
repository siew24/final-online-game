using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShoulderSwitch : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Shoulder Camera")]
    [SerializeField] CinemachineVirtualCamera[] cinemachineVirtualCameras;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cinemachineVirtualCameras[0].gameObject.SetActive(false);
        cinemachineVirtualCameras[1].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnShoulderSwitch()
    {
        cinemachineVirtualCameras[0].gameObject.SetActive(!cinemachineVirtualCameras[0].gameObject.activeInHierarchy);
        cinemachineVirtualCameras[1].gameObject.SetActive(!cinemachineVirtualCameras[1].gameObject.activeInHierarchy);
    }
}
