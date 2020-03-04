﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

namespace Firepower.Networking
{
    public class MultiplayerLobby : MonoBehaviourPunCallbacks
    {
        readonly string _gameVersion = "1";

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        int playerIndex;

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            Connect();
        }

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = _gameVersion;
                PhotonNetwork.ConnectUsingSettings();

                //Bazz
                PhotonNetwork.NickName = "Player " + PhotonNetwork.LocalPlayer.ActorNumber;
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master. Searching for a random room.");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, one will be created.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = "Player " + (PhotonNetwork.CurrentRoom.PlayerCount-1);
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            CharacterSelectionManager.Instance.StartPlayer(PhotonNetwork.CurrentRoom.PlayerCount - 1);
            //Welp this didn't work. I don't exactly know why...
            /*PhotonView playerView = CharacterSelectionManager.Instance.gameObject.AddComponent<PhotonView>();
            if (playerView.ObservedComponents == null) playerView.ObservedComponents = new List<Component>();
            playerView.ObservedComponents.Add(CharacterSelectionManager.Instance);
            playerView.OwnershipTransfer = OwnershipOption.Takeover;
            playerView.TransferOwnership(PhotonNetwork.LocalPlayer);
            playerView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;*/
        }
    }
}
