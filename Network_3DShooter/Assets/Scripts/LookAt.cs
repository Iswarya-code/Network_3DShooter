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

   
    // Update is called once per frame
    void FixedUpdate()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 6f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;

        crossHair.transform.position = Input.mousePosition;
    }
}
