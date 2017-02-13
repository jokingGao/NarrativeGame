using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class PrincessBehaviorTree : MonoBehaviour
{
    public GameObject HERO;
    public GameObject PRINCESS;
    private BehaviorAgent behaviorAgent;

    // Use this for initialization
    void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

    }

    protected Node DuckToHero()
    {
        return new Sequence(PRINCESS.GetComponent<BehaviorMecanim>().Node_OrientTowardsPrincess(),
            PRINCESS.GetComponent<BehaviorMecanim>().Node_BodyAnimationPrincess("DUCK", true), new LeafWait(1000));
    }

    protected Node Kiss()
    {
        return new Sequence(PRINCESS.GetComponent<BehaviorMecanim>().Node_OrientTowardsPrincess(),
            PRINCESS.GetComponent<BehaviorMecanim>().Node_BodyAnimationPrincess("KISSING", true), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        return new DecoratorLoop(
            new Sequence(
                Kiss()));
    }



}
