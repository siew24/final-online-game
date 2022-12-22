using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] Transform holdArea;

    [SerializeField] float pickupRange = 5.0f;
    [SerializeField] float pickupForce = 75.0f;

    [SerializeField] OnInteractionHoverChange onInteractionHoverChange;

    GameObject heldObject = null;
    Rigidbody heldObjectRigidbody = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (heldObject == null)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, pickupRange, LayerMask.GetMask("Interactable")))
            {
                onInteractionHoverChange.Raise(true);
            }
            else
                onInteractionHoverChange.Raise(false);

            return;
        }

        MoveObject();
    }

    void OnInteract()
    {
        if (heldObject != null)
        {
            DropObject();
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 5f, LayerMask.GetMask("Interactable")))
            PickupObject(hitInfo.collider.gameObject);
    }

    void PickupObject(GameObject target)
    {
        heldObjectRigidbody = target.GetComponent<Rigidbody>();
        heldObjectRigidbody.useGravity = false;
        heldObjectRigidbody.drag = 10;
        heldObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        heldObjectRigidbody.transform.parent = holdArea;
        heldObject = target;

        onInteractionHoverChange.Raise(false);
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - heldObject.transform.position;
            heldObjectRigidbody.AddForce(moveDirection * pickupForce);
        }
    }

    void DropObject()
    {
        if (heldObject == null)
            return;

        heldObjectRigidbody.useGravity = true;
        heldObjectRigidbody.drag = 1;
        heldObjectRigidbody.constraints = RigidbodyConstraints.None;

        heldObject.GetComponent<Interactable>().ResetInHierachy();

        heldObject = null;
    }
}