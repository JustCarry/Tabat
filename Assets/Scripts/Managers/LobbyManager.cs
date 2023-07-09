using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;
using Photon.Realtime;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private Button playButton;
    [SerializeField] private Button findRoomButton;

    [SerializeField] private TMP_InputField roomCodeInput;


    [SerializeField] private Button joinRoomByCodeButton;
    [SerializeField] private Button createRoomButton;

    private static System.Random _random = new System.Random();

    private bool MatchMaking = false;

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayerClicksPlay);
        findRoomButton.onClick.AddListener(OnPlayerClicksFindRoom);
        playerName.onValueChanged.AddListener(OnPlayerChangeName);
        joinRoomByCodeButton.onClick.AddListener(JoinRoomById);
        createRoomButton.onClick.AddListener(CreateRoom);

        PhotonNetwork.GameVersion = "v0.1";

        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
        Debug.Log("Connected!");
        MenuManager.Instance.OpenLobbyMenu();
    }


    /// Lobby.


    private void OnPlayerClicksPlay(){
        if(playerName.text.Length >= 3){
            PhotonNetwork.NickName = playerName.text;
            // MenuManager.Instance.OpenRoomsMenu();
            
            MatchMaking = true;
            
            PhotonNetwork.JoinRandomRoom();  
            CallAfterDelay.Create( 5.0f,  () => {
                if(MatchMaking){
                    CreateRoom();
                    MatchMaking = false;
                }
            });
            MenuManager.Instance.Loading();       
        }
    }

    private void OnPlayerClicksFindRoom(){
        if(playerName.text.Length >= 3){
            PhotonNetwork.NickName = playerName.text;
            MenuManager.Instance.OpenRoomsMenu();
        }
    }
    private void OnPlayerChangeName(string value){
        if( value.Length >= 14 ){
            playerName.text = value.Substring(0, 13);
        }
    }


    /// Room lobby;

    public void JoinRoomById(){
        string roomId = roomCodeInput.text;
        if(roomId.Length == 6){
            if( Regex.IsMatch(roomId, "^[a-zA-Z0-9]*$") ) {
                PhotonNetwork.JoinRoom(roomId);
                MenuManager.Instance.Loading();
            }
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        if(MatchMaking){
            CreateRoom();
            MatchMaking = false;
        } else {
            MenuManager.Instance.OpenRoomsMenu();
        }

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(){
        
        if(playerName.text.Length >= 3){
            PhotonNetwork.NickName = playerName.text;
            ExitGames.Client.Photon.Hashtable roomSettings = new ExitGames.Client.Photon.Hashtable();

            List<PlayerInfo> players = new List<PlayerInfo>();
            List<PlayerInfo> queue = new List<PlayerInfo>();
            


            roomSettings.Add("roomName", PhotonNetwork.NickName);
            roomSettings.Add("roomState", 0);
            roomSettings.Add("roomPlayers", JsonConvert.SerializeObject(players));
            roomSettings.Add("roomQueue", JsonConvert.SerializeObject(queue));

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 10;
            // roomOptions.PlayerTtl = 0;
            roomOptions.CustomRoomProperties = roomSettings;
            roomOptions.CleanupCacheOnLeave = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            roomOptions.CustomRoomPropertiesForLobby = new string[4] { "roomName", "roomState", "roomPlayers", "roomQueue" };

            PhotonNetwork.CreateRoom(RandomString(6).ToLower(), roomOptions);
            
            MenuManager.Instance.Loading();
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
        
    }

    // public override void OnRoomListUpdate(List<RoomInfo> roomList)
    // {
    //     foreach (Transform trans in roomListParent){
    //         Destroy(trans.gameObject);
    //     }
    //     for(int i = 0; i < roomList.Count; i++){
    //         if( roomList[i].RemovedFromList )
    //             continue;
    //         int state = (int)roomList[i].CustomProperties["roomState"];
    //         if( state == 0 )
    //             Instantiate(roomListItemPrefab, roomListParent).GetComponent<GameItem>().SetUp(roomList[i]);
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
        
    }

    

}
