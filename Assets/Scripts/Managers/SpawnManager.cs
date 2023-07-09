using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPositions;
    // Start is called before the first frame update
    public static SpawnManager Instance;
    void Start()
    {
        Instance = this;
        spawnPositions = GetComponentsInChildren<Transform>();
    }

    public Transform GetSpawnPoint(int position){
        return spawnPositions[position].transform;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
