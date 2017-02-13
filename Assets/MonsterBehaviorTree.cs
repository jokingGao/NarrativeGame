using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class MonsterBehaviorTree : MonoBehaviour
{
    public GameObject HERO;
    public GameObject Monster;
    private BehaviorAgent behaviorAgent;

    // Use this for initialization
    void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

    }
	
	// Update is called once per frame
	void Update ()
	{
    }

    protected Node DeathByHero()
    {
        return new Sequence(Monster.GetComponent<BehaviorMecanim>().Node_OrientTowardsMonster(),
            Monster.GetComponent<BehaviorMecanim>().Node_BodyAnimationMonster("DYING", true), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
	{
        return new DecoratorLoop(
            new Sequence(new LeafWait(1000),
                DeathByHero()));      
    }
}
