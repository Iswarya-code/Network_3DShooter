using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerCheck : MonoBehaviour
{
    public int maxPlayersInRoom = 2;
    public Text currentPlayers;
    public GameObject hint1, hint2;
    public GameObject EnterButton;
   

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersInRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            hint1.SetActive(false);
            hint2.SetActive(false);
            EnterButton.SetActive(true);

        }
        if(EnterButton.activeInHierarchy != true)
        {
            currentPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + maxPlayersInRoom.ToString();

        }
        else
        {
            currentPlayers.text = "";

        }
    }

    public void EnterTheArena()
    {
        this.gameObject.SetActive(false);

    }
}
