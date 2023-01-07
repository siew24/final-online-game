using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class HintCollectedBroadcaster : MonoBehaviour
{
    [SerializeField] Area trackingArea;
    [SerializeField] int hintNumber;
    [SerializeField] OnHintCollected onHintCollected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BroadcastHintCollected()
    {
        onHintCollected.Raise(Constants.Utils.GetHintId(trackingArea, hintNumber));
    }
}
