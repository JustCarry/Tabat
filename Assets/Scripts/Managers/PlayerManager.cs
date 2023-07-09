using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Newtonsoft.Json;
using Photon.Realtime;
using Photon.Voice.PUN;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{

    private float maxY = 2535f;
    private float minY = 820f;
    // [SerializeField] private AudioSource bounce;
    [SerializeField] private SpriteRenderer playerTabh;
    [SerializeField] private GameObject speaking;
    
    PhotonView PV;
    
    PhotonVoiceView PVV;

    // Start is called before the first frame update
    void Awake(){
        PV = GetComponent<PhotonView>();
        // playerTabh = GetComponent<SpriteRenderer>();

        PVV = GetComponent<PhotonVoiceView>();
        
    }
    void Start()
    {
        Input.gyro.enabled = true;

    }
    void Update(){
        if(PVV.IsRecording){
            if(!speaking.activeSelf){
                speaking.SetActive(true);
            }
        } else {
            if(speaking.activeSelf){
                speaking.SetActive(false);
            }
        }

        if (!PV.IsMine){
            return;
        }
        
        Movement();
    }

    
    void Movement(){
        float yMove =  (-Input.acceleration.x) * 1500f;


        if(transform.position.y >= maxY) { 
            if(yMove < 0 ){
                transform.Translate(0, yMove * Time.deltaTime, 0);
            }
        } else {
            if( transform.position.y <= minY ){
                if(yMove > 0 ){
                    transform.Translate(0, yMove * Time.deltaTime, 0);
                }
            } else {
                transform.Translate(0, yMove * Time.deltaTime, 0);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if(targetPlayer == PV.Owner){
            // if(PhotonNetwork.C)
            playerTabh.sprite = Resources.Load<Sprite>("Tabat/Tab_"+PhotonNetwork.LocalPlayer.CustomProperties["playerIndex"].ToString() );

        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info){
        //Assign this gameObject to player called instantiate the prefab
        info.Sender.TagObject = this.gameObject;
    }
}
