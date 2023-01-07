using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: Sync Signaler Shut Down
public class HoldTrigger : MonoBehaviour
{
    [SerializeField] Interactable[] interactables;
    [SerializeField] GameObject[] signalerPivots;
    [SerializeField] OnSignalerShutdown onSignalerShutdown;

    [Header("After Signaler Shutdown to be enabled GameObjects")]
    [SerializeField] GameObject[] objects;

    Dictionary<int, bool> holds;

    int holdCount;

    // Start is called before the first frame update
    void Start()
    {
        holdCount = 0;
        holds = new();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterHold(int actorId, bool pressed)
    {
        if (holds.TryGetValue(actorId, out bool _) || holds.Count < interactables.Count())
        {
            holds[actorId] = pressed;
            return;
        }

        if (holds.Count > interactables.Count())
            return;

        OnSuccess();
    }

    void OnSuccess()
    {
        StartCoroutine(nameof(ProcessSignalerShutDown));
    }

    IEnumerator ProcessSignalerShutDown()
    {
        foreach (GameObject pivot in signalerPivots)
        {
            pivot.SetActive(false);

            yield return new WaitForSeconds(.5f);
        }

        foreach (GameObject item in objects)
        {
            item.SetActive(true);
        }

        onSignalerShutdown.Raise();
    }
}
