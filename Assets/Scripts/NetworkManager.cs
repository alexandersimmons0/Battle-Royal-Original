using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks{
    public int maxPlayers;
    public static NetworkManager instance;

    void Awake(){
        if(instance != null && instance != this){
            gameObject.SetActive(false);
        }else{
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start(){
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("connected to master");
    }

    public void CreateRoom(string roomName){
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName){
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnDisconnected(DisconnectCause cause){
        PhotonNetwork.LoadLevel("Menu");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        GameManager.instance.alivePlayers--;
        GameUI.instance.UpdatePlayerInfoText();
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.CheckWinCondition();
        }
    }
}
