using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class HeroBehaviorTree : MonoBehaviour
{
    public GameObject HERO;
    private BehaviorAgent behaviorAgent;
    private Transform target;

    // Use this for initialization
    void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

    }

    protected Node PressButton()
    {
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_OrientTowardsButton(), HERO.GetComponent<BehaviorMecanim>().Node_HandAnimationButton("REACHRIGHT", true), new LeafWait(1));
    }

    protected Node WaveToNPC()
    {
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_HandAnimationNPC2("WAVE", true), new LeafWait(1));
    }

    protected Node WaveToPrincess()
    {
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_OrientTowardsPrincess(),
            HERO.GetComponent<BehaviorMecanim>().Node_HandAnimationPrincess("WAVE", true), new LeafWait(1));
    }

    protected Node KillMonster()
    {
        print("KILL");
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_OrientTowardsMonster(),
            HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimationKillMonster("KILL_MONSTER", true), new LeafWait(1));
    }

    protected Node KillBoxman()
    {
        print("boxman");
        return new Sequence(HERO.GetComponent<BehaviorMecanim>().Node_OrientTowardsBoxman(),
            HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimationKillBoxman("KILL_MONSTER", true), new LeafWait(1));
    }

    protected Node PickUpSword()
    {
        print("picksword");
        return new Sequence(
            HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimationSword("PICKUP_SWORD", true), new LeafWait(1));
    }

    protected Node Kiss()
    {
        print("kiss");
        return new Sequence(
            HERO.GetComponent<BehaviorMecanim>().Node_BodyAnimationKiss("KISSING", true), new LeafWait(1));
    }

    protected Node BuildTreeRoot()
    {
        print(HERO.transform.position);
        return new DecoratorLoop(
            new Sequence(
                WaveToNPC(),
                KillMonster(),
                KillBoxman(),
                WaveToPrincess(),
                PressButton(),
                PickUpSword(),
                Kiss()));
    }
}
