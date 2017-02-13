using UnityEngine;
using TreeSharpPlus;
using System;
using System.Collections;
using System.Collections.Generic;

using RootMotion.FinalIK;

public class CharacterMecanim : MonoBehaviour
{
    public const float MAX_REACHING_DISTANCE = 1.1f;
    public const float MAX_REACHING_HEIGHT = 2.0f;
    public const float MAX_REACHING_ANGLE = 100;

    /** DAVE NEW - OBJECT INTERACTION**/
    
    /** START **/

    public GameObject SW_ground;
    public GameObject SW_hand;
    
    /** END **/



    private GameObject HERO;
    private GameObject thisCharacter;
    private GameObject lastHitCharacter;
    private List<GameObject> hitCharacters = new List<GameObject>();
    public bool NPC_interact = false;
    public bool NPC_interact2 = false;
    public bool boxman_interact = false;
    public bool boxman_dying_interact = false;
    public bool monster_interact = false;
    public bool princess_interact = false;
    public bool button_interact = false;
    public bool Sword_interact = false;
    public bool has_Sword = false;
    public bool hero_orient = false;
    public Canvas speechBubble = null;
    public int win_count = 0;
    private Dictionary<FullBodyBipedEffector, bool> triggers;
    private Dictionary<FullBodyBipedEffector, bool> finish;

    [HideInInspector]
    public BodyMecanim Body = null;

    void Awake() { this.Initialize(); }

    /** DAVE NEW FUNCTIONS START **/

    void ShowSword()
    {
        print("2");
        SW_hand.SetActive(true);
        SW_ground.SetActive(false);
    }


    /** DAVE NEW FUNCTIONS END **/


    void Start()
    {
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");
        thisCharacter = this.gameObject;
        foreach (GameObject hero in heros)
        {
            HERO = hero;
        }

        //speechBubble.enabled = false;
    }

    private System.Diagnostics.Stopwatch speechTimer = new System.Diagnostics.Stopwatch();
    void Update()
    {
        if (speechBubble != null)
        {
            if (NPC_interact == true && speechTimer.ElapsedMilliseconds < 10000)
            {
                speechTimer.Start();
                speechBubble.enabled = true;
            }
            else
            {
                speechTimer.Stop();
                speechTimer.Reset();
                speechBubble.enabled = false;
                NPC_interact = false;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {

            // Find where the ray hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Detect who the ray hit and allow interactions
            if (Physics.Raycast(ray, out hit) == true)
            {
                lastHitCharacter = hit.transform.gameObject;
                Vector3 characterPosition = hit.transform.gameObject.transform.position;
                Vector3 heroPosition = HERO.transform.position;
                if (hit.transform.gameObject.CompareTag("NPC") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 3 && hit.transform.gameObject == thisCharacter)
                {
                    print("NPC hit");
                    NPC_interact = true;
                    Debug.Log(thisCharacter.name + " Clicked");

                }
                if (hit.transform.gameObject.CompareTag("NPC") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 3 && HERO == thisCharacter)
                {
                    NPC_interact2 = true;

                }
                if (hit.transform.gameObject.CompareTag("Monster") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 2 && (hit.transform.gameObject == thisCharacter || HERO == thisCharacter))
                {
                    print("monster hit");
                    monster_interact = true;
                    bool seen = false;
                    if (hitCharacters.Count != 0) {
                        for (int i = 0; i < hitCharacters.Count; i++)
                        { 
                            if (hitCharacters[i] == lastHitCharacter)
                            {
                                seen = true;
                            }
                        }
                    }
                    if (seen == false)
                    {
                        hitCharacters.Add(lastHitCharacter);
                        win_count = win_count + 1;
                    }
                }
                if (hit.transform.gameObject.CompareTag("Boxman") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 2 && (hit.transform.gameObject == thisCharacter || HERO == thisCharacter))
                {
                    print("boxman hit");
                    boxman_interact = true;
                    bool seen = false;
                    if (hitCharacters.Count != 0)
                    {
                        for (int i = 0; i < hitCharacters.Count; i++)
                        {
                            if (hitCharacters[i] == lastHitCharacter)
                            {
                                seen = true;
                            }
                        }
                    }
                    if (seen == false)
                    {
                        hitCharacters.Add(lastHitCharacter);
                        win_count = win_count + 1;
                    }
                }
                if (hit.transform.gameObject.CompareTag("Princess") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 2 && (hit.transform.gameObject == thisCharacter || HERO == thisCharacter))
                {
                    princess_interact = true;
                    bool seen = false;
                    if (hitCharacters.Count != 0)
                    {
                        for (int i = 0; i < hitCharacters.Count; i++)
                        {
                            if (hitCharacters[i] == lastHitCharacter)
                            {
                                seen = true;
                            }
                        }
                    }
                    if (seen == false)
                    {
                        hitCharacters.Add(lastHitCharacter);
                        win_count = win_count + 1;
                    }

                }
                if (hit.transform.gameObject.CompareTag("Button") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 3 && HERO == thisCharacter)
                {
                    button_interact = true;
                    Debug.Log("Button Clicked");

                }
                if (hit.transform.gameObject.CompareTag("Sword") && Mathf.Abs(Vector3.Distance(characterPosition, heroPosition)) < 3 && HERO == thisCharacter)
                {
                    Debug.Log("Sword Clicked");
                    Sword_interact = true;
                    has_Sword = true;
                }
            }
        }

    }
    /// <summary>
    /// Searches for and binds a reference to the Body interface
    /// </summary>
    public void Initialize()
    {
        this.Body = this.GetComponent<BodyMecanim>();
        this.Body.InteractionTrigger += this.OnInteractionTrigger;
        this.Body.InteractionStop += this.OnInteractionFinish;
    }

    private void OnInteractionTrigger(
        FullBodyBipedEffector effector, 
        InteractionObject obj)
    {
        if (this.triggers == null)
            this.triggers = new Dictionary<FullBodyBipedEffector, bool>();
        if (this.triggers.ContainsKey(effector))
            this.triggers[effector] = true;
    }

    private void OnInteractionFinish(
        FullBodyBipedEffector effector,
        InteractionObject obj)
    {
        if (this.finish == null)
            this.finish = new Dictionary<FullBodyBipedEffector, bool>();
        if (this.finish.ContainsKey(effector))
            this.finish[effector] = true;
    }

    #region Smart Object Specific Commands
    public virtual RunStatus WithinDistance(Vector3 target, float distance)
    {
        if ((transform.position - target).magnitude < distance)
            return RunStatus.Success;
        return RunStatus.Failure;
    }

    public virtual RunStatus Approach(Vector3 target, float distance)
    {
        Vector3 delta = target - transform.position;
        Vector3 offset = delta.normalized * distance;
        return this.NavGoTo(target - offset);
    }
    #endregion

    #region Navigation Commands
    /// <summary>
    /// Turns to face a desired target point
    /// </summary>
    public virtual RunStatus NavTurn(Val<Vector3> target)
    {
        this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
        this.Body.NavSetDesiredOrientation(target.Value);
        if (this.Body.NavIsFacingDesired() == true)
        {
            this.Body.NavSetOrientationBehavior(
                OrientationBehavior.LookForward);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public virtual RunStatus NavTurnButton()
    {
        if (button_interact)
        {
            if (thisCharacter == HERO)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(lastHitCharacter.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavTurnSword()
    {
        if (Sword_interact)
        {
            if (thisCharacter == HERO)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(lastHitCharacter.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavTurnBoxman()
    {
        if (boxman_interact)
        {
            if (thisCharacter == HERO)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(lastHitCharacter.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
            if (thisCharacter == lastHitCharacter)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(HERO.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavTurnMonster()
    {
        if (monster_interact)
        {
            if (thisCharacter == HERO)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(lastHitCharacter.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
            if (thisCharacter == lastHitCharacter)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(HERO.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavTurnPrincess()
    {
        if (princess_interact)
        {
            if(thisCharacter == HERO)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(lastHitCharacter.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
            if (thisCharacter == lastHitCharacter)
            {
                this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
                this.Body.NavSetDesiredOrientation(HERO.transform.position);
                if (this.Body.NavIsFacingDesired() == true)
                {
                    this.Body.NavSetOrientationBehavior(
                        OrientationBehavior.LookForward);
                    return RunStatus.Success;
                }
                return RunStatus.Running;
            }
        }
        return RunStatus.Success;
    }

    /// <summary>
    /// Turns to face a desired orientation
    /// </summary>
    public virtual RunStatus NavTurn(Val<Quaternion> target)
    {
        this.Body.NavSetOrientationBehavior(OrientationBehavior.None);
        this.Body.NavSetDesiredOrientation(target.Value);
        if (this.Body.NavIsFacingDesired() == true)
        {
            this.Body.NavFacingSnap();
            this.Body.NavSetOrientationBehavior(
                OrientationBehavior.LookForward);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    /// <summary>
    /// Sets a custom orientation behavior
    /// </summary>
    public virtual RunStatus NavOrientBehavior(
        Val<OrientationBehavior> behavior)
    {
        this.Body.NavSetOrientationBehavior(behavior.Value);
        return RunStatus.Success;
    }

    public virtual RunStatus NavOrientBehaviorButton(
        Val<OrientationBehavior> behavior)
    {
        if (button_interact)
        {
            this.Body.NavSetOrientationBehavior(behavior.Value);
            return RunStatus.Success;
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavOrientBehaviorBoxman(
        Val<OrientationBehavior> behavior)
    {
        if (boxman_interact)
        {
            this.Body.NavSetOrientationBehavior(behavior.Value);
            return RunStatus.Success;
        }
        return RunStatus.Success;
    }

    public virtual RunStatus NavOrientBehaviorMonster(
        Val<OrientationBehavior> behavior)
    {
        if (monster_interact)
        {
            this.Body.NavSetOrientationBehavior(behavior.Value);
            return RunStatus.Success;
        }
        return RunStatus.Success;
    }


    public virtual RunStatus NavOrientBehaviorPrincess(
        Val<OrientationBehavior> behavior)
    {
        if (princess_interact)
        {
            this.Body.NavSetOrientationBehavior(behavior.Value);
            return RunStatus.Success;
        }
        return RunStatus.Success;
    }

    /// <summary>
    /// Sets a new navigation target. Will fail immediately if the
    /// point is unreachable. Blocks until the agent arrives.
    /// </summary>
    public virtual RunStatus NavGoTo(Val<Vector3> target)
    {
        if (this.Body.NavCanReach(target.Value) == false)
        {
            Debug.LogWarning("NavGoTo failed -- can't reach target");
            return RunStatus.Failure;
        }
        // TODO: I previously had this if statement here to prevent spam:
        //     if (this.Interface.NavTarget() != target)
        // It's good for limiting the amount of SetDestination() calls we
        // make internally, but sometimes it causes the character1 to stand
        // still when we re-activate a tree after it's been terminated. Look
        // into a better way to make this smarter without false positives. - AS
        this.Body.NavGoTo(target.Value);
        if (this.Body.NavHasArrived() == true)
        {
            this.Body.NavStop();
            return RunStatus.Success;
        }
        return RunStatus.Running;
        // TODO: Timeout? - AS
    }

    public virtual RunStatus NavGoToPrincess()
    {
        if (princess_interact)
        {
            Vector3 princessRun = new Vector3(0f, 0f, 0f);
            if (this.Body.NavCanReach(princessRun) == false)
            {
                Debug.LogWarning("NavGoTo failed -- can't reach target");
                return RunStatus.Failure;
            }
            // TODO: I previously had this if statement here to prevent spam:
            //     if (this.Interface.NavTarget() != target)
            // It's good for limiting the amount of SetDestination() calls we
            // make internally, but sometimes it causes the character1 to stand
            // still when we re-activate a tree after it's been terminated. Look
            // into a better way to make this smarter without false positives. - AS
            this.Body.NavGoTo(princessRun);
            if (this.Body.NavHasArrived() == true)
            {
                this.Body.NavStop();
                return RunStatus.Success;
            }
            return RunStatus.Running;
        }
        return RunStatus.Success;
        // TODO: Timeout? - AS
    }

    /// <summary>
    /// Lerps the character towards a target. Use for precise adjustments.
    /// </summary>
    public virtual RunStatus NavNudgeTo(Val<Vector3> target)
    {
        bool? result = this.Body.NavDoneNudge();
        if (result == null)
        {
            this.Body.NavNudge(target.Value, 0.3f);
        }
        else if (result == true)
        {
            this.Body.NavNudgeStop();
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    private IEnumerator<RunStatus> snapper;

    private RunStatus TickSnap(
        Vector3 position,
        Vector3 target,
        float time = 1.0f)
    {
        if (this.snapper == null)
            this.snapper = 
                SnapToTarget(position, target, time).GetEnumerator();
        if (this.snapper.MoveNext() == false)
        {
            this.snapper = null;
            return RunStatus.Success;
        }
        return snapper.Current;
    }

    private IEnumerable<RunStatus> SnapToTarget(
        Vector3 position,
        Vector3 target,
        float time)
    {
        Interpolator<Vector3> interp =
            new Interpolator<Vector3>(
                position,
                target,
                Vector3.Lerp);
        interp.ForceMin();
        interp.ToMax(time);

        while (interp.State != InterpolationState.Max)
        {
            transform.position = interp.Value;
            yield return RunStatus.Running;
        }
        yield return RunStatus.Success;
        yield break;
    }

	/// <summary>
	/// Stops the Navigation system. Blocks until the agent is stopped.
	/// </summary>
	public virtual RunStatus NavStop()
    {
        this.Body.NavStop();
        if (this.Body.NavIsStopped() == true)
            return RunStatus.Success;
        return RunStatus.Running;
        // TODO: Timeout? - AS
    }

    public virtual RunStatus NavStopPrincess()
    {
        if (princess_interact)
        {
            this.Body.NavStop();
            if (this.Body.NavIsStopped() == true)
                princess_interact = false;
            return RunStatus.Success;
            return RunStatus.Running;
        }
        return RunStatus.Success;
        // TODO: Timeout? - AS
    }
    #endregion

    #region Interaction Commands
    public virtual RunStatus WaitForTrigger(
        Val<FullBodyBipedEffector> effector)
    {
        if (this.triggers == null)
            this.triggers = new Dictionary<FullBodyBipedEffector, bool>();
        if (this.triggers.ContainsKey(effector.Value) == false)
            this.triggers.Add(effector.Value, false);
        if (this.triggers[effector.Value] == true)
        {
            this.triggers.Remove(effector.Value);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public virtual RunStatus WaitForFinish(
        Val<FullBodyBipedEffector> effector)
    {
        if (this.finish == null)
            this.finish = new Dictionary<FullBodyBipedEffector, bool>();
        if (this.finish.ContainsKey(effector.Value) == false)
            this.finish.Add(effector.Value, false);
        if (this.finish[effector.Value] == true)
        {
            this.finish.Remove(effector.Value);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public virtual RunStatus StartInteraction(
        Val<FullBodyBipedEffector> effector, 
        Val<InteractionObject> obj)
    {
        this.Body.StartInteraction(effector, obj);
        return RunStatus.Success;
    }

    public virtual RunStatus ResumeInteraction(
        Val<FullBodyBipedEffector> effector)
    {
        this.Body.ResumeInteraction(effector);
        return RunStatus.Success;
    }

    public virtual RunStatus StopInteraction(Val<FullBodyBipedEffector> effector)
    {
        this.Body.StopInteraction(effector);
        return RunStatus.Success;
    }	
    #endregion

    #region HeadLook Commands
    public virtual RunStatus HeadLookAt(Val<Vector3> target)
    {
        this.Body.HeadLookAt(target);
        return RunStatus.Success;
    }

    public virtual RunStatus HeadLookStop()
    {
        this.Body.HeadLookStop();
		return RunStatus.Success;
	}
    #endregion

    #region Animation Commands
    public virtual RunStatus FaceAnimation(
        Val<string> gestureName, Val<bool> isActive)
    {
        this.Body.FaceAnimation(gestureName.Value, isActive.Value);
		return RunStatus.Success;
	}
	
	public virtual RunStatus HandAnimation(
        Val<string> gestureName, Val<bool> isActive)
    {
        this.Body.HandAnimation(gestureName.Value, isActive.Value);
		return RunStatus.Success;
	}

    public virtual RunStatus HandAnimationNPC(
        Val<string> gestureName, Val<bool> isActive)
    {
        if (NPC_interact == false)
        {
            this.Body.HandAnimation(gestureName.Value, isActive.Value);
            return RunStatus.Success;
        }
        return RunStatus.Success;
    }

    public virtual RunStatus HandAnimationNPC2(
        Val<string> gestureName, Val<bool> isActive)
    {
        if (NPC_interact2)
        {
            this.Body.HandAnimation(gestureName.Value, isActive.Value);
            NPC_interact2 = false;
            return RunStatus.Success;
        }
        this.Body.HandAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus HandAnimationButton(
        Val<string> gestureName, Val<bool> isActive)
    {
        if (button_interact)
        {
            this.Body.HandAnimation(gestureName.Value, isActive.Value);
            button_interact = false;
            print("button ");
            print(button_interact);
            return RunStatus.Success;
        }
        this.Body.HandAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }


    public virtual RunStatus HandAnimationPrincess(
        Val<string> gestureName, Val<bool> isActive)
    {
        if (princess_interact)
        {
            this.Body.HandAnimation(gestureName.Value, isActive.Value);
            princess_interact = false;
            return RunStatus.Success;
        }
        this.Body.HandAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimation(Val<string> gestureName, Val<bool> isActive)
	{
		this.Body.BodyAnimation(gestureName.Value, isActive.Value);
		return RunStatus.Success;
	}

    public virtual RunStatus BodyAnimationKillBoxman(
    Val<string> gestureName, Val<bool> isActive)
    {
        if (boxman_interact)
        {
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            boxman_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationKillMonster(
        Val<string> gestureName, Val<bool> isActive)
    {
        if (monster_interact)
        {
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            monster_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationKillMonsterWithoutInteract(
    Val<string> gestureName, Val<bool> isActive)
    {
        this.Body.BodyAnimation(gestureName.Value, isActive.Value);
        monster_interact = false;
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationButton(Val<string> gestureName, Val<bool> isActive)
    {
        if (button_interact)
        {
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            button_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationKiss(Val<string> gestureName, Val<bool> isActive)
    {
        if (princess_interact)
        {
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            princess_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationKissWithoutInteract(Val<string> gestureName, Val<bool> isActive)
    {
        this.Body.BodyAnimation(gestureName.Value, isActive.Value);
        princess_interact = false;
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationSword(Val<string> gestureName, Val<bool> isActive)
    {
        if (Sword_interact)
        {
            Debug.Log("Sword motion called!");
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            Sword_interact = false;
            has_Sword = true;
            Invoke("ShowSword", 1.0F);
            return RunStatus.Success;
        }
        //this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationSwordWithoutInteract(Val<string> gestureName, Val<bool> isActive)
    {
            Debug.Log("Sword motion called!");
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            Sword_interact = false;
            Invoke("ShowSword", 1.0F);
            return RunStatus.Success;
        //this.Body.BodyAnimation(gestureName.Value, false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }

    public virtual RunStatus BodyAnimationBoxing(Val<string> gestureName, Val<bool> isActive)
    {
        if (boxman_interact)
        {
            if (thisCharacter != HERO)
            {
                Wait();
            }
            print("Boxing action invoked!");
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            boxman_interact = false;
            //boxman_dying_interact = true;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationBoxingDying(Val<string> gestureName, Val<bool> isActive)
    {
        if (boxman_dying_interact)
        {
            if (thisCharacter != HERO)
            {
                Wait();
            }
            Debug.Log("boxman dying invoked!");
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            boxman_dying_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationMonster(Val<string> gestureName, Val<bool> isActive)
    {
        if (monster_interact)
        {
            if(thisCharacter != HERO)
            {
                Wait();
            }
            Debug.Log("Monster action invoked!");
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            monster_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationMonsterWithoutInteract(Val<string> gestureName, Val<bool> isActive)
    {
        if (thisCharacter != HERO)
         {
             Wait();
         }
        Debug.Log("Monster action invoked!");
        this.Body.BodyAnimation(gestureName.Value, isActive.Value);
        monster_interact = false;
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public virtual RunStatus BodyAnimationPrincess(Val<string> gestureName, Val<bool> isActive)
    {
        if (princess_interact)
        {
            this.Body.BodyAnimation(gestureName.Value, isActive.Value);
            princess_interact = false;
            return RunStatus.Success;
        }
        this.Body.BodyAnimation(gestureName.Value, false);
        return RunStatus.Success;
    }

    public RunStatus ResetAnimation()
    {
        this.Body.ResetAnimation();
        return RunStatus.Success;
    }
    #endregion

    #region Sitting Commands
    /// <summary>
    /// Sits the character down
    /// </summary>
    public virtual RunStatus SitDown()
    {
        if (this.Body.IsSitting() == true)
            return RunStatus.Success;
        this.Body.SitDown();
        return RunStatus.Running;
    }

    /// <summary>
    /// Stands the character up
    /// </summary>
    public virtual RunStatus StandUp()
    {
        if (this.Body.IsStanding() == true)
            return RunStatus.Success;
        this.Body.StandUp();
        return RunStatus.Running;
    }
    #endregion
}
