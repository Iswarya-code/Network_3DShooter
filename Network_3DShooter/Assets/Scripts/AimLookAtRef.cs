using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AimLookAtRef : MonoBehaviour
{
    GameObject lookAtObject;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        lookAtObject = GameObject.Find("AimRef");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.gameObject.GetComponent<PhotonView>().IsMine == true && isDead == false)
        {
            this.transform.position = lookAtObject.transform.position;
        }
    }
}
