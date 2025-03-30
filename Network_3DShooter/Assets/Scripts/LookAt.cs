using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    Vector3 worldPosition;
    Vector3 screenPosition;
    public GameObject crossHair;

    private void Start()
    {
        Cursor.visible = false;
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
