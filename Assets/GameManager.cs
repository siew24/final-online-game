using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject[] spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, Quaternion.identity);

            // Set this player loaded to true
            ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (!roomProperties.ContainsKey(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)))
                roomProperties.Add(Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED), new Dictionary<int, bool>());

            Dictionary<int, bool> playersLoaded = (Dictionary<int, bool>)roomProperties[Enum.GetName(typeof(Constants.Room.Properties), Constants.Room.Properties.PLAYER_LOADED)];

            playersLoaded[PhotonNetwork.LocalPlayer.ActorNumber] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
