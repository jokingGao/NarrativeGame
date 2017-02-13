
using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
    public GameObject Player;
    public GameObject NPC;
    public GameObject Princess;
    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Monster3;

    private Val<GameObject> CopClicked;

    
    public Transform Wander1Start;
    public Transform Wander1End;
    public Transform Wander2Start;
    public Transform Wander2End;
    public Transform Wander3Start;
    public Transform Wander3End;
    public Transform PlayerStartPoint;
    public Transform SwordPos;
    public Transform PrincePos;

    public monsterCloseDetector monsterDetector1;
    public monsterCloseDetector monsterDetector2;
    public monsterCloseDetector monsterDetector3;

    public npcDetector npcDetector1;

    public PrincessDetector princessDetector1;
    //public PlayerControls Controls;
    public CameraController Camera;
    public textBoxController TextBox;
    //public TreasureBoxMovement Treasure;
    public GameObject Poster;
    //public PoliceCollisionAggregator PCA;

    private BehaviorAgent behaviorAgent;

    public bool has_sword;
    private int counter = 0;
    public bool KillMonster;

    Val<Vector3> PlayerPos;
    // Use this for initialization
    void Start()
    {
        CopClicked = Val.V(() => new GameObject());
        behaviorAgent = new BehaviorAgent(this.BehaviorTree());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool returnTrue()
    {
        return true;
    }

    protected Node PrisonerFollow(GameObject Player, GameObject Prisoner)
    {
        Val<Vector3> PlayerPos = Val.V(() => Player.transform.position);
        return new Sequence(
            Prisoner.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(PlayerPos, 2.0f));
    }

    protected Node InitialConversation(GameObject Player, GameObject NPC)
    {
        PlayerPos = Val.V(() => Player.transform.position);
        Val<Vector3> NPCPos = Val.V(() => NPC.transform.position);
        Val<Vector3> StartPos = Val.V(() => PlayerStartPoint.position);
        Val<Vector3> Sword_Pos = Val.V(() => SwordPos.position);
        Val<Vector3> Prince_Pos = Val.V(() => PrincePos.position);
        //Val<Vector3> StartOrient = Val.V(() => PlayerStartOrientation.position);

        return new Sequence(
                                    //Player.GetComponent<BehaviorMecanim>().Node_GoTo(StartPos),
            new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(npcDetector1.npcClick)))))),

            new Sequence(NPC.GetComponent<BehaviorMecanim>().Node_OrientTowards(PlayerPos), new LeafWait(1000)),
            NPC.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
            new LeafInvoke((Func<RunStatus>)TextBox.startCounter),
            new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("WAVE", (long)2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(Player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new SequenceParallel(NPC.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 2000),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished))))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished)
            //new Sequence(Princess.GetComponent<BehaviorMecanim>().Node_OrientTowards(Prince_Pos), new LeafWait(1000))
            //new SequenceParallel(Princess.GetComponent<BehaviorMecanim>().Node_HandAnimation("WAVE", true),
            //new LeafInvoke((Func<RunStatus>)TextBox.PrincessDialog)),
            //new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            //new LeafWait(2000),
            //Princess.GetComponent<BehaviorMecanim>().Node_BodyAnimationKissWithoutInteract("KISSING", true)
            ));
    }

    protected RunStatus LostGame()
    {
        Time.timeScale = 0.0f;
        behaviorAgent.StopBehavior();
        return RunStatus.Success;
    }

    protected RunStatus MonsterKilled()
    {
        counter++;
        print(counter);
        if (counter == 3)
        {
            KillMonster = true;
        }
        return RunStatus.Success;
    }

    protected bool NotHasSword()
    {
        has_sword = Player.GetComponent<CharacterMecanim>().has_Sword;
        if (has_sword)
        {
            return false;
        }
        return true;
    }
    protected Node MonsterHero(GameObject Player, GameObject Monster, Transform WanderBegin, Transform WanderEnd, monsterCloseDetector monsterDetector)
    {
        Val<Vector3> begin = Val.V(() => WanderBegin.position);
        Val<Vector3> end = Val.V(() => WanderEnd.position);
        return new Sequence(
                    new Race(
                        new DecoratorLoop(
                            new Sequence(Monster.GetComponent<BehaviorMecanim>().Node_GoTo(begin), new LeafWait(1000),
                                 Monster.GetComponent<BehaviorMecanim>().Node_GoTo(end), new LeafWait(1000))),
                        new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(monsterDetector.monsterCloseToHero)))))),
                            new LeafWait(500))),
                    new DecoratorInvert(new Sequence(new LeafAssert(NotHasSword),
                                 Player.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true),
                                 new LeafInvoke((Func<RunStatus>)TextBox.gotKilled),
                                 new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished)))))),
                                 new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
                                 new LeafInvoke((Func<RunStatus>)LostGame))),
                    //new LeafInvoke((Func<RunStatus>)Controls.DisableControls),
                    new Sequence(
                          new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(monsterDetector.monsterCloseToHero)))))),
                          new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(monsterDetector.monsterClick)))))),
                          Player.GetComponent<BehaviorMecanim>().Node_BodyAnimationKillMonster("KILL_MONSTER", true),
                          Monster.GetComponent<BehaviorMecanim>().Node_BodyAnimationMonster("DYING", true),
                          new LeafInvoke((Func<RunStatus>)MonsterKilled)
                        ))));
    }

    protected Node PrincessHero()
    {
        return new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(princessDetector1.princessClick)))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new LeafInvoke((Func<RunStatus>)TextBox.PrincessDialog),
            new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(new LeafAssert(TextBox.isDialogFinished)))))),
            new LeafInvoke((Func<RunStatus>)TextBox.resetDialogFinished),
            new LeafInvoke((Func<RunStatus>)LostGame));
    }

    protected Node InitialConversationTree()
    {
        return new DecoratorLoop(
            new DecoratorForceStatus(RunStatus.Success, InitialConversation(Player, NPC)));
    }

    protected Node BehaviorTree()
    {
        return new DecoratorLoop(
            new SequenceParallel(
                InitialConversationTree(),
                PrincessHero(),
                MonsterHero(Player, Monster1, Wander1Start, Wander1End, monsterDetector1),
                MonsterHero(Player, Monster2, Wander2Start, Wander2End, monsterDetector2),
                MonsterHero(Player, Monster3, Wander3Start, Wander3End, monsterDetector3)
                ));
    }

}

