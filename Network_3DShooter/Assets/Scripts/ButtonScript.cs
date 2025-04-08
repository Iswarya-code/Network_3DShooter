using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonScript : MonoBehaviour
{
    GameObject[] players;
    int myID;
     GameObject panel;
    GameObject namesObject;
    private void Start()
    {
        Cursor.visible = true;
        panel = GameObject.Find("ChoosePanel");
        namesObject = GameObject.Find("NamesBG");
    }
    public void SelectButton(int ButtonNumber)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0; i<players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().IsMine == true)
            {
                myID = players[i].GetComponent<PhotonView>().ViewID;
                break;
            }
        }
        GetComponent<PhotonView>().RPC("SelectedColor", RpcTarget.AllBuffered, ButtonNumber, myID);
        Cursor.visible = false;
        panel.SetActive(false);
    }

    [PunRPC]

    void SelectedColor(int ButtonNumber, int myID)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<DisplayColor>().viewID[ButtonNumber] = myID;
            players[i].GetComponent<DisplayColor>().ChooseColor();
        }
        namesObject.GetComponent<Timer>().BeginTimer();
        this.transform.gameObject.SetActive(false);
    }
}
