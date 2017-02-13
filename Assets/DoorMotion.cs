using UnityEngine;
using System.Collections;

public class DoorMotion : MonoBehaviour {

    public GameObject Door;
    Animator dooranimator;
    public bool MonsterKilled;
    public MyBehaviorTree tr;
    // Use this for initialization
    void Start () {
        dooranimator = Door.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        MonsterKilled = tr.GetComponent<MyBehaviorTree>().KillMonster;
        if (MonsterKilled)
        {
            dooranimator.SetBool("Open", true);
        }
			
    }
}
