using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    public Canvas matchingPopup;
    public Text playerInfo;
    public Text enemyInfo;

    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    public byte Version = 0;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;


    public virtual void Start()
    {
        matchingPopup.enabled = true;

        int score = 0;
        foreach (UserScore us in UserScore.list)
            if (us.game_name == "Alkkagi")
            {
                score = us.score;
                break;
            }

        MatchingInfo.playerUserId = Player.instance.user_id;
        MatchingInfo.playerScore = score;

        PhotonNetwork.player.NickName = Player.instance.user_id;
        PhotonNetwork.player.SetScore(score);

        PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom() called by PUN.");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.Log("Not PhotonNetwork.isMasterClient");

            MatchingInfo.otherUserId = PhotonNetwork.otherPlayers[0].NickName;
            MatchingInfo.otherScore = PhotonNetwork.otherPlayers[0].GetScore();
            MatchingInfo.blackId = MatchingInfo.otherUserId;
            MatchingInfo.whiteId = MatchingInfo.playerUserId;

            playerInfo.color = Color.white;
            enemyInfo.color = Color.black;
            playerInfo.text = MatchingInfo.playerUserId + '\n' + MatchingInfo.playerScore.ToString();
            enemyInfo.text = MatchingInfo.otherUserId + '\n' + MatchingInfo.otherScore.ToString();

            matchingPopup.enabled = false;
            TurnManagerAlkkagi.color = 0;
            GameManagerAlkkagi.playerColor = 1;
            GameManagerAlkkagi.isStarted = true;
        }
    }

    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom() called by PUN.");
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton() called by PUN.");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("OnPhotonPlayerConnected() called by PUN.");

        MatchingInfo.otherUserId = newPlayer.NickName;
        MatchingInfo.otherScore = newPlayer.GetScore();
        MatchingInfo.blackId = MatchingInfo.playerUserId;
        MatchingInfo.whiteId = MatchingInfo.otherUserId;

        playerInfo.text = MatchingInfo.playerUserId + '\n' + MatchingInfo.playerScore.ToString();
        enemyInfo.text = MatchingInfo.otherUserId + '\n' + MatchingInfo.otherScore.ToString();

        matchingPopup.enabled = false;
        TurnManagerAlkkagi.color = 0;
        GameManagerAlkkagi.playerColor = 0;
        GameManagerAlkkagi.isStarted = true;
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("OnPhotonPlayerDisconnected() called by PUN.");

        if (GameManagerAlkkagi.isStarted && !GameManagerAlkkagi.isEnded)
            GameManagerAlkkagi.EndGame(GameManagerAlkkagi.playerColor);
    }

}
