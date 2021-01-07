using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
    
{

    private PhotonView PV;
    public Transform SpawnPoint;
    public TurnManager tm;
    public GameObject myPlayerObject;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnNumber = int.Parse(PhotonNetwork.LocalPlayer.NickName);
        tm = FindObjectOfType<TurnManager>();
        SpawnPoint = tm.ReturnSpawnPoint(spawnNumber -1);
        if (PV.IsMine)
        {
            myPlayerObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerPrefab"), SpawnPoint.position, SpawnPoint.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
