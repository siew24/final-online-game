using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public GameObject panel;
    private bool panelActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            panelActive = !panelActive;
            panel.SetActive(panelActive);
        }
    }
}
