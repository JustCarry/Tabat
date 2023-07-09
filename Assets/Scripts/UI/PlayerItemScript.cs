using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerItemScript : MonoBehaviour
{
    [SerializeField] private Image playerTabh;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerWins;
    [SerializeField] private Button playerKick;

    public void SetUp(Photon.Realtime.Player player){
        if(PhotonNetwork.IsMasterClient){
            playerKick.gameObject.SetActive(true);
        } else {
            playerKick.gameObject.SetActive(false);
        }
        

        playerTabh.sprite = Resources.Load<Sprite>("Tabat/Tab_"+player.CustomProperties["playerIndex"].ToString() );
        
        playerName.text = player.NickName;
        playerWins.text = "W/L: " + (int) player.CustomProperties["playerWins"] +" /" + (int) player.CustomProperties["playerLosses"];

    }


}
