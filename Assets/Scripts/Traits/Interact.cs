using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InteractableInViewListener),
                  typeof(InteractableNotInViewListener))]
public class Interact : MonoBehaviour
{
    [SerializeField] Transform holdArea;

    [SerializeField] float pickupMaxRange = 10.0f;
    [SerializeField] float pickupForce = 75.0f;

    [SerializeField] OnInteractionHoverChange onInteractionHoverChange;

    InteractableInViewListener interactableInViewListener;
    InteractableNotInViewListener interactableNotInViewListener;

    GameObject heldObject = null;
    Rigidbody heldObjectRigidbody = null;

    LookListener lookListener;

    List<GameObject> interactables;
    GameObject targetedInteractable;

    // Start is called before the first frame update
    void Start()
    {
        interactables = new();

        interactableInViewListener = GetComponent<InteractableInViewListener>();
        interactableInViewListener.Register(AddInteractable);

        interactableNotInViewListener = GetComponent<InteractableNotInViewListener>();
        interactableNotInViewListener.Register(RemoveInteractable);
    }

    // Update is called once per frame
    void Update()
    {
        if (heldObject == null)
        {
            if (targetedInteractable != null)
            {
                Destroy(targetedInteractable.GetComponent<Outline>());
                targetedInteractable = null;
            }

            if (interactables.Count == 0)
                return;

            GameObject bestTarget = null;
            float minimumAngle = float.MaxValue;
            foreach (GameObject interactable in interactables)
            {
                float angle = Vector3.Angle(Camera.main.transform.forward, interactable.transform.position - transform.position);

                if (angle < minimumAngle)
                {
                    minimumAngle = angle;
                    bestTarget = interactable;
                }

                onInteractionHoverChange.Raise(true);
            }

            if (bestTarget != null)
            {
                bestTarget.AddComponent<Outline>();
                targetedInteractable = bestTarget;
            }

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

        PickupObject(targetedInteractable);
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

    void OnScroll(InputValue inputValue)
    {
        // Value received is either -120 (Scroll Down), 120 (Scroll Up)
        float value = inputValue.Get<float>() / 120;

        Debug.Log(value);
    }

    void AddInteractable(GameObject interactable)
    {
        interactables.Add(interactable);
    }

    void RemoveInteractable(GameObject interactable)
    {
        interactables.Remove(interactable);

        if (targetedInteractable == interactable)
        {
            Destroy(targetedInteractable.GetComponent<Outline>());
            targetedInteractable = null;
        }
    }
}