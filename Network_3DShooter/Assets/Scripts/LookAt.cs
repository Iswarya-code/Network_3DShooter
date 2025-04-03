using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LookAt : MonoBehaviour
{
    Vector3 worldPosition;
    Vector3 screenPosition;
    public GameObject crossHair;
    public Text nickNameText;

    private void Start()
    {
        Cursor.visible = false;
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 3f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;

        crossHair.transform.position = Input.mousePosition;
    }
}
