using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponPickUps : MonoBehaviour
{
     AudioSource audioPlayer;
    public float reSpawnTime = 5;

    public int WeaponType = 1;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            this.GetComponent<PhotonView>().RPC("PlayerPickUpAudio", RpcTarget.All);
            this.GetComponent<PhotonView>().RPC("TurnOff", RpcTarget.All);

        }
    }

    [PunRPC]
    void PlayerPickUpAudio()
    {
        audioPlayer.Play();
    }

    [PunRPC]
    void TurnOff()
    {
        if(WeaponType == 1)
        {
            this.transform.gameObject.GetComponent<Renderer>().enabled = false;
            this.transform.gameObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.gameObject.GetComponent<Collider>().enabled = false;

        }

        StartCoroutine(WaitRespawn());

    }

    IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(reSpawnTime);
        this.GetComponent<PhotonView>().RPC("TurnOn", RpcTarget.All);

    }

    [PunRPC]
    void TurnOn()
    {
        if (WeaponType == 1)
        {
            this.transform.gameObject.GetComponent<Renderer>().enabled = true;
            this.transform.gameObject.GetComponent<Collider>().enabled = true;
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.gameObject.GetComponent<Collider>().enabled = true;
        }

    }
}
