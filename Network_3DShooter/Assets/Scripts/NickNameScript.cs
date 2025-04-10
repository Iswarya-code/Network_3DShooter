using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NickNameScript : MonoBehaviourPunCallbacks
{
    public Text[] names;
    public Image[] healthBars;
    GameObject waitObject;

    private void Start()
    {
        for(int i=0; i<names.Length;i++)
        {
            names[i].gameObject.SetActive(false);
            healthBars[i].gameObject.SetActive(false);
        }
        waitObject = GameObject.Find("WaitingBG");
    }

    public void Leaving()
    {
        StartCoroutine("BackToLobby");
    }

    IEnumerator BackToLobby()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.LoadLevel("Lobby");
    }

    //this is for the waiting screen
    public void ReturnToLobby()
    {
        waitObject.SetActive(false);
        RoomExit();
    }

    void RoomExit()
    {
        StartCoroutine(ToLobby());
    }

    IEnumerator ToLobby()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
