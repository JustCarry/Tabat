using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;
public class GoalManager : MonoBehaviour
{
    [SerializeField] private int GoalSide;
    [SerializeField] private AudioSource goalEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag.Equals("ball")){
            if(PhotonNetwork.IsMasterClient){
                ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
                List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomPlayers"].ToString());
                if(currentRoomPlayers.Count >= 2){

                    Player p1 = GameManager.GetPlayerByIndex(currentRoomPlayers[0].playerIndex);
                    Player p2 = GameManager.GetPlayerByIndex(currentRoomPlayers[1].playerIndex);
                    ExitGames.Client.Photon.Hashtable p1Props = p1.CustomProperties;
                    ExitGames.Client.Photon.Hashtable p2Props = p2.CustomProperties;
                    if(GoalSide == 2){
                        p1Props["playerWins"] = (int)p1Props["playerWins"] + 1;
                        p2Props["playerLosses"] = (int)p2Props["playerLosses"] + 1;
                    } else {
                        p1Props["playerLosses"] = (int)p1Props["playerLosses"] + 1;
                        p2Props["playerWins"] = (int)p2Props["playerWins"] + 1;
                    }
                    goalEffect.Play();

                    BallMovement.Instance.ResetBall(GoalSide);
                    p1.SetCustomProperties(p1Props);
                    p2.SetCustomProperties(p2Props);

                    List<PlayerInfo> currentRoomQueue = JsonConvert.DeserializeObject<List<PlayerInfo>>(roomProps["roomQueue"].ToString());
                    if(currentRoomQueue.Count>0){
                        Player lostPlayer = null;
                        if(GoalSide == 1){
                            lostPlayer = p1;
                        } else {
                            lostPlayer = p2;
                        }
                        for(int i = 0; i < currentRoomPlayers.Count; i++){
                            if(currentRoomPlayers[i].playerIndex == (int) lostPlayer.CustomProperties["playerIndex"]){
                                currentRoomPlayers.RemoveAt(i);
                            }
                        }

                        PlayerInfo nextPlayer = currentRoomQueue[0];
                        PlayerInfo playerBackToQueue = new PlayerInfo(lostPlayer.UserId, (int) lostPlayer.CustomProperties["playerIndex"], lostPlayer.NickName);
                        currentRoomQueue.RemoveAt(0);
                        currentRoomQueue.Add(playerBackToQueue);

                        currentRoomPlayers.Add(nextPlayer);

                        roomProps["roomPlayers"] = JsonConvert.SerializeObject(currentRoomPlayers);
                        roomProps["roomQueue"] = JsonConvert.SerializeObject(currentRoomQueue);

                        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);



                    
                    }

                }
            }


        
            // Add a win to the other side, reset the ball

        }

    }
}
