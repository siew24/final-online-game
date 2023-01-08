using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;
using System;
using Michsky.UI.Shift;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance = null;

    [SerializeField] OnGameStart onGameStart;
    [SerializeField] TextMeshProUGUI roomTitle;
    [SerializeField] GameObject roomListContent;
    [SerializeField] GameObject roomItemPrefab;

    [SerializeField] GameObject roomStartButton;
    [SerializeField] GameObject playerListContent;
    [SerializeField] GameObject playerItemPrefab;

    NetworkMainMenuHelper networkMainMenuHelper;

    Coroutine load = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        networkMainMenuHelper = FindObjectOfType<NetworkMainMenuHelper>();
        roomStartButton.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);
        onGameStart.Raise();
    }

    void StartGame()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = new()
        {
            { Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED), new Dictionary<int, bool>() }
        };

        Dictionary<int, bool> playersLoaded = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)];
        playersLoaded[PhotonNetwork.LocalPlayer.ActorNumber] = false;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    #region Public Methods
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetNickName(GameObject name)
    {
        PhotonNetwork.NickName = name.GetComponent<TMP_InputField>().text;
    }

    public void Connect()
    {
        Debug.Log("Connecting");

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CreateRoom(GameObject title)
    {
        string roomName = title.GetComponent<TMP_InputField>().text;

        RoomOptions roomOptions = new();
        roomOptions.SuppressPlayerInfo = false;
        roomOptions.SuppressRoomEvents = false;
        roomOptions.CleanupCacheOnLeave = true;
        roomOptions.IsOpen = true;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.MaxPlayers = 4;

        ExitGames.Client.Photon.Hashtable roomProperties = new()
        {
            { Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY), new Dictionary<int, bool>() }
        };

        Dictionary<int, bool> playersReady = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)];

        playersReady[PhotonNetwork.LocalPlayer.ActorNumber] = false;

        roomOptions.CustomRoomProperties = roomProperties;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(GameObject room)
    {
        PhotonNetwork.JoinRoom(room.GetComponent<TextMeshProUGUI>().text);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void MoveToLevel()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("GameLevel");

        // Pause incoming messages until level is loaded
        // if (!PhotonNetwork.IsMasterClient)
        //     PhotonNetwork.IsMessageQueueRunning = false;

        //StartGame();
    }

    public void TogglePlayerReady(bool isReady)
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)))
            roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY), new Dictionary<int, bool>());

        Dictionary<int, bool> playersReady = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)];

        playersReady[PhotonNetwork.LocalPlayer.ActorNumber] = isReady;
        roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)] = playersReady;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    #endregion

    #region Callback Methods
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "MainMenu")
        {
            networkMainMenuHelper = FindObjectOfType<NetworkMainMenuHelper>();
            // TODO: Leave current room and join the general lobby
            return;
        }

        if (scene.name == "GameLevel")
        {
            // PhotonNetwork.IsMessageQueueRunning = true;
            onGameStart.Raise();

            ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)))
                roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED), new Dictionary<int, bool>());

            Dictionary<int, bool> playersLoaded = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)];
            playersLoaded[PhotonNetwork.LocalPlayer.ActorNumber] = true;

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

            return;
        }
    }
    #endregion

    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnected()
    {
        Debug.Log("Connected To The Internet");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected with reason {cause}");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"Joined Default Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform roomItem in roomListContent.transform)
        {
            Destroy(roomItem);
        }

        foreach (RoomInfo room in roomList)
        {
            GameObject roomItem = Instantiate(roomItemPrefab, roomListContent.transform);

            roomItem.GetComponent<RoomMetadata>().title.text = room.Name;
            roomItem.GetComponent<RoomMetadata>().description.text = "";
            roomItem.GetComponent<RoomMetadata>().button.onClick.AddListener(() =>
            {
                networkMainMenuHelper.roomSelectedModal.windowTitle.text = room.Name;
                networkMainMenuHelper.roomSelectedModal.windowDescription.text = "";
                networkMainMenuHelper.roomSelectedModal.ModalWindowIn();
                networkMainMenuHelper.modalParent.BlurInAnim();
            });
        }
    }

    public override void OnCreatedRoom()
    {
        roomTitle.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        roomTitle.text = PhotonNetwork.CurrentRoom.Name;

        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)))
            roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY), new Dictionary<int, bool>());

        Dictionary<int, bool> playersReady = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)];

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            GameObject playerItem = Instantiate(playerItemPrefab, playerListContent.transform);

            playerItem.GetComponent<PlayerMetadata>().id = player.ActorNumber;
            playerItem.GetComponent<PlayerMetadata>().name.text = player.NickName;
            playerItem.GetComponent<PlayerMetadata>().description.text = "";
            playerItem.GetComponent<PlayerMetadata>().ready.text =
                !playersReady.ContainsKey(player.ActorNumber) ?
                    "Not Ready"
                    :
                    playersReady[player.ActorNumber] ?
                        "Ready"
                        :
                        "Not Ready";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerItem = Instantiate(playerItemPrefab, playerListContent.transform);

        playerItem.GetComponent<PlayerMetadata>().id = newPlayer.ActorNumber;
        playerItem.GetComponent<PlayerMetadata>().name.text = newPlayer.NickName;
        playerItem.GetComponent<PlayerMetadata>().description.text = "";
        playerItem.GetComponent<PlayerMetadata>().ready.text = "Not Ready";
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        foreach (Transform item in playerListContent.transform)
        {
            if (item.GetComponent<PlayerMetadata>().id == newPlayer.ActorNumber)
            {
                Destroy(item);
                break;
            }
        }

        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)))
            roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY), new Dictionary<int, bool>());

        Dictionary<int, bool> playersReady = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)];
        playersReady.Remove(newPlayer.ActorNumber);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)) && !PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.IsMessageQueueRunning = false;
            // if (SceneManager.GetActiveScene().name != "GameLevel")
            // {
            //     SceneManager.LoadSceneAsync("GameLevel");
            //     return;
            // }
        }

        // if (SceneManager.GetActiveScene().name == "GameLevel")
        // {
        //     if (load == null && propertiesThatChanged.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)))
        //     {
        //         ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        //         if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)))
        //             roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED), new Dictionary<int, bool>());

        //         Dictionary<int, bool> playersLoaded = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)];

        //         int count = 0;
        //         foreach (bool value in playersLoaded.Values)
        //             if (value) count++;

        //         Debug.Log($"Players loaded: {count}");

        //         if (count == PhotonNetwork.CurrentRoom.PlayerCount)
        //             load = StartCoroutine(nameof(Load));

        //         return;
        //     }
        // }

        if (SceneManager.GetActiveScene().name == "MainMenu")
            if (propertiesThatChanged.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)))
            {
                ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

                if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)))
                    roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY), new Dictionary<int, bool>());

                Dictionary<int, bool> playersReady = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_READY)];

                foreach (Transform item in playerListContent.transform)
                    item.GetComponent<PlayerMetadata>().ready.text = playersReady[item.GetComponent<PlayerMetadata>().id] ? "Ready" : "Not Ready";

                if (PhotonNetwork.IsMasterClient)
                {
                    int count = 0;
                    foreach (bool value in playersReady.Values)
                        if (value) count++;

                    Debug.Log($"Players ready: {count}");
                    roomStartButton.SetActive(count == PhotonNetwork.CurrentRoom.PlayerCount);
                }
                return;
            }

    }

    #endregion
}