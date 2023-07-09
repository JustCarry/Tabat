using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;

public class GameLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button roomStateToggle;
    [SerializeField] private Sprite OpenRoomImage;    
    [SerializeField] private Sprite CloseRoomImage;
    [SerializeField] private Button leaveRoom;
    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerListItem;
    [SerializeField] private TMP_Text roomCode;
    private bool roomState = true;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient){
            roomStateToggle.gameObject.SetActive(true);
            roomStateToggle.onClick.AddListener(() => {
                ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
                if(roomState){
                    roomStateToggle.image.sprite = OpenRoomImage;
                    roomState = false;
                    roomProps["roomState"] = 1;
                } else {
                    roomStateToggle.image.sprite = CloseRoomImage;
                    roomState = true;
                    roomProps["roomState"] = 0;
                }
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            });
        } else {
            
            roomStateToggle.gameObject.SetActive(false);
        }
        roomCode.text = "Code: " + PhotonNetwork.CurrentRoom.Name.ToUpper();
        playerCount.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount;
        leaveRoom.onClick.AddListener( () => {
            PhotonNetwork.LeaveRoom();

            PhotonNetwork.LoadLevel(0);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        playerCount.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount;


        UpdatePlayerList();
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        playerCount.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount;
        
        UpdatePlayerList();

        // Check if the player was playing.
        if(PhotonNetwork.IsMasterClient){
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

            List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomPlayers"].ToString());

            List<PlayerInfo> currentRoomQueue = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomQueue"].ToString());

            for(int i = 0; i < currentRoomPlayers.Count; i++){
                if(otherPlayer.CustomProperties != null && currentRoomPlayers[i].playerIndex == (int) otherPlayer.CustomProperties["playerIndex"]){
                    currentRoomPlayers.RemoveAt(i);
                }
            }

            if(currentRoomQueue.Count >= 1) {
                
                PlayerInfo nextPlayer = currentRoomQueue[0];

                currentRoomQueue.RemoveAt(0);

                currentRoomPlayers.Add(nextPlayer);


            } else {
                if( currentRoomPlayers.Count <= 1){

                    BallMovement.Instance.StopBall();

                }
            }
            
            roomProps["roomPlayers"] = JsonConvert.SerializeObject(currentRoomPlayers);
            roomProps["roomQueue"] = JsonConvert.SerializeObject(currentRoomQueue);

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            



        }

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if(PhotonNetwork.IsMasterClient){
            
            roomStateToggle.gameObject.SetActive(true);
            roomStateToggle.onClick.AddListener(() => {
                ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
                if(roomState){
                    roomStateToggle.image.sprite = OpenRoomImage;
                    roomState = false;
                    roomProps["roomState"] = 1;
                } else {
                    roomStateToggle.image.sprite = CloseRoomImage;
                    roomState = true;
                    roomProps["roomState"] = 0;
                }
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            });
        }
    }

    
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        UpdatePlayerList();
        
    }

    void UpdatePlayerList(){
        

        ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        
        List<PlayerInfo> currentRoomQueue = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomQueue"].ToString());

        if(currentRoomQueue.Count > 0 ){
            foreach (Transform trans in playerListParent){
                Destroy(trans.gameObject);
            }
            for(int i = 0; i < currentRoomQueue.Count; i++){
                Instantiate(playerListItem, playerListParent).GetComponent<PlayerItemScript>().SetUp(GameManager.GetPlayerByIndex(currentRoomQueue[i].playerIndex));
            }
        }


    }
}
