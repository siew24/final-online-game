using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AnimateGOTrigger : MonoBehaviourPun
{
    new PhotonView photonView;

    // Public vars
    [Header("[Source]")]
    public GameObject GoTarget;

    [Header("[AnimGO Settings]")]
    public bool Locked = false;
    public bool AutoAnim = false;

    [Header("Events")]
    public OnNotification onNotification;

    // Private vars
    [HideInInspector]
    public KeyCode UnlockKey;
    private Collider _activator;
    private bool _iscolliding = false;
    [HideInInspector]
    public Animator animator;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start()
    {
        // make this object invisible
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        // set animator
        animator = GoTarget.GetComponent<Animator>();
        // set default anim values
        //animator.SetBool("Opened", true);
    }

    // When any collider hits the trigger.
    void OnTriggerEnter(Collider trig)
    {
        // check if Key pressed and collider hit was from correct target
        if (trig.CompareTag("Player"))
        {
            // set the door ready to move
            //photonView.RPC(nameof(NetworkedSetIsColliding), RpcTarget.AllBuffered, true);
            _iscolliding = true;

            // If door can't be moved
            if (Locked)
                onNotification.Raise("Door Locked. Explore more");
        }
        // AUTOANIM
        if (AutoAnim && (!Locked))
        {
            animator.SetBool("Opened", false);
            animator.SetTrigger("Actived");
        }
    }
    void OnTriggerExit(Collider trig)
    {
        // set the door out of reach
        _iscolliding = false;
        //debug
        Debug.Log(trig.name + "has exit the activator trigger");

        //AUTOANIM
        if (AutoAnim && (!Locked))
        {
            animator.SetTrigger("Actived");
            animator.SetBool("Opened", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check if ready to move
        if (_iscolliding)
        {
            // Set movement on when Key pressed
            if ((Input.GetKeyUp(UnlockKey)) && (!Locked) && (!AutoAnim))
            {
                animator.SetTrigger("Actived");
                if (animator.GetBool("Opened") == false)
                {
                    animator.SetBool("Opened", true);
                }
                else
                {
                    animator.SetBool("Opened", false);
                }
            }
        }
    }

    public void SetDoorLock(bool value)
    {
        //photonView.RPC(nameof(NetworkedSetDoorLock), RpcTarget.AllBuffered, value);
        Locked = value;
    }

    [PunRPC]
    void NetworkedSetDoorLock(bool value)
    {
        Locked = value;
    }

    [PunRPC]
    void NetworkedSetIsColliding(bool value)
    {
        _iscolliding = value;
    }
}
