using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks

{

    public static PhotonLobby lobby;
    public GameObject findGameButton;
    public GameObject cancelButton;

    private void Awake()
    {

        lobby = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();


    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected established to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        findGameButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFindGamePressed()
    {
        Debug.Log("Find Game Pressed");
        PhotonNetwork.JoinRandomRoom();
        findGameButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Join");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to Create");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public void OnCancelButtonPressed()
    {
        Debug.Log("Cancel Button Clicked");
        cancelButton.SetActive(false);
        findGameButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    
}
