using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;

    void Start(){
        // List<RoomInfo> roomList = PhotonNetwork.GetCustomRoomList();
        // foreach (Transform trans in roomListParent){
        //     Destroy(trans.gameObject);
        // }
        // Debug.Log(roomList.Count);
        // for(int i = 0; i < roomList.Count; i++){
        //     if( roomList[i].RemovedFromList )
        //         continue;
        //     int state = (int)roomList[i].CustomProperties["roomState"];
        //     if( state == 0 )
        //         Instantiate(roomListItemPrefab, roomListParent).GetComponent<RoomItemScript>().SetUp(roomList[i]);
        // }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach (Transform trans in roomListParent){
            Destroy(trans.gameObject);
        }
        Debug.Log(roomList.Count);
        for(int i = 0; i < roomList.Count; i++){
            if( roomList[i].RemovedFromList )
                continue;
            int state = (int)roomList[i].CustomProperties["roomState"];
            if( state == 0 )
                Instantiate(roomListItemPrefab, roomListParent).GetComponent<RoomItemScript>().SetUp(roomList[i]);
        }
    }
}
