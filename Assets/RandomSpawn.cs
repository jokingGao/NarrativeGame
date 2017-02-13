using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {
    //gameobject for spawning 
    public GameObject spawnObj;

    public GameObject SpawnPT1;
    public GameObject SpawnPT2;
    public GameObject SpawnPT3;
    public GameObject SpawnPT4;

    //set custom range for random position
    // public float MinX = 0;
    // public float MaxX = 10;
    // public float MinY = 0;
    // public float MaxY = 10;

    //for 3d you have z position
    
    

    void SpawnObject()
    {
        int x = Random.Range(1,4);
        Debug.Log(x);
        
        if(x == 1)
        {
            spawnObj.transform.position = SpawnPT1.transform.position;
        }
        else if(x == 2)
        {
            spawnObj.transform.position = SpawnPT2.transform.position;
        }
        else if(x == 3)
        {
            spawnObj.transform.position = SpawnPT3.transform.position;
        }
        else if(x == 4)
        {
            spawnObj.transform.position = SpawnPT4.transform.position;
        }
    }

    void Start()
    {
        SpawnObject();
    }
}
