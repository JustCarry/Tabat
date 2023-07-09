using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;
using System.IO;

public class PlayerController : MonoBehaviourPunCallbacks
{
    
    PhotonView PV;

    // Start is called before the first frame update
    void Awake(){
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if(PV.IsMine){
            Input.gyro.enabled = true;
            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
            prop["playerIndex"] = GameManager.FindTheLowestNonUsedIndex();
            prop["playerWins"] = 0;
            prop["playerLosses"] = 0;

            PhotonNetwork.SetPlayerCustomProperties(prop);
            
            GameManager.AddLocalPlayer((int) prop["playerIndex"], PV.Owner.UserId, PV.Owner.NickName);

            CreatePlayerController((int) prop["playerIndex"]);
        }
    }
    void CreatePlayerController(int index){
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(index);
        GameObject thePlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player", "Player"), spawnPoint.position, spawnPoint.rotation);
        thePlayer.tag = "Player";
        PV.Owner.TagObject = thePlayer;
    
    }
}
