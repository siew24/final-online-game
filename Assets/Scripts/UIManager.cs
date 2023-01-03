using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionHoverChangeListener), typeof(OnHintsTriggerListener))]
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tooltip;

    InteractionHoverChangeListener interactionHoverChangeListener;

    OnHintsTriggerListener onHintsTriggerListener;

    // Reference to the player character
    public GameObject player;

    // Reference to the UI game object
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        tooltip.SetActive(false);

        interactionHoverChangeListener = GetComponent<InteractionHoverChangeListener>();
        interactionHoverChangeListener.Register(ToggleInteractPrompt);

        onHintsTriggerListener = GetComponent<OnHintsTriggerListener>();
        onHintsTriggerListener.Register(TriggerHints);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleInteractPrompt(bool value)
    {
        tooltip.SetActive(value);
    }

    public void TriggerHints()
    {
        UI.SetActive(true);
        //player.GetComponent<PlayerMovement>().isLocked = true;
    }

}
