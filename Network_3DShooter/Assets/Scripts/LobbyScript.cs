using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    TypedLobby killCount = new TypedLobby("killCount", LobbyType.Default);
    TypedLobby teamBattle = new TypedLobby("teamBattle", LobbyType.Default);
    TypedLobby noRespawn = new TypedLobby("noRespawn", LobbyType.Default);

    public GameObject roomNumber;
    string LevelName = "";

    private void Start()
    {
        roomNumber.SetActive(false);
    }
    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void JoinGameKillCount()
    {
        LevelName = "Floor layout";
        PhotonNetwork.JoinLobby(killCount);
    }

    public void JoinGameTeamBattle()
    {
        LevelName = "Floor layout";
        PhotonNetwork.JoinLobby(teamBattle);

    }

    public void JoinGameNoRespawn()
    {
        LevelName = "Floor layout";
        PhotonNetwork.JoinLobby(noRespawn);

    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joined random room failed, creating a new room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        PhotonNetwork.CreateRoom("Arena" + Random.Range(1, 1000), roomOptions);
    }

    public override void OnJoinedRoom()
    {
        roomNumber.SetActive(true);
        PhotonNetwork.LoadLevel(LevelName);
    }
}
