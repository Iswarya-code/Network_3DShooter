using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AimLookAtRef : MonoBehaviour
{
    GameObject lookAtObject;
    // Start is called before the first frame update
    void Start()
    {
        lookAtObject = GameObject.Find("AimRef");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            this.transform.position = lookAtObject.transform.position;
        }
    }
}
