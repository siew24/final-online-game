using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionHoverChangeListener))]
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tooltip;

    InteractionHoverChangeListener interactionHoverChangeListener;

    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(false);

        interactionHoverChangeListener = GetComponent<InteractionHoverChangeListener>();
        interactionHoverChangeListener.Register(ToggleInteractPrompt);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleInteractPrompt(bool value)
    {
        tooltip.SetActive(value);
    }
}
