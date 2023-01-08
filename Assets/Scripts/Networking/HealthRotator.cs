using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HealthRotator : MonoBehaviour
{
    GameObject localPlayer;
    Slider healthSlider;

    Quaternion initialOffset;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        initialOffset = healthSlider.transform.rotation;

        localPlayer = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = localPlayer.transform.position + new Vector3(0f, healthSlider.transform.position.y - localPlayer.transform.position.y, 0f) - healthSlider.transform.position;
        healthSlider.transform.rotation = Quaternion.Euler(0f, Vector3.Angle(healthSlider.transform.forward, lookDirection), 0f);
    }
}
