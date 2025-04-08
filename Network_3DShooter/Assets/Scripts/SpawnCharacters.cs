using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnCharacters : MonoBehaviour
{
    public GameObject character;
    public Transform[] spawnPoints;

    public GameObject[] Weapons;
    public Transform[] WeaponSpawnPoints;
    public float WeaponRespawnTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(character.name, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpwanWeaponsStart()
    {
        for(int i=0;i<Weapons.Length;i++)
        {
            PhotonNetwork.Instantiate(Weapons[i].name, WeaponSpawnPoints[i].position, WeaponSpawnPoints[i].rotation);
        }
    }
}
