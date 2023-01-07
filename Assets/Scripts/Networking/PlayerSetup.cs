using System.Collections;
using System.Collections.Generic;
using cakeslice;
using Photon.Pun;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] new Camera camera;
    [SerializeField] GameObject cameraRigs;
    new PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {

        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            camera.gameObject.SetActive(false);
            cameraRigs.SetActive(false);

            GetComponent<CharacterController>().enabled = false;
            GetComponent<ThirdPersonController>().enabled = false;
            GetComponent<BasicRigidBodyPush>().enabled = false;
            GetComponent<StarterAssetsInputs>().enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<TPSController>().enabled = false;
            GetComponent<Interact>().enabled = false;
            GetComponent<ShoulderSwitch>().enabled = false;
            GetComponent<GameControls>().enabled = false;
            return;
        }

        OutlineEffect outlineEffect = camera.AddComponent<OutlineEffect>();
        outlineEffect.lineThickness = 1.25f;
        outlineEffect.lineIntensity = .5f;
        outlineEffect.fillAmount = 0.2f;
        outlineEffect.lineColor0 = new(255, 0, 0, 255);
        outlineEffect.lineColor1 = new(0, 255, 0, 255);
        outlineEffect.lineColor2 = new(0, 0, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
