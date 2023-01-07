using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Constants;

[RequireComponent(typeof(HintCollectedListener))]
public class HintTracker : MonoBehaviour
{

    [SerializeField] Area trackingArea;
    [SerializeField] UnityEvent beforeAllHintsCollectedEvent;
    [SerializeField] UnityEvent<(int, int)> collectedHintAmountChangeEvent = new();
    [SerializeField] UnityEvent afterAllHintsCollectedEvent;

    HintCollectedListener hintCollectedListener;

    Dictionary<int, string> allHints;
    List<string> collectedHints;

    bool _collectedAll;

    // Start is called before the first frame update
    void Start()
    {
        _collectedAll = false;
        collectedHints = new();

        switch (trackingArea)
        {
            case Area.Area0:
                allHints = Constants.Area0.HINT;
                break;
            case Area.Area1:
                allHints = Constants.Area1.HINT;
                break;
            case Area.Area2:
                allHints = Constants.Area2.HINT;
                break;
            case Area.Area3:
                allHints = Constants.Area3.HINT;
                break;
        }

        Utils.GetListener(this, out hintCollectedListener);
        hintCollectedListener.Register(OnHintCollected);

        beforeAllHintsCollectedEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnHintCollected(string hintId)
    {
        if (!allHints.ContainsValue(hintId))
            return;

        if (collectedHints.Contains(hintId))
            return;

        collectedHints.Add(hintId);
        collectedHintAmountChangeEvent.Invoke((collectedHints.Count, allHints.Count));

        CheckAllHintsCollected();
    }

    void CheckAllHintsCollected()
    {
        if (collectedHints.Count != allHints.Count)
            return;

        _collectedAll = true;
        afterAllHintsCollectedEvent.Invoke();
    }
}
