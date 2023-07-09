using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class RoomItemScript : MonoBehaviour{
    private string roomId;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomCount;
    [SerializeField] private Button roomJoinButton;

    public void SetUp(Photon.Realtime.RoomInfo room){
        roomId = room.Name;
        roomName.text = (string) room.CustomProperties["roomName"];
        roomCount.text = room.PlayerCount + " / 6";
        roomJoinButton.onClick.AddListener( () => {
            PhotonNetwork.JoinRoom(room.Name);
        });
    }




}
