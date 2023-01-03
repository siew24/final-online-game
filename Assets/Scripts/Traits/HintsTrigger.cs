using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OnHintsTriggerListener), typeof(OnHintsDeactivateListener))]
public class HintsTrigger : MonoBehaviour
{
    // Reference to the player character
    public GameObject player;

    // Reference to the UI game object
    public GameObject UI;

    // Reference to the OnUITrigger event
    public OnHintsTriggerListener onHintsTriggerListener;

    // Reference to the OnUIDeactivate event
    public OnHintsDeactivateListener onHintsDeactivateListener;

    // Method to trigger the UI and lock player movement
    public void TriggerHints()
    {
        UI.SetActive(true);
        //player.GetComponent<PlayerMovement>().isLocked = true;
    }

    // Method to deactivate the UI and unlock player movement
    public void DeactivateHints()
    {
        UI.SetActive(false);
        //player.GetComponent<PlayerMovement>().isLocked = false;
    }

    void Start()
    {
        onHintsTriggerListener = GetComponent<OnHintsTriggerListener>();
        onHintsTriggerListener.Register(TriggerHints);

        onHintsDeactivateListener = GetComponent<OnHintsDeactivateListener>();
        onHintsDeactivateListener.Register(DeactivateHints);
    }
}
