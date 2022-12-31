using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [SerializeField] OnInteractableInView onInteractableInView;
    [SerializeField] OnInteractableNotInView onInteractableNotInView;

    new Camera camera;
    new Collider collider;

    LookListener lookListener;

    string uniqueId;

    GameObject originalParent;

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
        onInteractableInView.Raise(gameObject);
    }

    void OnBecameInvisible()
    {
        onInteractableNotInView.Raise(gameObject);
    }

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
    #endregion
}
