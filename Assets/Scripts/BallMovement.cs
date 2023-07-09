using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;
using TMPro;
public class BallMovement : MonoBehaviourPunCallbacks
{

    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private AudioSource bounce;
    private float thrust = 15000f;
    Vector3 LastVelocity;

    public bool isBallMoving = false;
    public static BallMovement Instance;

    [SerializeField] private TMP_Text Log;

    /// If there is only one player then stop the ball movement.. + reset it.
    void Start()
    {        
        Instance = this;
        // List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(PhotonNetwork.CurrentRoom.CustomProperties["roomPlayers"].ToString());

    }

    // Update is called once per frame
    void Update()
    {
        if(isBallMoving){
            LastVelocity = rb2d.velocity;
            Log.text = LastVelocity.normalized.ToString();
            if(LastVelocity.magnitude<300f){
                rb2d.velocity = LastVelocity.normalized * 300f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        var speed = LastVelocity.magnitude;
        var direction = Vector3.Reflect(LastVelocity.normalized, other.contacts[0].normal);
            Log.text = LastVelocity.normalized.ToString();
        if(direction.y > 0){
            direction = new Vector3(direction.x, Mathf.Clamp(direction.y,0.25f,0.85f), direction.z) ;
        } else {
            direction = new Vector3(direction.x, Mathf.Clamp(direction.y,-0.25f,-0.75f), direction.z) ;
        }
        rb2d.velocity = direction * Mathf.Max(speed*1.025f,0f);

        if(other.gameObject.GetComponent<PhotonView>().IsMine){
            float power = Input.acceleration.sqrMagnitude;
            if(power > 4f){
                rb2d.velocity = direction * Mathf.Max(speed*1.40f,0.5f);
            } else {
                if(power > 3f){
                    rb2d.velocity = direction * Mathf.Max(speed*1.30f,0.5f);
                } else {
                    
                    if(power > 2f){
                        rb2d.velocity = direction * Mathf.Max(speed*1.15f,0.5f);
                    }
                }
            }
        }
    }
    public void StopBall(){
        isBallMoving = false;
        rb2d.position = new Vector2(700f, 1600f);
        rb2d.velocity = Vector2.zero;
    }
    public void ResetBall(int side){
        
        rb2d.position = new Vector2(700f, 1600f);
        rb2d.velocity = Vector2.zero;
        CallAfterDelay.Create( 5.0f,  () => {
            if(side == 1){
                rb2d.AddForce(transform.right * thrust);
            } else {
                rb2d.AddForce(-transform.right * thrust);
            }
        });

    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if(PhotonNetwork.IsMasterClient){
            List<PlayerInfo> currentRoomPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(PhotonNetwork.CurrentRoom.CustomProperties["roomPlayers"].ToString());
            if(!isBallMoving && currentRoomPlayers.Count >= 2){
                isBallMoving = true;
                CallAfterDelay.Create( 5.0f,  () => {
                    rb2d.AddForce(transform.right * thrust);
                });
            }


        }
    }
}
