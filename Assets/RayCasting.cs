using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TreeSharpPlus;

public class RayCasting : MonoBehaviour {
    NavMeshAgent agent;
    Animator animator;
    bool doorOpen;
    bool doorClose;
    bool doorIdle;

    private GameObject agentToMove;
    private BehaviorAgent behaviorAgent;
    public GameObject HERO;
    public GameObject buttonDoor1;
    public GameObject MovingDoor;
    public GameObject MONSTER;
    public GameObject BOXMAN;
    public GameObject NPC;
    public GameObject buttonDoor1Border;
    public GameObject circle;
    public GameObject circle_around_dude;
    private bool NPCselected;
    private Vector3 participantLocation;
    float maxspeed = 10f;

    private Vector3 vector;
    // Use this for initialization
    void Start () {
        doorOpen = false;
        animator = MovingDoor.GetComponent<Animator>();
        participantLocation = NPC.transform.position;
        NPCselected = false;
      
    }
	
	// Update is called once per frame
	void Update () {

        // Debug.Log(HERO.transform.position);
        // Debug.Log("NPCSelected: " + NPCselected);
        // Debug.Log(behaviorAgent.StopBehavior());
        //behaviorAgent.StopBehavior();
       // Debug.Log("Difference in Positions: " + (participantLocation - HERO.transform.position).magnitude);


        //if (NPCselected == true)
        //{
        //    behaviorAgent.StopBehavior();
        //}
        


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 60, Color.cyan);

        
        if(Input.GetButtonDown("Fire1"))
        {
            if(Physics.Raycast(ray,out hit) == true)
            {
                if (hit.transform.gameObject.CompareTag("NPC"))
                {
                    vector = hit.point;
                    circle_around_dude.transform.position = vector;
                    circle_around_dude.SetActive(true);
                    Invoke("DontShowCircle2", 1);
                    participantLocation = gameObject.transform.position;


                }

            }
        }
        
        
        if (Input.GetButtonDown("Fire2") || Input.GetButton("Fire2"))
        {
            if (Physics.Raycast(ray, out hit) == true)
            {
                if(hit.transform.gameObject.CompareTag("Ground"))
                {
                    vector = hit.point;
                    circle.transform.position = vector; 
                    circle.SetActive(true);
                    Invoke("DontShowCircle", 1);
                    agentToMove = GameObject.Find("Hero");
                    agentToMove.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                }

                if (hit.transform.gameObject.CompareTag("NPC"))
                {
                    NPCselected = true;
                }
            }
        }
        //Debug.Log(Time.deltaTime);
    }

    protected Node ReadWithRightHand(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimation("REACHRIGHT", true), new LeafWait(1000));
    }

    void DontShowCircle2()
    {
        circle_around_dude.SetActive(false);
    }

    void DontShowCircle()
    {
        circle.SetActive(false);
    }

    protected Node KickMonster(Transform target)
    {
        print("33333");
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT", true), new LeafWait(1000));
    }

    
}
