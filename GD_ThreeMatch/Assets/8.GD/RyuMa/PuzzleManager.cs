using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public enum FoodState
{ 
    FS0_Full = 0,
    FS1_Half,
    FS2_Null,
}


[System.Serializable]
public struct BlockSeed
{
    public BlockType type;
    public string[] Data;


    public BlockSeed(BlockType _type, string[] _Data)
    {
        type = _type;
        Data = _Data;
    }



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
        FillBlank,          //매치한 블럭들이 모두 사라지고 빈곳을 채워준다
        Shuffle,            //더이상 매치가 없어서 셔플해준다
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
    [HideInInspector]
    public Block PlayerBlock;





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

    //가장 최근에 선택된 슬롯 번호
    public int SelectNum = 0;
    public int OtherNum = 0;

    //플레이어 위치
    public int StartWorldIndex;

    //최초 한번만 스텝을 확인
    public bool CheckOneStep;



    int HintNum;
    int[] EnemyCubeCount = new int[6];
    WaitForSeconds Wait = new WaitForSeconds(0.1f);
    float FillSpeed = 0.1f;
    FoodState foodState;


    Vector3 MovePos = new Vector3(0, 800, 0); //전투가 끝난후 UI위치
    Vector3 BattlePos = new Vector3(0, -750, 0); // 전투 시작시 UI위치


    //[HideInInspector] public List<string> PortalName;
    List<GameObject> StartEffect = new List<GameObject>();
    [HideInInspector] public List<PuzzleSlot> BurstList = new List<PuzzleSlot>();

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
        theObject = ObjectManager.Instance;
        theCamera = FindObjectOfType<CameraManager>();
        theGirl = FindObjectOfType<GirlManager>();
        //BT_ChangeDirection(1);
    }




    private void Update()
    {
        if (CheckEvent == true)
        {
            if (EventTime > 0)
                EventTime -= Time.deltaTime;
        }

        UpdateStep();

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



    }


    public void UpdateStep()
    {
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
        // 매치한 블럭들을 버스팅해주고 있다
        else if (state == State.MatchBurst)
        {
            CheckBursting();
        }
        else if (state == State.FillBlank)
        {
            CheckFillBlank();
        }
        else if (state == State.Shuffle)
        {
            CheckSuffling();
        }
    }


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





    #region 스텝



    //스텝

    public void CheckSwitching()
    {
        if (EventEnd)
        {
            //매치가 가능한가 체크
            if (theMatch.FindAllMatches(GetMap()))
            {
                //스위치 성공시 스텝 활성화
                CheckOneStep = true;
                state = State.MatchBurst;

                //매치가 된 블럭들을 모두 버스팅 해준다
                BurstEvent();


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

    public void CheckBursting()
    {
        if (EventEnd)
        {
            BurstResset();
            if (theMatch.FindBlank(GetMap()))
            {
                EventUpdate(MatchBase.BlockSpeed);
                state = State.FillBlank;
            }
            else
            {
                state = State.Ready;
            }


        }
    }





    //빈칸을 한칸씩 움직여준다, 모두 움직이면 다시 매치 체크 및 스텝
    public void CheckFillBlank()
    {
        if (EventEnd)
        {
            //먼저 포탈 스텝을 확인한다
            CheckPortal();


            //빈칸이 있는지 없는지 확인
            if (theMatch.FindBlank(GetMap()) == false)
            {

                //빈칸이 없을 경우 다시한번 매치가 있는지 확인
                if (theMatch.FindAllMatches(GetMap()))
                {
                    state = State.MatchBurst;
                    //매치가 된 블럭들을 모두 버스팅 해준다
                    BurstEvent();
                }
                else
                {
                    //매치가 더이상 없을 경우 모든 스텝을 확인

                    if (CheckOneStep == true)
                    {


                        CheckOneStep = false;
                    }
                    //매치가 불가능한 경우 셔플 시작
                    if (theMatch.CheckCanMatch(GetMap()) == false)
                    {
                        if (theMatch.Shuffling(GetMap()))
                        {
                            state = State.Shuffle;


                            StartCoroutine(ShuffleEvent(GetMap()));
                            EventUpdate(1f);
                            return;

                        }
                        else
                        {
                            //셔플을 해도 매치가 안된다면 게임오버

                        }
                    }

                    //매치가 더이상 없을 경우 모든 스텝을 확인
                    state = State.Ready;

                }
            }
            else
            {
                EventUpdate(MatchBase.BlockSpeed);
            }
        }

    }

    public void CheckSuffling()
    {
        if (EventEnd)
        {
            state = State.Ready;
        }

    }

    List<Block> ShuffleList = new List<Block>();

    IEnumerator ShuffleEvent(MapManager map)
    {
  
        for (int y = MatchBase.MaxHorizon; y < map.BottomLeft; y += MatchBase.MaxHorizon)
        {
            for (int x = 1; x < map.TopRight; x++)
            {
                if (map.Slots[x + y].CheckSwitch() && map.Slots[x + y].m_Block.nodeColor <= NodeColor.NC4_Yellow)
                {
                    map.Slots[x + y].m_Block.transform.DOScale(0, 0.5f);
                    ShuffleList.Add(map.Slots[x + y].m_Block);
                }

            }
        }
        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < ShuffleList.Count; i++)
        {
            ShuffleList[i].SetColor(ShuffleList[i].nodeColor);
            ShuffleList[i].transform.DOScale(1, 0.5f);

        }
        ShuffleList.Clear();
        yield return new WaitForSeconds(0.5f);

    }




    //포탈이 캐릭터와 겹쳤는지 확인
    public void CheckPortal()
    {
        return;
        if (PlayerBlock.m_Slot.m_MiddlePanel != null)
        {
            if (PlayerBlock.m_Slot.m_MiddlePanel.panelType == PanelType.PT1_Portal)
            {
                PortalPanel Portal = (PortalPanel)PlayerBlock.m_Slot.m_MiddlePanel;
                MapName = Portal.PortalMapName;
                SaveManager.Instance.LoadMap(MapName);
                StartWorldIndex = Portal.m_Count;
            }
        }

    }






    #endregion



    public List<BlockSeed> m_EditSeed = new List<BlockSeed>();
    public List<BlockSeed> m_BlockSeed = new List<BlockSeed>();



    //새로운 맵에 캐릭터를 넣는다
    public void InsertPlayer(MapManager map)
    {
        return;

        if (map.Slots[StartWorldIndex].m_Block != null)
            map.Slots[StartWorldIndex].m_Block.Resetting();

        map.Slots[StartWorldIndex].m_Block = PlayerBlock;

        PlayerBlock.m_Slot = map.Slots[StartWorldIndex];


    }

    public void IsBurstingReset(MapManager map)
    {
        for (int y = MatchBase.MaxHorizon; y < map.BottomLeft; y += MatchBase.MaxHorizon)
        {
            for (int x = 1; x < map.TopRight; x++)
            {
                map.Slots[x + y].m_isBursting = false;
            }
        }
    }


    //매치가 된 블럭, 주변 블럭을 모두 버스팅 해준다
    public void BurstEvent()
    {
        for (int i = 0; i < theMatch.currentMathces.Count; i++)
        {
            theMatch.currentMathces[i].BurstEvent();
        }
        for (int i = 0; i < AroundSlot.Count; i++)
        {
            AroundSlot[i].CheckAroundBurst();
        }

        AroundSlot.Clear();
        theMatch.currentMathces.Clear();
    }







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






    public void BT_ShowHint()
    {
        theMatch.CheckCanMatch(GetMap());
        HintNum = theMatch.CanMatchNum;

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








    //해당 슬롯에 블럭을 생성해준다
    public void CreateBlock(PuzzleSlot _Slot)
    {

        _Slot.CreatBlock(BlockType.BT0_Cube, null);

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


    [HideInInspector]
    public List<PuzzleSlot> AroundSlot = new List<PuzzleSlot>();


    public void BurstResset()
    {
        for (int i = 0; i < BurstList.Count; i++)
        {
            BurstList[i].m_isBursting = false;
        }
        BurstList.Clear();
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
