using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;


public enum SelectGirl
{ 
    G1_Alice = 0,
    G2a222,
    G3_Beryl,
    G4a444,
    G5a555,
    Null
}


public enum Direction
{ 
    Up = 0,
    Down,
    Left,
    Right
}

public enum FoodState
{ 
    FS0_Full = 0,
    FS1_Half,
    FS2_Null,
}



public class PuzzleManager : A_Singleton<PuzzleManager>
{
    public string MapName;


    public enum GameMode
    {
        MoveMap,
        Battle,
        Null,
        GameOver,
    }

    public enum State
    {
        Ready = 0,
        Switching,          //스위치를 시도
        SwitchRetrun,       //스위치후 다시 복귀
        MatchBurst,         //매치를 버스팅 한다
        DestroyCube,
        FillBlank,
        CheckMatch,
        ChangeMode,
        BattleResult,    // 배틀 끝나고 결과창
        SpecialCubeEvent,
        BattleEvent,
        LoadingMap,
        Tutorial,

    }
    public GameMode gameMode = GameMode.MoveMap;
    public State state;
    public SelectGirl selectGirl = SelectGirl.G1_Alice;

    public MapManager theMoveMap;
    public MapManager theBattleMap;
    public MapManager GetMap()
    {

        if (gameMode == GameMode.MoveMap)
        {
            return theMoveMap;
        }
        else
        {
            return theBattleMap;
        }
    }



    public Sprite[] CubeUiSprites;
    public Sprite[] CubeSprites;
    public Sprite[] SpecialSprites;
    public Sprite[] PlayerSkillSprites;
    public Sprite[] PlayerSkillBGSprites;
    public Sprite[] FoodSprites;


    [Space]
    [Header("UI 오브젝트")]
    //UI 오브젝트
    public CubeUI[] PlayerCubeUI;
    public PlayerUI[] playerUIs; // 0은 왼쪽 캐릭터, 1은 오른쪽 캐릭터

    public GameObject MoveUI;
    public GameObject BattleUI;
    public GameObject IllustSlot;
    public Image CubeBar;
    public Button HintButton;
    public TextMeshPro MoveCountText;
    public Image FoodImage;
    //메치가 되면 true;



    [Space]
    [Header("데이터 베이스")]

    //DB
    public bool SlotDown = false;

    public int MoveCount;   //움직임 가능한 횟수
    public int CurrentPoint; // 현재 점수
    // 일러스트에 넣을 소녀들의 번호
    public int FirstHeroNum;
    public int secondHeroNum;

    public PlayerCube Player;


    //쓰래기통

    //이밴트 발동중인 시간을 체크
    public float EventTime = 0f;
    public bool CheckEvent;





    public bool AutoEvent = false; // cubeEvent 를 강제로 실행한다
    [HideInInspector] public float CubeMoveSpeed = 0.2f;
    float AutoEventTime = 0;
    public int SelectNum = 0;
    public int OtherNum = 0;
    int HintNum;
    int[] EnemyCubeCount = new int[6];
    WaitForSeconds Wait = new WaitForSeconds(0.1f);
    float FillSpeed = 0.1f;
    FoodState foodState;


    Vector3 MovePos = new Vector3(0, 800, 0); //전투가 끝난후 UI위치
    Vector3 BattlePos = new Vector3(0, -750, 0); // 전투 시작시 UI위치


    //[HideInInspector] public List<string> PortalName;
    List<GameObject> StartEffect = new List<GameObject>();


    private ObjectManager theObject;
    private FindMatches theMatch;
    private FadeManager theFade;
    private CameraManager theCamera;
    private BattleManager theBattle;
    private GirlManager theGirl;
    private SoundManager theSound;
    private GameManager theGM;
    private PuzzleMaker theMaker;
    private CameraButtonManager CameraButton;
    private TitleManager theTitle;
    private MessageManager theMessage;
    private TutorialManager theTuto;
    private CameraButtonManager theCameraButton;
    private GameEndManager theEnd;
    private void Start()
    {
        if (HintButton != null)
        {
            HintButton.onClick.AddListener(() =>
            {
                BT_ShowHint();
            });
        }
        theEnd = FindObjectOfType<GameEndManager>();
        theCameraButton = FindObjectOfType<CameraButtonManager>();
        theTuto = FindObjectOfType<TutorialManager>();
        theMessage = FindObjectOfType<MessageManager>();
        theTitle = FindObjectOfType<TitleManager>();
        CameraButton = FindObjectOfType<CameraButtonManager>();
        theMaker = FindObjectOfType<PuzzleMaker>();
        theGM = FindObjectOfType<GameManager>();
        theSound = FindObjectOfType<SoundManager>();
        theBattle = FindObjectOfType<BattleManager>();
        theFade = FindObjectOfType<FadeManager>();
        Player = FindObjectOfType<PlayerCube>();
        theMatch = FindMatches.Instance;
        theObject = FindObjectOfType<ObjectManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theGirl = FindObjectOfType<GirlManager>();
        BT_ChangeDirection(1);
    }




    private void Update()
    {
        if (CheckEvent == true)
        {
            if (EventTime > 0)
                EventTime -= Time.deltaTime;
        }



        //기본 준비상태
        if (state == State.Ready)
        {

        }
        // 스위치 체크
        else if (state == State.Switching)
        {
            CheckSwitching();
        }
        // 매치가 없어서 돌아온다
        else if (state == State.SwitchRetrun)
        {
            CheckSwitchReturn();
        }
        else if (state == State.MatchBurst)
        {

        }




        return;

        //if (theGM.state == GMState.GM00_Title)
        //{
        //    if (theFade.FadeOutEnd == true)
        //    {
        //        theFade.FadeOutEnd = false;
        //        theTitle.TitleAnim.gameObject.SetActive(false);
        //        theGM.state = GMState.GM00_Tutorial;
        //        state = State.LoadingMap;
        //        MoveCount = 100;
        //        SetMoveCount(0);
        //        //theGM.LoadMap();


        //    }
        //}
        //else if (theGM.state == GMState.GM02_InGame)
        //{
        //    PuzzleUpdate();
        //}
        //else if (theGM.state == GMState.GM00_Tutorial)
        //{
        //    PuzzleTutorialUpdate();
        //}


        ////큐브 없이 큐브 이밴트를 사용해야 할 때 사용
        //if (AutoEvent == true)
        //{
        //    if (AutoEventTime < 0.6f)
        //        AutoEventTime += Time.deltaTime;
        //    else
        //    {
        //        CubeEvent = true;
        //        AutoEvent = false;
        //        AutoEventTime = 0;
        //    }
        //}

    }


    //public void PuzzleUpdate()
    //{
    //    if (gameMode == GameMode.MoveMap)
    //    {
    //        if (state == State.ChangeMatch) //  큐브를 교환하는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;

    //                //매치 조건이 맞는지 확인한다

    //                int SlotNum = CheckPlayerSlot(theMoveMap);
    //                //if (theMoveMap.Slots[SlotNum].panel.panelType == PanelType.Portal)
    //                //{
    //                //    SetMoveCount(-1);
    //                //    if (playerUIs[0].state == PlayerUIState.Die && playerUIs[1].state == PlayerUIState.Die)
    //                //    {
    //                //        GameOverMove();
    //                //        return;
    //                //    }
    //                //    CheckPortal(SlotNum);
    //                //    return;
    //                //}



    //                if (theMatch.FindAllMatches(theMoveMap))
    //                {
    //                    SetMoveCount(-1);
    //                    FindMatches.Instance.FindSpecialCube(theMoveMap);
    //                    DestroyCube(theMoveMap);

    //                    return;
    //                }
    //                else
    //                {
    //                    ChangeCube(theMoveMap, SelectNum, OtherNum, CubeMoveSpeed, true);
    //                    state = State.ChangeMatchRetrun;
    //                }


    //            }
    //        }
    //        else if (state == State.ChangeMatchRetrun)// 매치조건이 없어서 다시 원위치
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                Player.ChangeAnim("Idle", true);
    //                state = State.Ready;
    //            }
    //        }
    //        else if (state == State.FillBlank)// 빈칸을 채우는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                int PlayerSlotNum = CheckPlayerSlot(theMoveMap);
    //                //if (theMoveMap.Slots[PlayerSlotNum].panel.panelType == PanelType.Portal)
    //                //{
    //                //    if (playerUIs[0].state == PlayerUIState.Die && playerUIs[1].state == PlayerUIState.Die)
    //                //    {
    //                //        GameOverMove();
    //                //        return;
    //                //    }
    //                //    CheckPortal(PlayerSlotNum);
    //                //    return;
    //                //}



    //                BT_FillBlank(theMoveMap);
    //            }
    //        }
    //        else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
    //        {

    //            if (theMatch.FindAllMatches(theMoveMap))
    //            {
    //                DestroyCube(theMoveMap);
    //                theMatch.FindSpecialCube(theMoveMap);
    //                return;
    //            }
    //            else
    //            {
    //                Player.ChangeAnim("Idle", true);

    //                if (CheckEndMatchEvent() == true)
    //                {
    //                    return;
    //                }

    //                //if (CheckGoal(theMoveMap) == true)
    //                //{
    //                //    state = State.Ready;
    //                //    return;
    //                //}
    //                if (DeadlockCheck(theMoveMap))
    //                {
    //                    state = State.Ready;
    //                }
    //                else
    //                {
    //                    SetSlot(theMoveMap, true);
    //                    state = State.Ready;
    //                }
    //            }
    //        }
    //        else if (state == State.DestroyCube)//매치된 큐브 제거
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theMoveMap);
    //            }
    //        }
    //        else if (state == State.SpecialCubeEvent) // 특수블럭 깨지는 상황
    //        {
    //            if (CubeEvent == true)
    //            {
    //                if (theMatch.CheckBoom == true)
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theMoveMap);
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        theMatch.CheckBoom = false;
    //                        BT_FillBlank(theMoveMap);
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else if (state == State.LoadingMap) // 맵을 로딩한 후 페이드 인이 끝났을 때
    //        {
    //            if (theFade.FadeInEnd == true)
    //            {
    //                theFade.FadeInEnd = false;
    //                theFade.ShowMapNameEvent(theMaker.m_MapName);
    //                CheckMoveBGM();
    //                CheckMoveMessage();
    //                state = State.Ready;
    //            }
    //        }
    //        else if (state == State.ChangeMode)
    //        {
    //            if (theFade.FadeOutEnd == true)
    //            {
    //                theFade.FadeOutEnd = false;
    //                SetSlot(theBattleMap, true);
    //                theBattle.SetBattle(theBattle.SelectEnemyNum);
    //                ChangeGameMode();
    //            }
    //        }
    //    }
    //    else if (gameMode == GameMode.Battle)
    //    {

    //        if (state == State.Ready)
    //        {
    //            theBattle.CheckComboCoolDonw();
    //        }
    //        else if (state == State.ChangeMatch) //  큐브를 교환하는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;

    //                //매치 조건이 맞는지 확인한다

    //                if (theMatch.FindAllMatches(theBattleMap))
    //                {

    //                    SetMoveCount(-1);
    //                    DestroyCube(theBattleMap);
    //                    theMatch.FindSpecialCube(theBattleMap);
    //                    return;
    //                }
    //                else
    //                {
    //                    ChangeCube(theBattleMap, SelectNum, OtherNum, CubeMoveSpeed, true);
    //                    state = State.ChangeMatchRetrun;
    //                }

    //            }
    //        }
    //        else if (state == State.ChangeMatchRetrun)// 매치조건이 없어서 다시 원위치
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                theBattle.ResetCombo();
    //                state = State.Ready;
    //            }
    //        }
    //        else if (state == State.FillBlank)// 빈칸을 채우는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theBattleMap);

    //            }


    //        }
    //        else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
    //        {
    //            if (theBattle.PlayerAttackEffectList.Count > 0 && theBattle.CurrentEnemyCount == 0)
    //                return;


    //            if (theMatch.FindAllMatches(theBattleMap))
    //            {

    //                DestroyCube(theBattleMap);
    //                theMatch.FindSpecialCube(theBattleMap);
    //                return;
    //            }
    //            else
    //            {
    //                if (DeadlockCheck(theBattleMap))
    //                {
    //                    // 카운트가 0이되어 적이 공격함
    //                    if (theBattle.CurrentEnemyCount <= 0)
    //                    {
    //                        theBattle.battleState = BattleState.EnemyAttack;
    //                        state = State.BattleEvent;
    //                        return;
    //                    }


    //                    state = State.Ready;
    //                }
    //                else
    //                {
    //                    SetSlot(theBattleMap, true);
    //                    state = State.Ready;
    //                }
    //            }
    //        }
    //        else if (state == State.DestroyCube)//매치된 큐브 제거
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theBattleMap);
    //            }
    //        }
    //        else if (state == State.SpecialCubeEvent) // 특수블럭 깨지는 상황
    //        {
    //            if (CubeEvent == true)
    //            {
    //                if (theMatch.CheckBoom == true)
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CheckBoom = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theBattleMap);
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theBattleMap);
    //                        return;
    //                    }
    //                }
    //            }
    //        }



    //    }
    //    else if (gameMode == GameMode.GameOver)
    //    {
    //        if (theFade.FadeOutEnd == true)
    //        {
    //            theFade.FadeOutEnd = false;
    //            GameOver(); 
    //        }
    //        if (theFade.FadeInEnd == true)
    //        {
    //            theFade.FadeInEnd = false;
    //            theGM.state = GMState.GM00_Title;
    //            state = State.Ready;
    //            theSound.PlayBGM("TitleBGM");
    //            theTitle.TitleAnim.Play("Start1");
    //        }

    //    }

    //}

    //튜토리얼에서만 사용할 업데이트
    //public void PuzzleTutorialUpdate()
    //{
    //    if (gameMode == GameMode.MoveMap)
    //    {
    //        if (state == State.ChangeMatch) //  큐브를 교환하는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;

    //                //매치 조건이 맞는지 확인한다

    //                int SlotNum = CheckPlayerSlot(theMoveMap);
    //                //if (theMoveMap.Slots[SlotNum].panel.panelType == PanelType.Portal)
    //                //{
    //                //    SetMoveCount(-1);
    //                //    if (playerUIs[0].state == PlayerUIState.Die && playerUIs[1].state == PlayerUIState.Die)
    //                //    {
    //                //        GameOverMove();
    //                //        return;
    //                //    }
    //                //    CheckPortal(SlotNum);
    //                //    return;
    //                //}



    //                if (theMatch.FindAllMatches(theMoveMap))
    //                {

    //                    SetMoveCount(-1);
    //                    DestroyCube(theMoveMap);
    //                    theMatch.FindSpecialCube(theMoveMap);
    //                    return;
    //                }
    //                else
    //                {
    //                    ChangeCube(theMoveMap, SelectNum, OtherNum, CubeMoveSpeed, true);
    //                    state = State.ChangeMatchRetrun;
    //                }
    //            }
    //        }
    //        else if (state == State.ChangeMatchRetrun)// 매치조건이 없어서 다시 원위치
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                Player.ChangeAnim("Idle", true);
    //                state = State.Ready;
    //            }
    //        }
    //        else if (state == State.FillBlank)// 빈칸을 채우는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                int PlayerSlotNum = CheckPlayerSlot(theMoveMap);
    //                //if (theMoveMap.Slots[PlayerSlotNum].panel.panelType == PanelType.Portal)
    //                //{
    //                //    if (playerUIs[0].state == PlayerUIState.Die && playerUIs[1].state == PlayerUIState.Die)
    //                //    {
    //                //        GameOverMove();
    //                //        return;
    //                //    }
    //                //    CheckPortal(PlayerSlotNum);
    //                //    return;
    //                //}



    //                BT_FillBlank(theMoveMap);
    //            }
    //        }
    //        else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
    //        {

    //            if (theMatch.FindAllMatches(theMoveMap))
    //            {
    //                DestroyCube(theMoveMap);
    //                theMatch.FindSpecialCube(theMoveMap);
    //                return;
    //            }
    //            else
    //            {
    //                Player.ChangeAnim("Idle", true);

    //                if (CheckEndMatchEvent() == true)
    //                {
    //                    return;
    //                }

    //                //if (CheckGoal(theMoveMap) == true)
    //                //{
    //                //    state = State.Ready;
    //                //    return;
    //                //}
    //                if (DeadlockCheck(theMoveMap))
    //                {
    //                    state = State.Ready;
    //                }
    //                else
    //                {
    //                    SetSlot(theMoveMap, true);
    //                    state = State.Ready;
    //                }
    //                CheckTutorialMessage();
    //            }

    //        }
    //        else if (state == State.DestroyCube)//매치된 큐브 제거
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theMoveMap);
    //            }
    //        }
    //        else if (state == State.SpecialCubeEvent) // 특수블럭 깨지는 상황
    //        {
    //            if (CubeEvent == true)
    //            {
    //                if (theMatch.CheckBoom == true)
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theMoveMap);
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        theMatch.CheckBoom = false;
    //                        BT_FillBlank(theMoveMap);
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else if (state == State.LoadingMap) // 맵을 로딩한 후 페이드 인이 끝났을 때
    //        {
    //            if (theFade.FadeInEnd == true)
    //            {
    //                theFade.FadeInEnd = false;
    //                state = State.Ready;
    //                theFade.ShowMapNameEvent(theMaker.m_MapName);
    //                CheckMoveBGM();
    //                CheckTutorialMessage();

    //            }
    //        }
    //        else if (state == State.ChangeMode)
    //        {
    //            if (theFade.FadeOutEnd == true)
    //            {
    //                theFade.FadeOutEnd = false;
    //                SetSlot(theBattleMap, true);
    //                theBattle.SetBattle(theBattle.SelectEnemyNum);
    //                ChangeGameMode();
    //            }
    //        }
    //        else if (state == State.Tutorial)
    //        {
    //            CheckTutorialMessage();
    //        }
    //    }
    //    else if (gameMode == GameMode.Battle)
    //    {

    //        if (state == State.Ready)
    //        {
    //            theBattle.CheckComboCoolDonw();
    //        }
    //        else if (state == State.ChangeMatch) //  큐브를 교환하는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;

    //                //매치 조건이 맞는지 확인한다

    //                if (theMatch.FindAllMatches(theBattleMap))
    //                {

    //                    SetMoveCount(-1);
    //                    DestroyCube(theBattleMap);
    //                    theMatch.FindSpecialCube(theBattleMap);
    //                    return;
    //                }
    //                else
    //                {
    //                    ChangeCube(theBattleMap, SelectNum, OtherNum, CubeMoveSpeed, true);
    //                    state = State.ChangeMatchRetrun;
    //                }
    //            }
    //        }
    //        else if (state == State.ChangeMatchRetrun)// 매치조건이 없어서 다시 원위치
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                theBattle.ResetCombo();
    //                state = State.Ready;
    //            }
    //        }
    //        else if (state == State.FillBlank)// 빈칸을 채우는 상태
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theBattleMap);

    //            }


    //        }
    //        else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
    //        {
    //            if (theBattle.PlayerAttackEffectList.Count > 0 && theBattle.CurrentEnemyCount == 0)
    //                return;

    //            if (theMatch.FindAllMatches(theBattleMap))
    //            {

    //                DestroyCube(theBattleMap);
    //                theMatch.FindSpecialCube(theBattleMap);
    //                return;
    //            }
    //            else //매치가 안될경우
    //            {
    //                if (DeadlockCheck(theBattleMap))
    //                {
    //                    // 카운트가 0이되어 적이 공격함
    //                    if (theBattle.CurrentEnemyCount <= 0)
    //                    {
    //                        theBattle.battleState = BattleState.EnemyAttack;
    //                        state = State.BattleEvent;
    //                        return;
    //                    }


    //                    state = State.Ready;
    //                }
    //                else
    //                {
    //                    SetSlot(theBattleMap, true);
    //                    state = State.Ready;
    //                }
    //            }
    //        }
    //        else if (state == State.DestroyCube)//매치된 큐브 제거
    //        {
    //            if (CubeEvent == true)
    //            {
    //                CubeEvent = false;
    //                BT_FillBlank(theBattleMap);
    //            }
    //        }
    //        else if (state == State.SpecialCubeEvent) // 특수블럭 깨지는 상황
    //        {
    //            if (CubeEvent == true)
    //            {
    //                if (theMatch.CheckBoom == true)
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CheckBoom = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theBattleMap);
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (theMatch.CurrentCheckBoomTime > 0)
    //                    {
    //                        theMatch.CurrentCheckBoomTime -= Time.deltaTime;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        CubeEvent = false;
    //                        theMatch.CurrentCheckBoomTime = theMatch.MaxCheckBoomTime;
    //                        BT_FillBlank(theBattleMap);
    //                        return;
    //                    }
    //                }
    //            }
    //        }



    //    }
    //    else if (gameMode == GameMode.GameOver)
    //    {
    //        if (theFade.FadeOutEnd == true)
    //        {
    //            theFade.FadeOutEnd = false;
    //            GameOver();
    //        }
    //        if (theFade.FadeInEnd == true)
    //        {
    //            theFade.FadeInEnd = false;
    //            theGM.state = GMState.GM00_Title;
    //            state = State.Ready;
    //            theSound.PlayBGM("TitleBGM");
    //            theTitle.TitleAnim.Play("Start1");
    //        }

    //    }

    //}


    #region 이밴트 시간 체크

    public void EventUpdate(float _EventTime = 1f)
    {
        CheckEvent = true;
        if (EventTime < _EventTime)
        {
            EventTime = _EventTime;
        }
    }
    public bool EventEnd
    {
        get
        {
            if (EventTime <= 0 && CheckEvent == true)
            {
                EventTime = 0;
                CheckEvent = false;
                return true;
            }
            return false;
        }
        
    }

    #endregion




    //스텝

    public void CheckSwitching()
    {
        if (EventEnd)
        {
            //매치가 가능한가 체크
            if (theMatch.FindAllMatches(GetMap()))
            {
                state = State.MatchBurst;

                //매치가 된 블럭들을 모두 버스팅 해준다
                for (int i = 0; i < theMatch.currentMathces.Count; i++)
                {
                    theMatch.currentMathces[i].BurstEvent();
                }
                theMatch.currentMathces.Clear();


            }
            else
            {
                state = State.SwitchRetrun;
                EventUpdate(MatchBase.BlockSpeed);
                GetMap().Slots[SelectNum].SwitchBlock(GetMap().Slots[OtherNum]);

            }
        }

    }


    public void CheckSwitchReturn()
    {
        if (EventEnd)
        {
            state = State.Ready;
        }
    }











    //슬롯을 채운다. false일 경우 최초실행, true일 경우 현재 맵에서 리셋

    public void LoadMap(MapManager _Map, bool ReLoadMap) //맵을 다시 불러온다면 true, 처음이면 false
    {
        //CubeBar.sprite = theObject.CubeBarSpirte[(int)theMaker.mapMainType];

        ////PortalName.Clear();


        //if (ReLoadMap == true)
        //{

        //    for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.Slots.Length; i++)
        //    {
        //        if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
        //        {
        //            _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
        //            _Map.Slots[i].nodeColor = NodeColor.NC8_Null;
        //            if (_Map.Slots[i].cube != null)
        //            {
        //                _Map.Slots[i].cube.Resetting();
                        
        //            }
        //        }
        //    }
        //}

        //for (int Hor = 0; Hor < _Map.BottomRight; Hor += _Map.Horizontal)
        //{
        //    for (int i = 0; i <= _Map.TopRight; i++)
        //    {
        //        theObject.SpawnSlotPanel(_Map.transform, _Map.Slots[i + Hor].transform.position, _Map.Slots[i + Hor].SlotSheet.SlotSheet,MapType.M1_MoveMap, i + Hor);
        //        _Map.Slots[i + Hor].TestText.enabled = false;
        //        if (_Map.Slots[i + Hor].nodeType != PuzzleSlot.NodeType.Null)
        //        {
        //            if (_Map.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Enemy)
        //            {
  
        //                //_Map.Slots[i + Hor].GetComponent<Image>().color = new Color(1, 0, 0, 0.4f);
        //            }
        //            else if (_Map.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Portal)
        //            {
        //                //theObject.SpawnPortal(_Map.Slots[i + Hor].transform.position);
        //                //_Map.Slots[i + Hor].GetComponent<Image>().color = new Color(0, 0, 1, 0.4f);
        //            }

                    
        //        }
        //    }
        //}


        //NotMatchSetCube(_Map);
       
    }



    public void SetSlot(MapManager _Map, bool Reset = false)
    {

        //if (Reset == true)
        //{
        //    Debug.Log("더이상 매치를 할 수 없어서 리셋을 시작합니다");
        //}
        //else
        //{
        //    Debug.Log("처음 리셋을 시작합니다");
        //}
        //MapType mapType = _Map.mapType;


        //if (_Map.mapType == MapType.M2_BattleMap)
        //{
        //    if (_Map.FirstBattle == false)
        //    {
        //        _Map.FirstBattle = true;

        //        for (int i = 0; i < _Map.Slots.Length; i++)
        //        {
        //            if (i >0 && i < _Map.TopRight)
        //            {
        //                theBattleMap.Slots[i].SlotSheet.SlotSheet = SlotObjectSheet.ST_0_SlotPanel;
        //                theObject.SpawnSlotPanel(_Map.transform, _Map.Slots[i].transform.position,
        //                   SlotObjectSheet.S_0_Object, mapType, i);
        //            }


        //            if (i < _Map.BottomLeft &&
        //                i % _Map.Horizontal != 0 &&
        //                i % _Map.Horizontal != _Map.TopRight)
        //            {

        //                theObject.SpawnSlotPanel(_Map.transform ,_Map.Slots[i].transform.position,
        //                    SlotObjectSheet.ST_0_SlotPanel, mapType, i);
        //            }
        //        }
        //    }
        //}



        //for (int i = 0; i < _Map.Slots.Length; i++)
        //{
        //    if (i <= _Map.TopRight ||
        //        i >= _Map.BottomLeft ||
        //        i % _Map.Horizontal <= 0 ||
        //        i % _Map.Horizontal >= _Map.TopRight)
        //    {
        //        _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
        //        _Map.Slots[i].nodeColor = NodeColor.NC8_Null;

        //    }

        //    _Map.Slots[i].SlotNum = i;
        //}






        //if (Reset == false)
        //{
        //    for (int i = 0; i < _Map.Slots.Length; i++)
        //    {
        //        if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
        //        {
        //            _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
        //            _Map.Slots[i].nodeColor = NodeColor.NC8_Null;
        //            if (_Map.Slots[i].cube != null)
        //            {
        //                _Map.Slots[i].cube.Resetting();
        //                _Map.Slots[i].cube = null;
        //            }
                    
        //            _Map.Slots[i].TestText.text = i.ToString();
        //        }
        //        else
        //        {
        //            _Map.Slots[i].TestText.text = i.ToString();
        //            _Map.Slots[i].TestText.color = new Color(1, 1, 1);
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < _Map.Slots.Length; i++)
        //    {
        //        if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
        //        {
        //            //변경하지 않을 큐브를 넣는다
        //            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Enemy &&
        //                _Map.Slots[i].nodeColor != NodeColor.NC6_Player &&
        //                _Map.Slots[i].nodeType != PuzzleSlot.NodeType.Object)
        //            {
        //                _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
        //                _Map.Slots[i].nodeColor = NodeColor.NC8_Null;
        //                if (_Map.Slots[i].cube != null)
        //                {
        //                    _Map.Slots[i].cube.Resetting();
        //                    _Map.Slots[i].cube = null;
        //                }
        //            }

        //        }
        //    }
        //}



        //NotMatchSetCube(_Map);



        //if (gameMode == GameMode.MoveMap)
        //{
        //    //Vector2 vec = new Vector2(Player.transform.position.x, Player.transform.position.y + 0.5f);
        //    theCamera.SetBound(_Map, _Map.transform.position, true);
        //}
        //else if (gameMode == GameMode.Battle)
        //{

        //    theCamera.SetBound(_Map, _Map.transform.position, false);
        //}

    }

  

    //골 슬롯에 UI 배치
    //public void SetGoal(MapManager _Map)
    //{
    //    for (int i = _Map.TopLeft; i < _Map.BottomLeft; i++)
    //    {
    //        if (_Map.Slots[i].nodeType == PuzzleSlot.NodeType.Goal)
    //        {
    //            _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Goal;
    //            Goal.transform.position = _Map.Slots[i].transform.position;
    //            Transform Parent = _Map.Slots[i].transform;
    //            Goal.transform.parent = Parent;
    //            break;
    //        }
    //    }
    //}


    // 처음 매치가 안된 상태로 세팅
    public void NotMatchSetCube(MapManager _Map)
    {

        //List<int> ColorList = new List<int>();
        //ColorList.Add(0);
        //ColorList.Add(1);
        //ColorList.Add(2);
        //ColorList.Add(3);
        //ColorList.Add(4);

        //for (int Hor = 0; Hor < _Map.BottomRight; Hor += _Map.Horizontal)
        //{
        //    for (int i = 0; i < _Map.TopRight; i++)
        //    {
        //        if (_Map.Slots[i + Hor].nodeType != PuzzleSlot.NodeType.Null &&
        //            _Map.Slots[i + Hor].nodeColor == NodeColor.NC8_Null)
        //        {
        //            List<int> RandomList = new List<int>(ColorList);
        //            if (_Map.Slots[i + Hor - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //            {
        //                if (_Map.Slots[i + Hor - _Map.Horizontal - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[i + Hor - _Map.Horizontal].nodeColor == _Map.Slots[i + Hor - _Map.Horizontal - _Map.Horizontal].nodeColor)
        //                    {
        //                        RandomList.Remove((int)_Map.Slots[i + Hor - _Map.Horizontal].nodeColor);
        //                    }
        //                    else
        //                    {
        //                        int rand = Random.Range(0, 2);
        //                        if (rand == 0)
        //                        {
        //                            RandomList.Remove((int)_Map.Slots[i + Hor - _Map.Horizontal].nodeColor);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    int rand = Random.Range(0, 2);
        //                    if (rand == 0)
        //                    {
        //                        RandomList.Remove((int)_Map.Slots[i + Hor - _Map.Horizontal].nodeColor);
        //                    }
        //                }


        //            }
        //            if (_Map.Slots[i + Hor - 1].nodeType != PuzzleSlot.NodeType.Null)
        //            {
        //                if (_Map.Slots[i + Hor - 2].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[i + Hor - 1].nodeColor == _Map.Slots[i + Hor - 2].nodeColor)
        //                    {
        //                        RandomList.Remove((int)_Map.Slots[i + Hor - 1].nodeColor);
        //                    }
        //                    else
        //                    {
        //                        int rand = Random.Range(0, 2);
        //                        if (rand == 0)
        //                        {
        //                            RandomList.Remove((int)_Map.Slots[i + Hor - 1].nodeColor);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    int rand = Random.Range(0, 2);
        //                    if (rand == 0)
        //                    {
        //                        RandomList.Remove((int)_Map.Slots[i + Hor - 1].nodeColor);
        //                    }
        //                }


        //            }

        //            int RandColorNum = Random.Range(0, RandomList.Count);
        //            GameObject Cube = theObject.SpawnCube();
        //            SetCube(Cube, _Map.Slots[i + Hor], RandomList[RandColorNum]);

        //        }
               
        //    }
        //}

    }







    // 최초 한번만 실행해서 NULL이 아닌 슬롯에 큐브를 설치
    public void SetCube(GameObject _Cube, PuzzleSlot _Slot, int _Num = -1)
    {
        //int ColorNum = _Num;

        //Cube cube = _Cube.GetComponent<Cube>();
        //_Slot.cube = cube;
        //if (ColorNum == -1)
        //{
        //    ColorNum = Random.Range(0, 5);
        //}
        //_Slot.cube.SpriteRen.sprite = CubeSprites[ColorNum];
        //_Slot.nodeColor = (NodeColor)ColorNum;
        ////if (_Cube.GetComponent<SpriteRenderer>().color.a < 1)
        ////{
        ////    Debug.Log("투명한 상태로 받아옴");
        ////    _Slot.cube.SpriteRen.color = new Color(1, 1, 1, 1);
        ////}

        //cube.nodeColor = (NodeColor)ColorNum;
        //cube.Num = _Slot.SlotNum;
        //_Cube.transform.position = _Slot.transform.position;

    }


    //큐브가 움직일 수 있는지 확인
    public void CheckMoveCube(MapManager _Map, Direction _Direction, int _Num)
    {

        //int ChangeNum = 0;

        //if (_Direction == Direction.Up)
        //{
        //    if (_Map.Slots[_Num - _Map.Horizontal].nodeType == PuzzleSlot.NodeType.Null)
        //        return;
        //    else
        //        ChangeNum = _Num - _Map.Horizontal;
        //}
        //else if (_Direction == Direction.Down)
        //{
        //    if (_Map.Slots[_Num + _Map.Horizontal].nodeType == PuzzleSlot.NodeType.Null)
        //        return;
        //    else
        //        ChangeNum = _Num + _Map.Horizontal;
        //}
        //else if (_Direction == Direction.Left)
        //{
        //    if (_Map.Slots[_Num - 1].nodeType == PuzzleSlot.NodeType.Null)
        //        return;
        //    else
        //        ChangeNum = _Num - 1;
        //}
        //else if (_Direction == Direction.Right)
        //{
        //    if (_Map.Slots[_Num + 1].nodeType == PuzzleSlot.NodeType.Null)
        //        return;
        //    else
        //        ChangeNum = _Num + 1;
        //}

        //state = State.ChangeMatch;
        //ChangeCube(_Map, _Num, ChangeNum, CubeMoveSpeed, true);
        //SelectNum = _Num;
        //OtherNum = ChangeNum;
    }

    //큐브와 큐브의 위치를 바꾸는 기능
    public void ChangeCube(MapManager _Map, int _Num, int _OtherNum, float _Speed = 0.2f, bool _Event = false)
    {

        //if (_Map.Slots[_Num].nodeColor == NodeColor.NC6_Player ||
        //    _Map.Slots[_OtherNum].nodeColor == NodeColor.NC6_Player)
        //{
        //    if (gameMode == GameMode.MoveMap)
        //        Player.ChangeAnim("Run", true);
        //}
        ////_Num의 큐브정보 복제
        //Vector2 Vec = _Map.Slots[_Num].cube.transform.position;
        //GameObject Cube = _Map.Slots[_Num].cube.gameObject;
        //NodeColor nodeColor = _Map.Slots[_Num].nodeColor;

        ////_Num의 정보를 _OtherNum의 정보로 덮어쓰기
        //_Map.Slots[_Num].cube.MoveCube(_Map.Slots[_OtherNum].transform.position, _Speed, false);

        //_Map.Slots[_Num].cube = _Map.Slots[_OtherNum].cube;
        //_Map.Slots[_Num].nodeColor = _Map.Slots[_OtherNum].nodeColor;
        //_Map.Slots[_Num].cube.nodeColor = _Map.Slots[_OtherNum].cube.nodeColor;

        ////_OtherNum을 복제정보로 덮어쓰기
        //_Map.Slots[_OtherNum].cube.MoveCube(_Map.Slots[_Num].transform.position, _Speed, _Event);
        //_Map.Slots[_OtherNum].cube = Cube.GetComponent<Cube>();
        //_Map.Slots[_OtherNum].nodeColor = nodeColor;
        //_Map.Slots[_OtherNum].cube.nodeColor = nodeColor;


        //_Map.Slots[_Num].cube.Num = _Map.Slots[_Num].SlotNum;
        //_Map.Slots[_OtherNum].cube.Num = _Map.Slots[_OtherNum].SlotNum;


    }


    // 이동에서 전투, 전투에서 이동으로 바꾸는 기능
    public void ChangeGameMode()
    {
        //if (gameMode == GameMode.MoveMap)
        //{
        //    IllustSlot.transform.localPosition = BattlePos;
        //    //theCamera.SetBound(theBattleMap.Bound, theBattleMap.Bound.transform.position,false);
        //    MoveUI.SetActive(false);
        //    BattleUI.SetActive(true);
        //    gameMode = GameMode.Battle;
        //    CubeBar.transform.localPosition = new Vector3(0, 2.5f, 0);
        //    for (int i = 0; i < theBattle.Enemy[theBattle.SelectEnemyNum].CubeCount.Length; i++)
        //    {
        //        EnemyCubeCount[i] = theBattle.Enemy[theBattle.SelectEnemyNum].CubeCount[i];
        //    }
        //    state = State.BattleEvent;
        //    theBattle.battleState = BattleState.BattleInit;
        //    theFade.ShowBattleInit(theGirl.Girls[(int)selectGirl].BattlePortrait,
        //        theBattle.Enemy[theBattle.SelectEnemyNum].BattlePortrait);
        //    theFade.FadeInEvent();
        //}
        //else if (gameMode == GameMode.Battle)
        //{
        //    IllustSlot.transform.localPosition = MovePos;
        //    CubeBar.transform.localPosition = new Vector3(0, -3.3f, 0);
        //    theCamera.SetBound(theMoveMap, Player.transform.position, true);
        //    theBattle.Resetting();
        //    MoveUI.SetActive(true);
        //    BattleUI.SetActive(false);
        //    theBattle.ReadySkill(SkillUI.UI2_Null);
        //    gameMode = GameMode.MoveMap;
        //    state = State.Ready;
        //    CubeEvent = false;
        //}
    }


    // 빈칸을 방향에 맞게 채우는 기능
    public void BT_FillBlank(MapManager _Map)
    {
        //if (gameMode == GameMode.Battle)
        //{
        //    FillSpeed = 0.2f;
        //}
        //else
        //    FillSpeed = 0.15f;

        //state = State.FillBlank;
        //bool FirstEvent = true;
        //bool PlayerMove = false; // 플레이어 큐브가 움직였는지 확인 true면 움직임
        //if (_Map.direction == Direction.Down)
        //{
        //    for (int i = 0; i < _Map.TopRight - _Map.TopLeft; i++)
        //    {
        //        for (int Num = _Map.BottomLeft - _Map.Horizontal; Num > _Map.TopRight; Num -= _Map.Horizontal)
        //        {
        //            if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //            {

        //                _Map.Slots[Num + i].cube.gameObject.SetActive(false);

        //                if (_Map.Slots[Num + i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[Num + i].nodeColor == NodeColor.NC6_Player ||
        //                        _Map.Slots[Num + i - _Map.Horizontal].nodeColor == NodeColor.NC6_Player)
        //                    {
        //                        PlayerMove = true;
        //                    }
        //                    ChangeCube(_Map, Num + i, Num + i - _Map.Horizontal, FillSpeed, FirstEvent);
        //                    //if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //                    //    _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        //                    FirstEvent = false;
        //                }
        //                else
        //                {
        //                    GameObject NewCube = theObject.SpawnCube();
        //                    SetCube(NewCube, _Map.Slots[Num + i]);
        //                    NewCube.transform.position = _Map.Slots[Num + i - _Map.Horizontal].transform.position;
        //                    _Map.Slots[Num + i].cube.MoveCube(_Map.Slots[Num + i].transform.position, FillSpeed, FirstEvent);
        //                    FirstEvent = false;
        //                }
        //            }
        //        }

        //    }

        //}
        //else if (_Map.direction == Direction.Up)
        //{
        //    for (int i = 0; i < _Map.TopRight - _Map.TopLeft; i++)
        //    {
        //        for (int Num = _Map.TopLeft + _Map.Horizontal; Num < _Map.BottomRight; Num += _Map.Horizontal)
        //        {
        //            if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //            {

        //                _Map.Slots[Num + i].cube.gameObject.SetActive(false);

        //                if (_Map.Slots[Num + i + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[Num + i].nodeColor == NodeColor.NC6_Player ||
        //                       _Map.Slots[Num + i + _Map.Horizontal].nodeColor == NodeColor.NC6_Player)
        //                    {
        //                        PlayerMove = true;
        //                    }
        //                    ChangeCube(_Map, Num + i, Num + i + _Map.Horizontal, FillSpeed, FirstEvent);
        //                    //if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //                    //    _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        //                    FirstEvent = false;
        //                }
        //                else
        //                {
        //                    GameObject NewCube = theObject.SpawnCube();
        //                    SetCube(NewCube, _Map.Slots[Num + i]);
        //                    NewCube.transform.position = _Map.Slots[Num + i + _Map.Horizontal].transform.position;
        //                    _Map.Slots[Num + i].cube.MoveCube(_Map.Slots[Num + i].transform.position, FillSpeed, FirstEvent);
        //                    FirstEvent = false;
        //                }
        //            }
        //        }

        //    }
        //}
        //else if (_Map.direction == Direction.Left)
        //{
        //    for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.BottomLeft; i += _Map.Horizontal)
        //    {
        //        for (int Num = 0; Num < _Map.TopRight - _Map.TopLeft; Num++)
        //        {
        //            if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //            {

        //                _Map.Slots[Num + i].cube.gameObject.SetActive(false);

        //                if (_Map.Slots[Num + i + 1].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[Num + i].nodeColor == NodeColor.NC6_Player ||
        //                      _Map.Slots[Num + i + 1].nodeColor == NodeColor.NC6_Player)
        //                    {
        //                        PlayerMove = true;
        //                    }
        //                    ChangeCube(_Map, Num + i, Num + i + 1, FillSpeed, FirstEvent);
        //                    //if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //                    //    _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        //                    FirstEvent = false;
        //                }
        //                else
        //                {
        //                    GameObject NewCube = theObject.SpawnCube();
        //                    SetCube(NewCube, _Map.Slots[Num + i]);
        //                    NewCube.transform.position = _Map.Slots[Num + i + 1].transform.position;
        //                    _Map.Slots[Num + i].cube.MoveCube(_Map.Slots[Num + i].transform.position, FillSpeed, FirstEvent);
        //                    FirstEvent = false;
        //                }
        //            }
        //        }

        //    }
        //}
        //else if (_Map.direction == Direction.Right)
        //{
        //    for (int i = _Map.TopRight + _Map.Horizontal; i < _Map.BottomLeft; i += _Map.Horizontal)
        //    {
        //        for (int Num = 0; Num > -(_Map.TopRight - _Map.TopLeft); Num--)
        //        {
        //            if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //            {

        //                _Map.Slots[Num + i].cube.gameObject.SetActive(false);

        //                if (_Map.Slots[Num + i - 1].nodeType != PuzzleSlot.NodeType.Null)
        //                {
        //                    if (_Map.Slots[Num + i].nodeColor == NodeColor.NC6_Player ||
        //                      _Map.Slots[Num + i - 1].nodeColor == NodeColor.NC6_Player)
        //                    {
        //                        PlayerMove = true;
        //                    }
        //                    ChangeCube(_Map, Num + i, Num + i - 1, FillSpeed, FirstEvent);
        //                    //if (_Map.Slots[Num + i].nodeColor == NodeColor.NC5_Blank)
        //                    //    _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        //                    FirstEvent = false;
        //                }
        //                else
        //                {
        //                    GameObject NewCube = theObject.SpawnCube();
        //                    SetCube(NewCube, _Map.Slots[Num + i]);
        //                    NewCube.transform.position = _Map.Slots[Num + i - 1].transform.position;
        //                    _Map.Slots[Num + i].cube.MoveCube(_Map.Slots[Num + i].transform.position, FillSpeed, FirstEvent);
        //                    FirstEvent = false;
        //                }
        //            }
        //        }

        //    }
        //}


        //if (PlayerMove == false && gameMode == GameMode.MoveMap)
        //{
        //    if (gameMode == GameMode.MoveMap)
        //        Player.ChangeAnim("Idle",true);
        //}

        //if (FirstEvent == true)
        //{
        //    state = State.CheckMatch;
        //}
    }

    // 매치가 된 큐브를 제거하는 기능
    public void DestroyCube(MapManager _Map)
    {
        //if (_Map.mapType == MapType.M2_BattleMap)
        //{
        //    theBattle.AddComboValue();
        //}
        //state = State.DestroyCube;




        //for (int i = 0; i < FindMatches.Instance.currentMathces.Count; i++)
        //{
        //    FindMatches.Instance.currentMathces[i].BurstEvent();
        //}
        //FindMatches.Instance.currentMathces.Clear();
    }





    // 플레이어가 있는 슬롯의 넘버를 가져온다
    public int CheckPlayerSlot(MapManager _Map)
    {
        //for (int i = 0; i < _Map.TopRight; i++)
        //{
        //    for (int Hor = 0; Hor < _Map.BottomRight; Hor += _Map.Horizontal)
        //    {
        //        if (_Map.Slots[i + Hor].nodeColor == NodeColor.NC6_Player)
        //        {
        //            return i + Hor;
        //        }
        //    }
        //}
        return 0;
    }

    //매치가 끝난 다음에 이밴트 확인
    public bool CheckEndMatchEvent()
    {
        //int SlotNum = CheckPlayerSlot(theMoveMap);

        ////마을에 도착했는가?


        //if (playerUIs[0].state == PlayerUIState.Die && playerUIs[1].state == PlayerUIState.Die)
        //{
        //    GameOverMove();
        //    return true;
        //}


        ////현재 슬롯이 포탈인지
        //if (theMoveMap.Slots[SlotNum].nodeType == PuzzleSlot.NodeType.Portal)
        //{
        //    CheckPortal(SlotNum);
        //    return true;
        //}


        ////현재 슬롯이 몬스터인지
        //if (theMoveMap.Slots[SlotNum].nodeType == PuzzleSlot.NodeType.Enemy)
        //{
        //    return EnemyEvent(theMoveMap, SlotNum);
        //}
        //else
        //{
        //    Player.CurrentEnemyMeetChance = 0;
        //}

        //if (theMoveMap.Slots[SlotNum].nodeType == PuzzleSlot.NodeType.Normal)
        //{
        //    return false;
        //}
       

        return false;


    }


    // 몬스터 이밴트
    public bool EnemyEvent(MapManager _Map, int i)
    {
        //Player.CurrentEnemyMeetChance += _Map.Slots[i].monsterSheet.addEnemyMeet;

        ////몬스터와 만날 확률을 계산, true면 적과 조우
        //if (CheckEnemyMeet(_Map) == true)
        //{
        //    theSound.FadeOutBGM();
        //    float rand = Random.Range(0.0f, 100.0f);

        //    for (int Index = 0; Index < _Map.Slots[i].monsterSheet.EnemyIndex.Length; i++)
        //    {
        //        if (rand <= _Map.Slots[i].monsterSheet.EnemyChance[Index])
        //        {
        //            theBattle.SelectEnemyNum = _Map.Slots[i].monsterSheet.EnemyIndex[Index];
        //            theFade.FadeOutEvent();
        //            state = State.ChangeMode;
        //            return true;
        //        }
        //    }
        //}
        return false;

    }
    // 현재 적과 조우했는지 확인, true면 전투 시작
    public bool CheckEnemyMeet(MapManager _Map)
    {
        float rand = Random.Range(0.0f, 100.0f);
        if (rand <= Player.CurrentEnemyMeetChance)
        {
            return true;
        }
        return false;
    }

    // 포탈 이밴트
    public void CheckPortal(int _Num)
    {
        // 로딩 화면 넣기
        //theMaker.MapName = theMoveMap.Slots[_Num].portalSheet.MapName; //로드할 맵 이름
        //theMaker.PlayerStartNum = theMoveMap.Slots[_Num].portalSheet.NextPosNum; //로드 후 플레이어 위치
        state = State.LoadingMap;
        Player.ChangeAnim("Idle",true);
        theFade.FadeOutEvent();

        StartCoroutine(PortalCor());

    }



    public IEnumerator PortalCor()
    {
        while (theFade.FadeOutEnd == false)
        {
            yield return Wait;
        }
        theFade.FadeOutEnd = false;
        ResetMoveMap();

        //theGM.LoadMap();
    }

    //현재 매치가 가능한 상태가 있는지 체크 true 면 가능, false 면 불가능
    public bool DeadlockCheck(MapManager _Map)
    {
        NodeColor CopyColor;


        //for (int i = 0; i < _Map.Horizontal * _Map.Vertical; i++)
        //{
        //    if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
        //    {

        //        //해당 슬롯의 큐브가 특수블럭인지 확인
        //        if (_Map.Slots[i].cube.specialCubeType != SpecialCubeType.Null)
        //        {
        //            HintNum = i;
        //            return true;
        //        }

        //        // 상
        //        if (_Map.Slots[i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //        {

        //            CopyColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = _Map.Slots[i - _Map.Horizontal].nodeColor;
        //            _Map.Slots[i - _Map.Horizontal].nodeColor = CopyColor;

        //            if (theMatch.FindAllMatches(theMoveMap, false))
        //            {
        //                isMatched = false;
        //                HintNum = i;
        //                return true;
        //            }


        //            _Map.Slots[i - _Map.Horizontal].nodeColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = CopyColor;
        //        }
        //        // 하
        //        if (_Map.Slots[i + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
        //        {

        //            CopyColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = _Map.Slots[i + _Map.Horizontal].nodeColor;
        //            _Map.Slots[i + _Map.Horizontal].nodeColor = CopyColor;

        //            theMatch.FindAllMatches(theMoveMap, false);

        //            _Map.Slots[i + _Map.Horizontal].nodeColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = CopyColor;

        //            if (isMatched)
        //            {


        //                isMatched = false;
        //                HintNum = i;
        //                return true;
        //            }
        //        }
        //        // 좌
        //        if (_Map.Slots[i - 1].nodeType != PuzzleSlot.NodeType.Null)
        //        {

        //            CopyColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = _Map.Slots[i - 1].nodeColor;
        //            _Map.Slots[i - 1].nodeColor = CopyColor;

        //            theMatch.FindAllMatches(_Map, false);

        //            _Map.Slots[i - 1].nodeColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = CopyColor;

        //            if (isMatched)
        //            {


        //                isMatched = false;
        //                HintNum = i;
        //                return true;
        //            }
        //        }
        //        // 우
        //        if (_Map.Slots[i + 1].nodeType != PuzzleSlot.NodeType.Null)
        //        {

        //            CopyColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = _Map.Slots[i + 1].nodeColor;
        //            _Map.Slots[i + 1].nodeColor = CopyColor;

        //            theMatch.FindAllMatches(_Map, false);

        //            _Map.Slots[i + 1].nodeColor = _Map.Slots[i].nodeColor;
        //            _Map.Slots[i].nodeColor = CopyColor;

        //            if (isMatched)
        //            {
        //                isMatched = false;
        //                HintNum = i;
        //                return true;
        //            }
        //        }

        //    }
        //}

        return false;

    }


    public void BT_ShowHint()
    {
        DeadlockCheck(theMoveMap);
        theObject.SpawnSelectSlot(theMoveMap.Slots[HintNum].transform.position);
    }




    //MoveCount를 조종한다. 
    public void SetMoveCount(int _Count = 0)
    {
        if (gameMode == GameMode.MoveMap)
        {
            MoveCount += _Count;

            if (MoveCount > 50 && foodState != FoodState.FS0_Full)
            {
                foodState = FoodState.FS0_Full;
                FoodImage.sprite = FoodSprites[(int)foodState];
            }
            else if (MoveCount <= 50 && MoveCount > 0&& foodState != FoodState.FS1_Half)
            {
                foodState = FoodState.FS1_Half;
                FoodImage.sprite = FoodSprites[(int)foodState];
            }
            else if (MoveCount <=0 && foodState != FoodState.FS2_Null)
            {
                foodState = FoodState.FS2_Null;
                FoodImage.sprite = FoodSprites[(int)foodState];
            }


            if (MoveCount < 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (playerUIs[i].CurrentHp > 0)
                    {
                        playerUIs[i].TakeDamageEvent(playerUIs[i].MaxHp / 10);
                    }
                }

                MoveCount = 0;


                if (selectGirl == playerUIs[0].selectGirl && 
                    playerUIs[0].state == PlayerUIState.Die && 
                    playerUIs[1].state != PlayerUIState.Die)
                {
                    Player.SetSpine((int)playerUIs[1].selectGirl);
                }
                else if (selectGirl == playerUIs[1].selectGirl &&
                    playerUIs[1].state == PlayerUIState.Die &&
                    playerUIs[0].state != PlayerUIState.Die)
                {
                    Player.SetSpine((int)playerUIs[0].selectGirl);
                }

            }
                

            MoveCountText.text = MoveCount.ToString();
        }
        else if (gameMode == GameMode.Battle)
        {
            theBattle.SetEnemyCount(-1);
        }


    }



    // 이동맵에서 방향을 바꾼다
    public void BT_ChangeDirection(int _Num)
    {
        if (state == State.Ready)
        {
            theMoveMap.direction = (Direction)_Num;
            CameraButton.ButtonChange(_Num);
            Player.ChangeDirection(theMoveMap.direction);
        }
        else if (state == State.Tutorial)
        {
            theMoveMap.direction = (Direction)_Num;
            CameraButton.ButtonChange(_Num);
            Player.ChangeDirection(theMoveMap.direction);
        }
    }

    // 플레이어 UI를 세팅한다
    public void SetPlayerUi()
    {
        List<int> ColorList = new List<int>();
        ColorList.Add(0);
        ColorList.Add(1);
        ColorList.Add(2);
        ColorList.Add(3);
        ColorList.Add(4);
        ColorList.Remove(FirstHeroNum);
        ColorList.Remove(secondHeroNum);
        PlayerCubeUI[0].SetCubeUi(FirstHeroNum, 0, CubeUiSprites[FirstHeroNum]);
        PlayerCubeUI[1].SetCubeUi(secondHeroNum, 1, CubeUiSprites[secondHeroNum]);
        playerUIs[0].SetUi(FirstHeroNum, 0);
        playerUIs[1].SetUi(secondHeroNum, 1);
        for (int i = 0; i < 3; i++)
        {
            PlayerCubeUI[i + 2].SetCubeUi(ColorList[i], i + 2, CubeUiSprites[ColorList[i]]);
        }
    }





    // 전투 시작전 적 큐브를 깍는 이밴트
    public void CheckEnemyCubeCount()
    {
        //if (StartEffect.Count > 0)
        //{
        //    for (int i = 0; i < StartEffect.Count;)
        //    {
        //        if (StartEffect[i].activeSelf == false)
        //        {
        //            StartEffect.RemoveAt(i);
        //        }
        //        else
        //        {
        //            i++;
        //        }
        //    }
        //}


        //if (EventTime <= 0.1f)
        //{
        //    EventTime += Time.deltaTime;
        //}
        //else
        //{
        //    EventEnd = true; // false일 경우 이밴트가 실행 true일 경우 이밴트 종료
        //    EventTime = 0.1f;
        //    for (int i = 0; i < theBattle.Enemy[theBattle.SelectEnemyNum].CubeCount.Length; i++)
        //    {
        //        for (int UINum = 0; UINum < CubeSprites.Length; UINum++)
        //        {
        //            if (theBattle.EnemyCubeUi[i].cubeColor == PlayerCubeUI[UINum].cubeColor)
        //            {
        //                if (EnemyCubeCount[i] <= 0)
        //                {
        //                    continue;
        //                }
        //                int CubeCount = 0;
        //                if (EnemyCubeCount[i] >= 10 && PlayerCubeUI[UINum].CubeCount >= 10)
        //                {
        //                    CubeCount = 10;
        //                }
        //                else if (EnemyCubeCount[i] >= PlayerCubeUI[UINum].CubeCount)
        //                {
        //                    CubeCount = PlayerCubeUI[UINum].CubeCount;
        //                }
        //                else if (EnemyCubeCount[i] <= PlayerCubeUI[UINum].CubeCount)
        //                {
        //                    CubeCount = EnemyCubeCount[i];
        //                }
        //                EnemyCubeCount[i] -= CubeCount;


        //                if (PlayerCubeUI[UINum].CubeCount > 0)
        //                {
        //                    PlayerCubeUI[UINum].AddCount(-CubeCount);
        //                    EventEnd = false;
        //                    GameObject Effect = theObject.CubeEffectEvent(PlayerCubeUI[UINum].transform.position,
        //                        theBattle.EnemyCubeUi[i].gameObject, PlayerCubeUI[UINum].cubeColor,
        //                        (CubeEffectType)1, -CubeCount, false, 10000);
        //                    StartEffect.Add(Effect);
        //                }

        //            }
        //        }
        //    }

        //    if (EventEnd == true && StartEffect.Count == 0)
        //    {
        //        EventTime = 0;
        //        EventEnd = false;
        //        state = State.Ready;
        //        Vector2 vec = new Vector2(playerUIs[0].transform.position.x,
        //            playerUIs[0].transform.position.y + 1.8f);
        //        // theObject.SpeechEvent(vec, "전투 시작!!!", 3);
        //        theBattle.BattleStart = true;
        //        theBattle.battleState = BattleState.Null;
        //        theMessage.MessageEnd = false;
        //        theEnd.GameEndOn = true;
        //    }


        //}
    }

    //몬스터가 죽으면 해당 몬스터 데이터시트 확인한다
    public void CheckEnemyData()
    {
        //int SlotNum = CheckPlayerSlot(theMoveMap);
        

        //if (theMoveMap.Slots[SlotNum].monsterSheet.OnlyOneEnemy == true)
        //{
        //    int DataNum = theMoveMap.Slots[SlotNum].monsterSheet.OnlyOneNum;

        //    theGM.EnemyDataSheet[DataNum] = true;

        //    for (int Hor = 0; Hor < theMoveMap.BottomRight; Hor += theMoveMap.Horizontal)
        //    {
        //        for (int i = 0; i <= theMoveMap.TopRight; i++)
        //        {
        //            if (theMoveMap.Slots[i + Hor].nodeType == PuzzleSlot.NodeType.Enemy)
        //            {
        //                if (theMoveMap.Slots[i + Hor].monsterSheet.OnlyOneEnemy == true &&
        //                 theMoveMap.Slots[i + Hor].monsterSheet.OnlyOneNum == DataNum)
        //                {
        //                    theMoveMap.Slots[i + Hor].slotObject.ResetEnemy();
        //                    theMoveMap.Slots[i + Hor].nodeType = PuzzleSlot.NodeType.Normal;
        //                }
        //            }
                 
        //        }
        //    }
        //}
        //else
        //{
        //    return;
        //}

    }

    public void ChangePlayer(SelectGirl _Girl)
    {
        if (selectGirl != _Girl)
        {
            selectGirl = _Girl;

            Player.SetSpine((int)selectGirl);
        }
    }


    public void GameOverMove()
    {
        gameMode = GameMode.GameOver;

        string Dec = "";
        switch (theMaker.mapMainType)
        {
            case MapMainType.M0_Forest:
                Dec = "우리는 배고픔에 지쳐 바닥과 하나가 되었다.";
                break;
        }
        theSound.PlaySE("Lose");
        theFade.ShowBlackChat("패배", Dec);
    }

    public void GameOverBattle()
    {
        gameMode = GameMode.GameOver;
        theSound.PlaySE("Lose");
        theFade.ShowBlackChat("패배", theBattle.Enemy[theBattle.CurrentEnemyCount].LoseDec);
    }

    public void GameOver()
    {
        ResetMoveMap();


        if (BattleUI.activeSelf == true)
        {
            IllustSlot.transform.localPosition = MovePos;
            CubeBar.transform.localPosition = new Vector3(0, -3.3f, 0);
            theBattle.Resetting();
            MoveUI.SetActive(true);
            BattleUI.SetActive(false);
            theBattle.ReadySkill(SkillUI.UI2_Null);
        }
        theGM.EnemyDataSheet[0] = false;
        theGM.EnemyDataSheet[1] = false;
        theGM.CurrentProgressNum = 0;

        theMaker.m_MapName = "속삭이는 숲1";
        theMaker.PlayerStartNum = 20;
        MoveCount = 0;
        SetMoveCount(100);
        theFade.CloseBlackChat();
        theFade.FadeInEvent();
        theTitle.ReturnTitle();
    }



    // 퍼즐 슬롯을 모두 리셋(초기화)시키는 이밴트
    public void PuzzleResetting(MapManager _Map)
    {
        //for (int i = 0; i < _Map.Horizontal * _Map.Vertical; i++)
        //{
        //    _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
        //    _Map.Slots[i].nodeColor = NodeColor.NC8_Null;
        //    _Map.Slots[i].cube.Resetting();
        //    _Map.Slots[i].cube = null;
        //}
    }

    public void CheckMoveBGM()
    {
        if (!theSound.BGMSound.audioSource.isPlaying)
        {
            switch (theMaker.mapMainType)
            {
                case MapMainType.M0_Forest:
                    theSound.PlayBGM("ForestMove");
                    break;
            }
        }
    }

    public void CheckMoveMessage()
    {
        //if (theGM.CurrentProgressNum == 0)
        //{
        //    theGM.CurrentProgressNum = 1;
        //    theMessage.ShowMessageText(0);
        //}
        
    }

    public void CheckTutorialMessage()
    {

        // 타이틀을 누르고 처음 시작했을때
        if (theGM.CurrentProgressNum == 0)
        {
            theGM.CurrentProgressNum = 1;
            theMessage.ShowMessageText(0);
        }
        // 처음으로 매치를 했다
        else if (theGM.CurrentProgressNum == 1)
        {
            theGM.CurrentProgressNum = 2;
            theMessage.ShowMessageText(1);
            state = State.Tutorial;
            theTuto.TutoDir = theMoveMap.direction;
        }
        // 방향키를 바꿨다
        else if (theGM.CurrentProgressNum == 2 && theTuto.TutoDir != theMoveMap.direction)
        {
            theCameraButton.TouchUp();
            theGM.CurrentProgressNum = 3;
            theMessage.ShowMessageText(2);
            state = State.Ready;
        }
        //속삭이는 숲2에 도착했다
        else if (theGM.CurrentProgressNum == 3 && theMaker.m_MapName == "속삭이는 숲2")
        {
            theGM.CurrentProgressNum = 4;
            theMessage.ShowMessageText(3);
            state = State.Ready;
        }
        //포이즌 슬라임을 처치
        else if (theGM.CurrentProgressNum == 5 && theGM.EnemyDataSheet[1] == true)
        {
            state = State.Tutorial;
            theGM.CurrentProgressNum = 6;
            theMessage.ShowMessageText(5, true);
        }
        else if (theGM.CurrentProgressNum == 6 && theMessage.MessageEnd == true)
        {
            theMessage.MessageEnd = false;
            gameMode = GameMode.GameOver;

            string Dec = "플레이해 주셔서 감사합니다.";

            theSound.PlaySE("Win");
            theFade.ShowBlackChat("튜토리얼 완료", Dec);
        }
        // 식량이 0이 됬다
        else if (MoveCount == 0)
        {
            SetMoveCount(999);
            theMessage.ShowMessageText(6);

        }
        else
        {
            theEnd.GameEndOn = true;
        }

    }



    public void ResetMoveMap()
    {
        theBattleMap.FirstBattle = false;

        for (int Hor = 0; Hor < theMoveMap.BottomRight; Hor += MatchBase.MaxHorizon)
        {
            for (int i = 0; i < theMoveMap.TopRight; i++)
            {
                theMoveMap.Slots[i + Hor].Resetting();
            }
        }

        //for (int i = 0; i < theObject.EnemySkullList.Count; i++)
        //{
        //    if (theObject.EnemySkullList[i].activeSelf == true)
        //    {
        //        theObject.EnemySkullList[i].gameObject.SetActive(false);
        //        theObject.EnemySkulls.Enqueue(theObject.EnemySkullList[i]);
        //    }
        //}
        //for (int i = 0; i < theObject.SlotPanelList.Count; i++)
        //{
        //    if (theObject.SlotPanelList[i].activeSelf == true)
        //    {
        //        theObject.SlotPanelList[i].GetComponent<SlotObject>().Resetting();
        //    }
        //}
        //for (int i = 0; i < theObject.PortalList.Count; i++)
        //{
        //    if (theObject.PortalList[i].activeSelf == true)
        //    {
        //        theObject.PortalList[i].GetComponent<ParticleManager>().Resetting();
        //    }
        //}

        //for (int i = 0; i < theObject.PortalArrowList.Count; i++)
        //{
        //    if (theObject.PortalArrowList[i].activeSelf == true)
        //    {
        //        theObject.PortalArrowList[i].GetComponent<PortalArrowManager>().Resetting();
        //    }
        //}

    }




}
