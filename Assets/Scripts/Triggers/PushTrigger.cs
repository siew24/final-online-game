// TOPDOWN CONTROL SCRIPTS 1.0 www.dlnkworks.com 2016(c) This script is licensed as free content in the pack. Support is not granted while this is not part of the art pack core. Is licensed for commercial purposes while not for resell.

using UnityEngine;

using Photon.Pun;

// TODO: Attach PhotonView on all PushTriggers for Pun.RPC
public class PushTrigger : MonoBehaviour//Pun
{
    //new PhotonView photonView;

    // Public vars
    [Header("[Source]")]
    public Transform DoorHinge;
    [Header("[Door Settings]")]
    public bool opened;
    public float Percentage = 100;
    public bool auto = false;
    [Header("[Movement Settings]")]
    public Vector3 XYZDisplacement;
    public Quaternion Rotation = new Quaternion(0f, 90f, 0f, 100f);
    public float Duration;
    [Header("[Area Door Settings]")]
    public bool isAreaDoor = false;
    public OnNotification onNotification;



    // Private vars
    [HideInInspector]
    private bool _iscolliding = false;
    private bool _ismoving = false;
    private bool _islocked = false;
    private Collider _activator;
    private float _percentage = 0;
    private float _timer = 0;

    void Awake()
    {
        //photonView = GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start()
    {
        _islocked = isAreaDoor;

        // make this object invisible
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (opened)
        {
            DoorHinge.transform.localPosition = XYZDisplacement;
            //      DoorHinge.rotation= new Quaternion(Rotation.x, Rotation.y,Rotation.z, Rotation.w);
            _percentage = 100;
        }
    }

    // When any collider hits the trigger.
    void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player"))
        {
            // set the door ready to move
            SetIsColliding(true);

            if (_islocked)
                onNotification.Raise("Door Locked. Explore more");
        }
    }
    void OnTriggerExit(Collider trig)
    {
        // set the door out of reach
        _iscolliding = false;
        //debug
        Debug.Log(trig.name + "has exit the door trigger");
    }

    // Update is called once per frame
    void Update()
    {
        // check if ready to move and not moving and not opened 
        if (_iscolliding && !_ismoving && !opened)
        {
            if (!_islocked)
            {
                _ismoving = true;
                Debug.Log("Door Unlocked");
            }
        }
        else if (!_iscolliding && !_ismoving && opened)
        {
            _ismoving = true;
            Debug.Log("Door is locking");
        }

        if (_ismoving)
        {
            // Get time updated and fixed
            _timer = _timer + Time.deltaTime;

            //set percentage of movement done
            if (!opened)
                _percentage = (_timer / Duration);
            else
                _percentage = (1 - (_timer / Duration));
            // debug
            // Debug.Log("Movement done: " + (_percentage * 100) + "%");

            //stop movement when time is over.
            if (_percentage > (Percentage * 0.01f))
            {
                _ismoving = false;
                _percentage = 1;
                _timer = 0f;
                opened = true;
                // debug
                Debug.Log("Door Opened");
            }
            else if (_percentage < 0f)
            {
                _ismoving = false;
                _percentage = 0;
                _timer = 0f;
                opened = false;
                // debug
                Debug.Log("Door Closed");
            }
            // Move position
            DoorHinge.transform.localPosition = (_percentage * XYZDisplacement);
            // Rotate
            DoorHinge.transform.localRotation = new Quaternion(Rotation.x * _percentage, Rotation.y * _percentage, Rotation.z * _percentage, Rotation.w);

            //debug
            Debug.Log(DoorHinge + " is moved: " + (XYZDisplacement * _percentage) + "and rotated: " + (Rotation) + " degrees from original position");
        }

    }

    public void SetDoorLock(bool value)
    {
        //photonView.RPC(nameof(NetworkedIsLocked), RpcTarget.AllBuffered, value);
        _iscolliding = value;
    }

    void SetIsColliding(bool value)
    {
        //photonView.RPC(nameof(NetworkedIsColliding), RpcTarget.AllBuffered, value);
        _islocked = value;
    }

    [PunRPC]
    void NetworkedIsLocked(bool value, PhotonMessageInfo photonMessageInfo)
    {
        Debug.Log($"{photonMessageInfo.SentServerTime}: Set _islocked for {gameObject.name} to {value}");
        _islocked = value;
    }

    [PunRPC]
    void NetworkedIsColliding(bool value, PhotonMessageInfo photonMessageInfo)
    {
        Debug.Log($"{photonMessageInfo.SentServerTime}: Set _iscolliding for {gameObject.name} to {value}");
        _iscolliding = value;
    }
}
