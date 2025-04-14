using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisplayColor : MonoBehaviourPunCallbacks
{
    public int[] buttonNumbers;
    public int[] viewID;
    public Color32[] colors;

    GameObject namesObject;
    GameObject waitForPlayers;

    public AudioClip[] gunShotSounds;

    private void Start()
    {
        namesObject = GameObject.Find("NamesBG");
        waitForPlayers = GameObject.Find("WaitingBG");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GetComponent<PhotonView>().IsMine == true && waitForPlayers.activeInHierarchy == false)
            {
                RemoveData();
                RoomExit();
            }
        }
    }

    public void DeliverDamage(string name, float damageAmt)
    {
        GetComponent<PhotonView>().RPC("GunDamage", RpcTarget.AllBuffered, name, damageAmt);
    }
    [PunRPC]
    public void GunDamage(string name, float damageAmt)
    {
        for(int i=0;i<namesObject.GetComponent<NickNameScript>().name.Length; i++)
        {
            if(name == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount -= damageAmt;
            }
        }
    }

    void RemoveData()
    {
        GetComponent<PhotonView>().RPC("RemoveMe", RpcTarget.AllBuffered);
    }

   
    void RoomExit()
    {

    }
    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC("AssignColor", RpcTarget.AllBuffered);
    }

    //play audio when gunshot
    public void PlayGunShot(string name, int weaponNumber)
    {
        GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.All, name, weaponNumber);
    }

    [PunRPC]
    
    public void PlaySound(string name, int weaponNumber)
    {
        for(int i=0; i< namesObject.GetComponent<NickNameScript>().names.Length; i++)
        {
            if(name == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                GetComponent<AudioSource>().clip = gunShotSounds[weaponNumber];
                GetComponent<AudioSource>().Play();
            }
        }
    }

    [PunRPC]
    public void AssignColor()
    {
        for(int i=0; i<viewID.Length; i++)
        {
            if(this.GetComponent<PhotonView>().ViewID == viewID[i])
            {
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
            }
        }
    }

    [PunRPC]
    void RemoveMe()
    {
        for(int i = 0; i< namesObject.gameObject.GetComponent<NickNameScript>().names.Length;i++)
        {
            if(this.GetComponent<PhotonView>().Owner.NickName == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(false);
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(false);

            }
        }
    }

    IEnumerator GetReadyToLeave()
    {
        yield return new WaitForSeconds(1);
        namesObject.GetComponent<NickNameScript>().Leaving();
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
    }
}
