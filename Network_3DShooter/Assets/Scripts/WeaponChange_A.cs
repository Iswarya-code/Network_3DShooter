using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Photon.Pun;
using UnityEngine.UI;

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

    Image weaponIcon;
    Text ammoAmtText;
    public Sprite[] weaponIcons;
    public int[] ammoAmts;
    //adding muzzleflash
    public GameObject[] muzzleFalsh;
    //shooting
    string shooterName;
    string gotShotName;
    public float[] damageAmts;


    // Start is called before the first frame update
    void Start()
    {
        weaponIcon = GameObject.Find("WeaponUI").GetComponent<Image>();
        ammoAmtText = GameObject.Find("AmmoAmt").GetComponent<Text>();

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
        if(Input.GetMouseButtonDown(0))
        {
            if(this.GetComponent<PhotonView>().IsMine == true)
            {
                GetComponent<DisplayColor>().PlayGunShot(GetComponent<PhotonView>().Owner.NickName, weaponNumber);
                this.GetComponent<PhotonView>().RPC("GunMuzzleFlash", RpcTarget.All);
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                if(Physics.Raycast(ray,out hit,500))
                {
                    if(hit.transform.gameObject.GetComponents<PhotonView>()!=null) //ignore when it hits gameobjects which doesn't have photonview component
                    {
                        gotShotName = hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName;
                    }
                    if(hit.transform.gameObject.GetComponent<DisplayColor>() != null)
                    {
                        hit.transform.gameObject.GetComponent<DisplayColor>().DeliverDamage(hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName, damageAmts[weaponNumber]);

                    }
                    shooterName = GetComponent<PhotonView>().Owner.NickName;
                    Debug.Log(gotShotName + " got hit by " + shooterName);
                }
                this.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        if(Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            // weaponNumber++;
            this.GetComponent<PhotonView>().RPC("Change", RpcTarget.AllBuffered);
            if(weaponNumber>weapons.Length -1)
            {
                weaponIcon.GetComponent<Image>().sprite = weaponIcons[0];
                ammoAmtText.text = ammoAmts[0].ToString();
                weaponNumber = 0;
            }
            for(int i=0;i<weapons.Length;i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            weaponIcon.GetComponent<Image>().sprite = weaponIcons[weaponNumber];
            ammoAmtText.text = ammoAmts[weaponNumber].ToString();

            leftHand.data.target = leftTargets[weaponNumber];
            rightHand.data.target = rightTargets[weaponNumber];
            rig.Build();


        }
    }

    [PunRPC]
    public void GunMuzzleFlash()
    {
        muzzleFalsh[weaponNumber].SetActive(true);
        StartCoroutine(MuzzleOff());
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

    IEnumerator MuzzleOff()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<PhotonView>().RPC("MuzzleFalshOff", RpcTarget.All);
       
    }

    [PunRPC]
    public void MuzzleFalshOff()
    {
        muzzleFalsh[weaponNumber].SetActive(false);
    }
}
