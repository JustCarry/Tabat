using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RoomManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    // public static RoomManager Instance;
    void Awake(){
        // if(Instance){
        //     Destroy(gameObject);
        //     return;
        // }
        // DontDestroyOnLoad(gameObject);
        // Instance = this;
        // Debug.Log("CREATED");
    }
    void Start()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
