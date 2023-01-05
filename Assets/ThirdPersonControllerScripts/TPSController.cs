using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;

public class TPSController : MonoBehaviour
{
    public static TPSController instance;
    

    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSenstivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] float pushPower;

    private Transform hitTransform;

    //Camera
    [SerializeField] public Vector3 mouseWorldPosition;
    [SerializeField] public Vector2 screenCentrePoint;
    public bool canMoveCamera = true;

    [SerializeField] private Transform muzzleFlashSpawnPoint;
    public ParticleSystem[] muzzleFlash;

    private ThirdPersonController tpsController;
    private StarterAssetsInputs starterAssetInputs;

    private Animator anim;

    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    private Gun gun;

    private void Awake()
    {
        instance = this;

        tpsController = GetComponent<ThirdPersonController>();
        starterAssetInputs = GetComponent<StarterAssetsInputs>();
        
        anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
        gun = GetComponentInChildren<Gun>();

        //Update character rotation based on camera
        mouseWorldPosition = Vector3.zero;

        screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
        hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycasthit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycasthit.point;
            hitTransform = raycasthit.transform;

            Aim();
        }

    }

    public void Aim()
    {

        if (starterAssetInputs.aim)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);

            aimCamera.gameObject.SetActive(true);
            tpsController.SetSensitivity(aimSenstivity);
            tpsController.SetRotateOnMove(false);
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            tpsController.isAiming = true;

            gun.Input();

        }

        else
        {
            aimCamera.gameObject.SetActive(false);
            tpsController.SetSensitivity(normalSensitivity); 
            tpsController.SetRotateOnMove(true);
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            tpsController.isAiming = false;

        }
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
