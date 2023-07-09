using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Newtonsoft.Json;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text player1Name;
    [SerializeField] private TMP_Text player1Wins;

    [SerializeField] private TMP_Text player2Name;
    [SerializeField] private TMP_Text player2Wins;


    // Start is called before the first frame update
    void Awake(){

        




    }
    void Update()
    {


    }

    public static void AddLocalPlayer(int index, string UserId, string NickName){
        ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomPlayers"].ToString());
        List<PlayerInfo> currentRoomQueue = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomQueue"].ToString());

        PlayerInfo currentPlayer = new PlayerInfo(UserId, index, NickName);
        if(currentRoomPlayers.Count < 2 ){
            currentRoomPlayers.Add(currentPlayer);
            roomProps["roomPlayers"] = JsonConvert.SerializeObject(currentRoomPlayers);
        } else {
            currentRoomQueue.Add(currentPlayer);
            roomProps["roomQueue"] = JsonConvert.SerializeObject(currentRoomQueue);
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
    }


    public static int FindTheLowestNonUsedIndex(){
        int lowestFound = 1;
        for(int i = 1; i <= 10; i++){
            bool found = false;
            foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList){
                if(player.CustomProperties != null && player.CustomProperties["playerIndex"] != null && i == (int)player.CustomProperties["playerIndex"]){
                    found = true;
                }
            }
            if(found){
                continue;
            } else {
                lowestFound = i;
                break;
            }
        }
        return lowestFound;

    }

    public static Player GetPlayerByIndex(int index){
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList){
            if(player.CustomProperties != null && player.CustomProperties["playerIndex"] != null && index == (int)player.CustomProperties["playerIndex"]){
                return player;
            }
        }
        return null;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        

        ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        
        List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomPlayers"].ToString());

        Player player1 = null;
        Player player2 = null;
        if(currentRoomPlayers.Count >= 2){
            player1 = GetPlayerByIndex(currentRoomPlayers[0].playerIndex);
            player2 = GetPlayerByIndex(currentRoomPlayers[1].playerIndex);
        } else{
            if(currentRoomPlayers.Count == 1){
               player1 = GetPlayerByIndex(currentRoomPlayers[0].playerIndex);
            }
        }
        if(player1 != null ){
            player1Name.text = player1.NickName;
            player1Wins.text = "Wins: " + player1.CustomProperties["playerWins"].ToString();
        } else {
            player1Name.text = "Waiting...";
            player1Wins.text = "";
        }

        if(player2 != null ){
            player2Name.text = player2.NickName;
            player2Wins.text = "Wins: " + player2.CustomProperties["playerWins"].ToString();
        } else {
            player2Name.text = "Waiting...";
            player2Wins.text = "";
        }
    }    
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);


        ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        
        List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomPlayers"].ToString());
        List<PlayerInfo> currentRoomQueue = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomQueue"].ToString());
        Player player1 = null;
        Player player2 = null;

        if(currentRoomPlayers.Count >= 2){
            player1 = GetPlayerByIndex(currentRoomPlayers[0].playerIndex);
            player2 = GetPlayerByIndex(currentRoomPlayers[1].playerIndex);
        } else{
            if(currentRoomPlayers.Count == 1){
               player1 = GetPlayerByIndex(currentRoomPlayers[0].playerIndex);
            }
        }
        if(player1 != null ){
            player1Name.text = player1.NickName;
            player1Wins.text = "Wins: " + player1.CustomProperties["playerWins"].ToString();

            GameObject p1Tabh = player1.TagObject as GameObject;
            p1Tabh.transform.position = SpawnManager.Instance.GetSpawnPoint(1).position;
            p1Tabh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tabat/Tab_"+player1.CustomProperties["playerIndex"].ToString() );
            

        } else {
            player1Name.text = "Waiting...";
            player1Wins.text = "";
        }

        if(player2 != null ){
            player2Name.text = player2.NickName;
            player2Wins.text = "Wins: " + player2.CustomProperties["playerWins"].ToString();

            GameObject p2Tabh = player2.TagObject as GameObject;
            p2Tabh.transform.position = SpawnManager.Instance.GetSpawnPoint(2).position;
            p2Tabh.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tabat/Tab_"+player2.CustomProperties["playerIndex"].ToString() );

        } else {
            player2Name.text = "Waiting...";
            player2Wins.text = "";
        }

        if(currentRoomQueue.Count > 0){
            
            for(int i = 0; i < currentRoomQueue.Count; i++){
                
                Player mPlayer = GetPlayerByIndex(currentRoomQueue[i].playerIndex);
                GameObject pTabh = mPlayer.TagObject as GameObject;
                pTabh.transform.position = SpawnManager.Instance.GetSpawnPoint(3+i).position;
            }
        }

    }
    
}
