using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;


public class BoxmanBehaviorTree : MonoBehaviour
{
    public GameObject HERO;
    public GameObject Boxman;
    private BehaviorAgent behaviorAgent;

    public Transform pin; // The hitting point as in the animation
    public AimIK aimIK; // Reference to the AimIK component

    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

    }

    void LateUpdate()
    {
        aimIK.solver.transform.LookAt(pin.position);

        // Set myself as IK target
        aimIK.solver.IKPosition = HERO.transform.position;
    }

    protected Node HitByHero()
    {
        return new Sequence(Boxman.GetComponent<BehaviorMecanim>().Node_OrientTowardsBoxman(),
            Boxman.GetComponent<BehaviorMecanim>().Node_BodyAnimationBoxing("BOXING", true),
            Boxman.GetComponent<BehaviorMecanim>().Node_BodyAnimationBoxingDying("DYING", true), new LeafWait(1));
    }

    protected Node BuildTreeRoot()
    {
        return new DecoratorLoop(
            new Sequence(new LeafWait(1),
                HitByHero()));
    }
}
