using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractionType
{
    Grabable,
    Trigger,
    Hold
}

public class Interactable : MonoBehaviour
{
    [SerializeField]
    InteractionType interactionType;

    [Header("Grabable Interaction Events (Requires Rigidbody and InteractionType.Grabable)")]
    [SerializeField] OnInteractableInView onInteractableInView;
    [SerializeField] OnInteractableNotInView onInteractableNotInView;

    [Header("Trigger Interaction Events")]
    public UnityEvent onInteractionTrigger;

    [Header("Hold Interaction Event")]
    public UnityEvent<int, bool> onInteractionHold;

    new Camera camera;
    new Collider collider;

    LookListener lookListener;

    string uniqueId;

    GameObject originalParent;

    // Getters and Setters
    public InteractionType InteractionType { get { return interactionType; } set { interactionType = value; } }

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        if (transform.parent != null)
        {
            originalParent = transform.parent.gameObject;
            uniqueId = transform.parent.name;
        }

        uniqueId += $"/{name}";

        camera = Camera.main;
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnBecameVisible()
    {
        if (enabled)
            onInteractableInView.Raise(gameObject);
    }

    void OnBecameInvisible()
    {
        if (enabled)
            onInteractableNotInView.Raise(gameObject);
    }

    #endregion

    #region Public Methods
    public string GetUniqueId()
    {
        return uniqueId;
    }

    public void ResetInHierachy()
    {
        if (originalParent != null)
        {
            transform.parent = originalParent.transform;
            return;
        }

        transform.parent = null;
    }

    public void Trigger()
    {
        onInteractionTrigger.Invoke();
        // TODO: Also invoke event in all clients
    }

    public void Hold(int actorID, bool value)
    {
        onInteractionHold.Invoke(actorID, value);
        // TODO: Also invoke event in all clients
    }
    #endregion
}
