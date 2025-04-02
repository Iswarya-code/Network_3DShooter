using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Photon.Pun;

public class WeaponChange_A : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand;
    public TwoBoneIKConstraint rightHand;
    public RigBuilder rig;
    public Transform[] leftTargets;
    public Transform[] rightTargets;
    public GameObject[] weapons;
    int weaponNumber = 0;
    GameObject TestForWeapons;

    CinemachineVirtualCamera cam;
    GameObject camObject;
    public MultiAimConstraint[] aimObjects;
     Transform aimTarget;

    // Start is called before the first frame update
    void Start()
    {
        camObject = GameObject.Find("PlayerCam");
       // aimTarget = GameObject.Find("AimRef").transform;
        if(this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            cam = camObject.GetComponent<CinemachineVirtualCamera>();
            cam.Follow = this.gameObject.transform;
            cam.LookAt = this.gameObject.transform;
           // Invoke("SetLookAt", 0.1f);
        }
        else
        {
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        TestForWeapons = GameObject.Find("Weapon1 PickUp(Clone)");
        if(TestForWeapons == null)
        {
            var spawner = GameObject.Find("SpawnScripts");
            spawner.GetComponent<SpawnCharacters>().SpwanWeaponsStart();
        }
    }

   /* void SetLookAt()
    {
        if(aimTarget != null)
        {
            for(int i = 0; i< aimObjects.Length; i++)
            {
                var target = aimObjects[i].data.sourceObjects;
                target.SetTransform(0, aimTarget.transform);
                aimObjects[i].data.sourceObjects = target;
            }
            rig.Build();
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            // weaponNumber++;
            this.GetComponent<PhotonView>().RPC("Change", RpcTarget.AllBuffered);
            if(weaponNumber>weapons.Length -1)
            {
                weaponNumber = 0;
            }
            for(int i=0;i<weapons.Length;i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            leftHand.data.target = leftTargets[weaponNumber];
            rightHand.data.target = rightTargets[weaponNumber];
            rig.Build();


        }
    }

    [PunRPC]

    public void Change()
    {
         weaponNumber++;
        if (weaponNumber > weapons.Length - 1)
        {
            weaponNumber = 0;
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[weaponNumber].SetActive(true);
        leftHand.data.target = leftTargets[weaponNumber];
        rightHand.data.target = rightTargets[weaponNumber];
        rig.Build();
    }
}
