using System.Collections.Generic;
using cakeslice;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InteractableInViewListener),
                  typeof(InteractableNotInViewListener))]
[RequireComponent(typeof(GameStartListener),
                  typeof(GameEndListener),
                  typeof(GameSuspendListener))]
public class Interact : MonoBehaviour
{
    [SerializeField] Transform holdArea;

    [Header("Grab Attributes")]
    [SerializeField] float pickupMaxRange = 10.0f;
    [SerializeField] float pickupForce = 75.0f;

    [Header("Interact Attributes")]
    [SerializeField] float interactRange = 10f;

    [SerializeField] OnInteractionHoverChange onInteractionHoverChange;

    // Listeners
    GameStartListener gameStartListener;
    GameSuspendListener gameSuspendListener;
    GameEndListener gameEndListener;
    InteractableInViewListener interactableInViewListener;
    InteractableNotInViewListener interactableNotInViewListener;

    GameObject heldObject = null;
    Rigidbody heldObjectRigidbody = null;

    // Grabables
    List<GameObject> grabableInteractables;

    // Triggers
    List<GameObject> triggerInteractables;

    // Holds
    List<GameObject> holdInteractables;

    // Player current grabbing interactable
    GameObject targetedInteractable;

    // Game
    bool _isGameSuspended;


    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        _isGameSuspended = true;

        triggerInteractables = new();
        grabableInteractables = new();
        holdInteractables = new();

        Utils.GetListener(this, out gameStartListener);
        gameStartListener.Register(() => OnGameSuspend(false));

        Utils.GetListener(this, out gameSuspendListener);
        gameSuspendListener.Register(OnGameSuspend);

        Utils.GetListener(this, out gameEndListener);
        gameEndListener.Register(() => OnGameSuspend(true));

        Utils.GetListener(this, out interactableInViewListener);
        interactableInViewListener.Register(AddInteractable);

        Utils.GetListener(this, out interactableNotInViewListener);
        interactableNotInViewListener.Register(RemoveInteractable);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Only Choose Interactable for Owner
        if (_isGameSuspended)
        {
            if (targetedInteractable == null)
                return;

            if (!targetedInteractable.TryGetComponent(out Outline component))
            {
                return;
            }

            Destroy(component);
            return;
        }

        if (targetedInteractable != null && !targetedInteractable.TryGetComponent(out Outline _))
            targetedInteractable.AddComponent<Outline>();


        ChooseInteractable();

        if (heldObject != null && heldObject.GetComponent<Interactable>().InteractionType == InteractionType.Grabable)
            MoveObject();
    }
    #endregion

    #region Interaction Methods
    void ChooseInteractable()
    {
        onInteractionHoverChange.Raise(false);

        if (ChooseHoldInteractable())
            return;

        if (ChooseTriggerInteractable())
            return;

        ChooseGrabable();

    }

    bool ChooseHoldInteractable()
    {
        if (targetedInteractable != null)
        {
            Destroy(targetedInteractable.GetComponent<Outline>());
            targetedInteractable = null;
        }

        GameObject bestTarget = null;
        float minimumDistance = interactRange;
        foreach (GameObject interactable in holdInteractables)
        {
            Debug.DrawRay(interactable.transform.position, transform.position + new Vector3(0, 1, 0) - interactable.transform.position, Color.red, Time.deltaTime);

            if (Physics.Raycast(interactable.transform.position, (transform.position + new Vector3(0, 1, 0) - interactable.transform.position).normalized, out RaycastHit hitInfo, interactRange))//, LayerMask.NameToLayer("Player")))
            {
                Debug.Log($"{interactable.name} hit {hitInfo.collider.gameObject.name}");


                if (Vector3.Distance(transform.position, interactable.transform.position) >= minimumDistance)
                    continue;

                Debug.Log($"{interactable.name}: {Vector3.Distance(transform.position, interactable.transform.position)} >= {minimumDistance} => {Vector3.Distance(transform.position, interactable.transform.position) >= minimumDistance}");

                minimumDistance = Vector3.Distance(transform.position, interactable.transform.position);
                bestTarget = interactable;
            }
        }

        if (bestTarget != null)
        {
            bestTarget.AddComponent<Outline>();
            targetedInteractable = bestTarget;

            onInteractionHoverChange.Raise(true);
            return true;
        }

        return false;
    }

    bool ChooseTriggerInteractable()
    {
        if (targetedInteractable != null)
        {
            Destroy(targetedInteractable.GetComponent<Outline>());
            targetedInteractable = null;
        }

        GameObject bestTarget = null;
        float minimumDistance = interactRange;
        foreach (GameObject interactable in triggerInteractables)
        {
            Debug.DrawRay(interactable.transform.position, transform.position + new Vector3(0, 1, 0) - interactable.transform.position, Color.red, Time.deltaTime);

            if (Physics.Raycast(interactable.transform.position, (transform.position + new Vector3(0, 1, 0) - interactable.transform.position).normalized, out RaycastHit hitInfo, interactRange))//, LayerMask.NameToLayer("Player")))
            {
                Debug.Log($"{interactable.name} hit {hitInfo.collider.gameObject.name}");


                if (Vector3.Distance(transform.position, interactable.transform.position) >= minimumDistance)
                    continue;

                Debug.Log($"{interactable.name}: {Vector3.Distance(transform.position, interactable.transform.position)} >= {minimumDistance} => {Vector3.Distance(transform.position, interactable.transform.position) >= minimumDistance}");

                minimumDistance = Vector3.Distance(transform.position, interactable.transform.position);
                bestTarget = interactable;
            }
        }

        if (bestTarget != null)
        {
            bestTarget.AddComponent<Outline>();
            targetedInteractable = bestTarget;

            onInteractionHoverChange.Raise(true);
            return true;
        }

        return false;
    }

    void ChooseGrabable()
    {
        if (heldObject == null)
        {
            if (targetedInteractable != null)
            {
                Destroy(targetedInteractable.GetComponent<Outline>());
                targetedInteractable = null;
            }

            if (grabableInteractables.Count == 0)
                return;

            GameObject bestTarget = null;
            float minimumAngle = float.MaxValue;
            foreach (GameObject interactable in grabableInteractables)
            {
                float angle = Vector3.Angle(Camera.main.transform.forward, interactable.transform.position - transform.position);

                if (Physics.Raycast(interactable.transform.position, transform.position - interactable.transform.position, out RaycastHit hitInfo, pickupMaxRange, LayerMask.NameToLayer("Player")))
                {
                    if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
                        continue;

                    if (angle < minimumAngle)
                    {
                        minimumAngle = angle;
                        bestTarget = interactable;
                    }
                }


            }

            if (bestTarget != null)
            {
                bestTarget.AddComponent<Outline>();
                targetedInteractable = bestTarget;

                onInteractionHoverChange.Raise(true);
            }

            return;
        }
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

    void AddInteractable(GameObject interactable)
    {
        switch (interactable.GetComponent<Interactable>().InteractionType)
        {
            case InteractionType.Grabable:
                grabableInteractables.Add(interactable);
                return;
            case InteractionType.Trigger:
                triggerInteractables.Add(interactable);
                return;
            case InteractionType.Hold:
                holdInteractables.Add(interactable);
                return;
        }
    }

    void RemoveInteractable(GameObject interactable)
    {
        switch (interactable.GetComponent<Interactable>().InteractionType)
        {
            case InteractionType.Grabable:
                grabableInteractables.Remove(interactable);
                break;
            case InteractionType.Trigger:
                triggerInteractables.Remove(interactable);
                break;
            case InteractionType.Hold:
                holdInteractables.Remove(interactable);
                return;
        }

        if (targetedInteractable != null && targetedInteractable == interactable)
        {
            Destroy(targetedInteractable.GetComponent<Outline>());
            targetedInteractable = null;
        }
    }
    #endregion

    #region Callback Methods
    void OnInteract(InputValue inputValue)
    {
        bool pressedDown = inputValue.Get<float>() > 0;

        if (targetedInteractable == null)
            return;

        /*
            TODO: Handle Transfer Ownership

            In each of the interactions, data will be synced if the interactor is the owner
        */

        if (targetedInteractable.GetComponent<Interactable>().InteractionType == InteractionType.Hold)
        {
            // TODO: We should also pass the Actor ID here to recognize two players
            targetedInteractable.GetComponent<Interactable>().Hold(PhotonNetwork.LocalPlayer.ActorNumber, pressedDown);
            if (pressedDown)
            {
                heldObject = targetedInteractable;
                Destroy(targetedInteractable.GetComponent<Outline>());
            }
            else
            {
                heldObject = null;
                targetedInteractable.AddComponent<Outline>();
            }

            return;
        }

        // The rest of the interactables does not need release input
        if (!pressedDown)
            return;

        if (targetedInteractable.GetComponent<Interactable>().InteractionType == InteractionType.Grabable)
        {
            if (heldObject != null)
            {
                DropObject();
                return;
            }

            PickupObject(targetedInteractable);
            return;
        }

        if (targetedInteractable.GetComponent<Interactable>().InteractionType == InteractionType.Trigger)
        {
            targetedInteractable.GetComponent<Interactable>().Trigger();
            Destroy(targetedInteractable.GetComponent<Interactable>());
            Destroy(targetedInteractable.GetComponent<Outline>());
            triggerInteractables.Remove(targetedInteractable);
            return;
        }


    }

    void OnGameSuspend(bool value)
    {
        _isGameSuspended = value;
    }

    #endregion
}