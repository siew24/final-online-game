using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [SerializeField] OnGameSuspend onGameSuspend;
    public GameObject panel;
    private bool panelActive = false;

    void Start()
    {
        GetComponent<Interactable>().InteractionType = InteractionType.Trigger;
    }

    void Update()
    {
    }

    public void Trigger()
    {
        panelActive = !panelActive;
        panel.SetActive(panelActive);
        onGameSuspend.Raise(true);
    }
}
