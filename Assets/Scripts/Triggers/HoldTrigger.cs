using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// TODO: Sync Signaler Shut Down
public class HoldTrigger : MonoBehaviour, IInRoomCallbacks
{
    [SerializeField] Interactable[] interactables;
    [SerializeField] GameObject[] signalerPivots;
    [SerializeField] OnSignalerShutdown onSignalerShutdown;

    [Header("After Signaler Shutdown to be enabled GameObjects")]
    [SerializeField] GameObject[] objects;

    Dictionary<int, bool> holds;

    int holdCount;

    Coroutine signalerAnimation = null;

    // Start is called before the first frame update
    void Start()
    {
        holdCount = 0;
        holds = new();
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterHold(int actorId, bool pressed)
    {
        if (signalerAnimation != null)
            return;

        if (holds.Count > interactables.Count())
            return;

        if (holds.TryGetValue(actorId, out bool _) || holds.Count < PhotonNetwork.CurrentRoom.PlayerCount)
            holds[actorId] = pressed;

        if (!pressed)
            return;

        foreach (bool isHolding in holds.Values)
            if (!isHolding)
                return;

        OnSuccess();
    }

    void OnSuccess()
    {
        signalerAnimation = StartCoroutine(nameof(ProcessSignalerShutDown));

        foreach (Interactable interactable in interactables)
            Destroy(interactable);
    }

    IEnumerator ProcessSignalerShutDown()
    {
        foreach (GameObject pivot in signalerPivots.Reverse())
        {
            pivot.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }

        foreach (GameObject item in objects)
            item.SetActive(true);

        onSignalerShutdown.Raise();
        yield return null;
        Destroy(this);
    }

    #region Photon Callbacks

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (holds.ContainsKey(otherPlayer.ActorNumber))
            holds.Remove(otherPlayer.ActorNumber);

    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
    }

    #endregion
}
