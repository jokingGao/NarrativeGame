using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

using RootMotion.FinalIK;

public enum AnimationLayer
{
    Hand,
    Body,
    Face,
}

public class BehaviorMecanim : MonoBehaviour
{
    [HideInInspector]
    public CharacterMecanim Character = null;

    void Awake() { this.Initialize(); }

    protected void Initialize()
    {
        this.Character = this.GetComponent<CharacterMecanim>();
    }

    protected void StartTree(
        Node root,
        BehaviorObject.StatusChangedEventHandler statusChanged = null)
    {
    }

    #region Helper Nodes

    #region Navigation
    /// <summary>
    /// Approaches a target
    /// </summary>
    public Node Node_GoTo(Val<Vector3> targ)
    {
        return new LeafInvoke(
            () => this.Character.NavGoTo(targ),
            () => this.Character.NavStop());
    }

    public Node Node_GoToPrincess()
    {
        return new LeafInvoke(
            () => this.Character.NavGoToPrincess(),
            () => this.Character.NavStopPrincess());
    }

    public Node Node_NudgeTo(Val<Vector3> targ)
    {
        return new LeafInvoke(
            () => this.Character.NavNudgeTo(targ),
            () => this.Character.NavStop());
    }

    //public Node Node_GoAlongPoints(Val<Vector3>[] targ)
    //{
    //    return new LeafInvoke(
    //        () => this.Character.NavAlongPoints(targ),
    //        () => this.Character.NavStop());
    //}

    // TODO: No speed support yet! - AS 5/4/14
    ///// <summary>
    ///// Approaches a target with a certain speed
    ///// </summary>
    //public Node Node_GoTo(Val<Vector3> targ, Val<float> speed)
    //{
    //    this.Character.SetSpeed(speed.Value);
    //    return new LeafInvoke(
    //        () => this.Character.NavGoTo(targ),
    //        () => this.Character.NavStop());
    //}

    /// <summary>
    /// Orient towards a target position
    /// </summary>
    /// <param name="targ"></param>
    /// <returns></returns>
    public Node Node_OrientTowards(Val<Vector3> targ)
    {
        return new LeafInvoke(
            () => this.Character.NavTurn(targ),
            () => this.Character.NavOrientBehavior(
                OrientationBehavior.LookForward));
    }

    public Node Node_OrientTowardsBoxman()
    {
        return new LeafInvoke(
            () => this.Character.NavTurnBoxman(),
            () => this.Character.NavOrientBehaviorBoxman(
                OrientationBehavior.LookForward));
    }

    public Node Node_OrientTowardsMonster()
    {
        return new LeafInvoke(
            () => this.Character.NavTurnMonster(),
            () => this.Character.NavOrientBehaviorMonster(
                OrientationBehavior.LookForward));
    }

    public Node Node_OrientTowardsButton()
    {
        return new LeafInvoke(
            () => this.Character.NavTurnButton(),
            () => this.Character.NavOrientBehaviorButton(
                OrientationBehavior.LookForward));
    }

    public Node Node_OrientTowardsPrincess()
    {
        return new LeafInvoke(
            () => this.Character.NavTurnPrincess(),
            () => this.Character.NavOrientBehaviorPrincess(
                OrientationBehavior.LookForward));
    }

    /// <summary>
    /// Orient towards a target position
    /// </summary>
    /// <param name="targ"></param>
    /// <returns></returns>
    public Node Node_Orient(Val<Quaternion> direction)
    {
        return new LeafInvoke(
            () => this.Character.NavTurn(direction),
            () => this.Character.NavOrientBehavior(
                OrientationBehavior.LookForward));
    }

    /// <summary>
    /// Approaches a target at a given radius
    /// </summary>
    public Node Node_GoToUpToRadius(Val<Vector3> targ, Val<float> dist)
    {
        Func<RunStatus> GoUpToRadius =
            delegate()
            {
                Vector3 targPos = targ.Value;
                Vector3 curPos = this.transform.position;
                if ((targPos - curPos).magnitude < dist.Value)
                {
                    this.Character.NavStop();
                    return RunStatus.Success;
                }
                return this.Character.NavGoTo(targ);
            };

        return new LeafInvoke(
            GoUpToRadius,
            () => this.Character.NavStop());
    }
    #endregion

    #region Reach
    /// <summary>
    /// Tries to reach target with right or left hand.
    /// </summary>
    public Node Node_StartInteraction(Val<FullBodyBipedEffector> effector, Val<InteractionObject> obj)
    {
        return new LeafInvoke(
            () => this.Character.StartInteraction(effector, obj),
            () => this.Character.StopInteraction(effector));
    }

    /// <summary>
    /// Tries to reach target with right or left hand.
    /// </summary>
    public Node Node_ResumeInteraction(Val<FullBodyBipedEffector> effector)
    {
        return new LeafInvoke(
            () => this.Character.ResumeInteraction(effector),
            () => this.Character.StopInteraction(effector));
    }

    /// <summary>
    /// Stops reaching the target for the specified hand.
    /// </summary>
    public Node Node_StopInteraction(Val<FullBodyBipedEffector> effector)
    {
        return new LeafInvoke(
            () => this.Character.StopInteraction(effector));
    }

    /// <summary>
    /// Waits for a trigger on a specific effector
    /// </summary>
    public Node Node_WaitForTrigger(Val<FullBodyBipedEffector> effector)
    {
        return new LeafInvoke(
            () => this.Character.WaitForTrigger(effector));
    }

    /// <summary>
    /// Waits for a specific effector to finish its interaction
    /// </summary>
    public Node Node_WaitForFinish(Val<FullBodyBipedEffector> effector)
    {
        return new LeafInvoke(
            () => this.Character.WaitForFinish(effector));
    }
    #endregion

    #region HeadLook
    public Node Node_HeadLook(Val<Vector3> targ)
    {
        return new LeafInvoke(
                () => this.Character.HeadLookAt(targ),
                () => this.Character.HeadLookStop());
    }

    public Node Node_HeadLookTurnFirst(Val<Vector3> targ)
    {
        return new Sequence(
            new LeafInvoke(() => this.Character.NavTurn(targ)),
            Node_HeadLook(targ));
    }

    public Node Node_HeadLookStop()
    {
        return new LeafInvoke(
            () => this.Character.HeadLookStop());
    }

    #endregion

    #region Animation
    /// <summary>
    /// A Hand animation is started if the bool is true, the hand animation 
    /// is stopped if the bool is false
    /// </summary>
    public Node Node_HandAnimation(Val<string> gestureName, Val<bool> start)
    {

        return new LeafInvoke(
            () => this.Character.HandAnimation(gestureName, start),
            () => this.Character.HandAnimation(gestureName, false));
    }

    public Node Node_HandAnimationNPC(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.HandAnimationNPC(gestureName, start),
            () => this.Character.HandAnimationNPC(gestureName, false));
    }

    public Node Node_HandAnimationNPC2(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.HandAnimationNPC2(gestureName, start),
            () => this.Character.HandAnimationNPC2(gestureName, false));
    }

    public Node Node_HandAnimationButton(Val<string> gestureName, Val<bool> start)
    {

        return new LeafInvoke(
            () => this.Character.HandAnimationButton(gestureName, start),
            () => this.Character.HandAnimationButton(gestureName, false));
    }

    public Node Node_BodyAnimationKillBoxman(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationKillBoxman(gestureName, start),
            () => this.Character.BodyAnimationKillBoxman(gestureName, false));
    }

    public Node Node_BodyAnimationKillMonster(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationKillMonster(gestureName, start),
            () => this.Character.BodyAnimationKillMonster(gestureName, false));
    }

    public Node Node_BodyAnimationKillMonsterWithoutInteract(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationKillMonsterWithoutInteract(gestureName, start),
            () => this.Character.BodyAnimationKillMonsterWithoutInteract(gestureName, false));
    }

    public Node Node_HandAnimationPrincess(Val<string> gestureName, Val<bool> start)
    {

        return new LeafInvoke(
            () => this.Character.HandAnimationPrincess(gestureName, start),
            () => this.Character.HandAnimationPrincess(gestureName, false));
    }

    /// <summary>
    /// A Face animation is started if the bool is true, the face animation 
    /// is stopped if the bool is false
    /// </summary>
    public Node Node_FaceAnimation(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.FaceAnimation(gestureName, start),
            () => this.Character.FaceAnimation(gestureName, false));
    }

    public Node Node_BodyAnimation(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimation(gestureName, start),
            () => this.Character.BodyAnimation(gestureName, false));
    }

    public Node Node_BodyAnimationButton(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationButton(gestureName, start),
            () => this.Character.BodyAnimationButton(gestureName, false));
    }

    public Node Node_BodyAnimationSword(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationSword(gestureName, start),
            () => this.Character.BodyAnimationSword(gestureName, false));
    }

    public Node Node_BodyAnimationSwordWithoutInteract(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationSwordWithoutInteract(gestureName, start),
            () => this.Character.BodyAnimationSwordWithoutInteract(gestureName, false));
    }

    public Node Node_BodyAnimationKiss(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationKiss(gestureName, start),
            () => this.Character.BodyAnimationKiss(gestureName, false));
    }

    public Node Node_BodyAnimationKissWithoutInteract(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationKissWithoutInteract(gestureName, start),
            () => this.Character.BodyAnimationKissWithoutInteract(gestureName, false));
    }

    public Node Node_BodyAnimationBoxing(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationBoxing(gestureName, start),
            () => this.Character.BodyAnimationBoxing(gestureName, false));
    }

    public Node Node_BodyAnimationBoxingDying(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationBoxingDying(gestureName, start),
            () => this.Character.BodyAnimationBoxingDying(gestureName, false));
    }

    public Node Node_BodyAnimationMonster(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationMonster(gestureName, start),
            () => this.Character.BodyAnimationMonster(gestureName, false));
    }


    public Node Node_BodyAnimationMonsterWithoutInteract(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationMonsterWithoutInteract(gestureName, start),
            () => this.Character.BodyAnimationMonsterWithoutInteract(gestureName, false));
    }

    public Node Node_BodyAnimationPrincess(Val<string> gestureName, Val<bool> start)
    {
        return new LeafInvoke(
            () => this.Character.BodyAnimationPrincess(gestureName, start),
            () => this.Character.BodyAnimationPrincess(gestureName, false));
    }


    #endregion

    #endregion

    #region Helper Subtrees

    /// <summary>
    /// Plays a gesture of a determined type for a given duration
    /// </summary>
    public Node ST_PlayGesture(
        Val<string> gestureName,
        Val<AnimationLayer> layer,
        Val<long> duration)
    {
        switch (layer.Value)
        {
            case AnimationLayer.Hand:
                return this.ST_PlayHandGesture(gestureName, duration);
            case AnimationLayer.Body:
                return this.ST_PlayBodyGesture(gestureName, duration);
            case AnimationLayer.Face:
                return this.ST_PlayFaceGesture(gestureName, duration);
        }
        return null;
    }

    /// <summary>
    /// Plays a hand gesture for a duration in miliseconds
    /// </summary>
    public Node ST_PlayHandGesture(
        Val<string> gestureName, Val<long> duration)
    {
        return new DecoratorCatch(
            () => this.Character.HandAnimation(gestureName, false),
            new Sequence(
                Node_HandAnimation(gestureName, true),
                new LeafWait(duration),
                Node_HandAnimation(gestureName, false)));
    }

    /// <summary>
    /// Plays a body gesture for a duration in miliseconds
    /// </summary>
    public Node ST_PlayBodyGesture(
        Val<string> gestureName, Val<long> duration)
    {
        return new DecoratorCatch(
            () => this.Character.BodyAnimation(gestureName, false),
            new Sequence(
            this.Node_BodyAnimation(gestureName, true),
            new LeafWait(duration),
            this.Node_BodyAnimation(gestureName, false)));
    }

    /// <summary>
    /// Plays a face gesture for a duration in miliseconds
    /// </summary>
    public Node ST_PlayFaceGesture(
        Val<string> gestureName, Val<long> duration)
    {
        return new DecoratorCatch(
            () => this.Character.FaceAnimation(gestureName, false),
            new Sequence(
                Node_FaceAnimation(gestureName, true),
                new LeafWait(duration),
                Node_FaceAnimation(gestureName, false)));
    }

    /// <summary>
    /// Turns to face a target position
    /// </summary>
    public Node ST_TurnToFace(Val<Vector3> target)
    {
        Func<RunStatus> turn =
            () => this.Character.NavTurn(target);

        Func<RunStatus> stopTurning =
            () => this.Character.NavOrientBehavior(
                OrientationBehavior.LookForward);

        return
            new Sequence(
                new LeafInvoke(turn, stopTurning));
    }
    #endregion
}
