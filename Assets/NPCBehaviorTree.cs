using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class NPCBehaviorTree : MonoBehaviour
{
    public GameObject NPC;
    public GameObject Hero;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    protected Node PerformAction(Transform target)
    {
        return new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_HandAnimationNPC("CALLOVER", true), new LeafWait(1000));
    }

    protected Node PerformAction2(Transform target)
    {
        return new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_HandAnimationNPC2("WAVE", true), new LeafWait(1000));
    }

    protected Node StareAtHero(Transform target)
    {
        print(target.position);
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        return
            new DecoratorLoop(
                new SequenceShuffle(
                    this.StareAtHero(this.Hero.transform),
                    this.PerformAction(this.Hero.transform),
                    this.PerformAction2(this.Hero.transform),
                    this.StareAtHero(this.Hero.transform)));
    }
}