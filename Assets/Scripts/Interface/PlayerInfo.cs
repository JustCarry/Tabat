using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
    public string playerId;
    public int playerIndex;
    public string playerName;


    public PlayerInfo(string id, int index, string name){
        playerId = id;
        playerIndex = index;
        playerName = name;
    }
}
