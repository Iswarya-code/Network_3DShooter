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
    //for team battle
    public Color32[] teamColors;
    bool teamMode = false;

    GameObject namesObject;
    GameObject waitForPlayers;

    public AudioClip[] gunShotSounds;

    //no respawn scene
    bool isRespawn = false;

    private void Start()
    {
        namesObject = GameObject.Find("NamesBG");
        waitForPlayers = GameObject.Find("WaitingBG");
        InvokeRepeating("CheckTime", 1, 1);
        teamMode = namesObject.GetComponent<NickNameScript>().teamMode;
        isRespawn = namesObject.GetComponent<NickNameScript>().noRespawn;
        GetComponent<PlayerMovement>().noRespawn = isRespawn;
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
        if(this.GetComponent<Animator>().GetBool("Hit") == true)
        {
            StartCoroutine(Recover());
        }
    }

    public void NoRespawnExit()
    {
        namesObject.GetComponent<NickNameScript>().eliminationPanel.SetActive(true);
        StartCoroutine(WaitToExit());
    }
    void CheckTime()
    {
        if(namesObject.GetComponent<Timer>().timeStop==true)
        {
            this.gameObject.GetComponent<PlayerMovement>().isDead = true;
            this.gameObject.GetComponent<PlayerMovement>().gameOver = true;
            this.gameObject.GetComponent<WeaponChange_A>().isDead = true;
            this.gameObject.GetComponentInChildren<AimLookAtRef>().isDead = true;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    public void Respawn(string name)
    {
        GetComponent<PhotonView>().RPC("ResetForReplay", RpcTarget.AllBuffered,name);
    }
    [PunRPC]
    void ResetForReplay(string name)
    {
        for(int i=0; i<namesObject.GetComponent<NickNameScript>().names.Length; i++)
        {
            if(name == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                this.GetComponent<Animator>().SetBool("Dead", false);
                this.gameObject.GetComponent<WeaponChange_A>().isDead = false;
                this.gameObject.GetComponentInChildren<AimLookAtRef>().isDead = false;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount = 1;
            }
        }
    }
    public void DeliverDamage(string shooterName,string name, float damageAmt)
    {
        GetComponent<PhotonView>().RPC("GunDamage", RpcTarget.AllBuffered,shooterName, name, damageAmt);
    }
    [PunRPC]
    public void GunDamage(string shooterName,string name, float damageAmt)
    {
        for(int i=0;i<namesObject.GetComponent<NickNameScript>().name.Length; i++)
        {
            if(name == namesObject.GetComponent<NickNameScript>().names[i].text)
            {
                if (namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount > 0.1f)
                {
                    this.GetComponent<Animator>().SetBool("Hit", true);
                    namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount -= damageAmt;

                }
                else
                {
                    namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.GetComponent<Image>().fillAmount = 0;
                    this.GetComponent<Animator>().SetBool("Dead", true);
                    this.gameObject.GetComponent<PlayerMovement>().isDead = true;
                    this.gameObject.GetComponent<WeaponChange_A>().isDead = true;
                    this.gameObject.GetComponentInChildren<AimLookAtRef>().isDead = true;
                    namesObject.GetComponent<NickNameScript>().RunMessage(shooterName, name);
                    this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                }
            }
        }
    }

    void RemoveData()
    {
        GetComponent<PhotonView>().RPC("RemoveMe", RpcTarget.AllBuffered);
    }

   
    void RoomExit()
    {
        StartCoroutine(GetReadyToLeave());
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
            if(teamMode == false)
            {
                if (this.GetComponent<PhotonView>().ViewID == viewID[i])
                {
                    this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                    namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(true);
                    namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(true);
                    namesObject.GetComponent<NickNameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
                }
            }
            else if(teamMode == true)
            {
                if (this.GetComponent<PhotonView>().ViewID == viewID[i])
                {
                    this.transform.GetChild(1).GetComponent<Renderer>().material.color = teamColors[i];
                    namesObject.GetComponent<NickNameScript>().names[i].gameObject.SetActive(true);
                    namesObject.GetComponent<NickNameScript>().healthBars[i].gameObject.SetActive(true);
                    namesObject.GetComponent<NickNameScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
                }
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

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<Animator>().SetBool("Hit", false);
    }

    IEnumerator WaitToExit()
    {
        yield return new WaitForSeconds(3);
        RemoveMe();
        RoomExit();
    }
}
