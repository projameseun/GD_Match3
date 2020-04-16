using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Direction
{ 
    Up = 0,
    Down,
    Left,
    Right
}


public class PuzzleManager : MonoBehaviour
{
    public enum GameMode
    { 
        MoveMap,
        Battle
    }

    public enum State
    { 
        Ready = 0,
        ChangeMatch,
        ChangeMatchRetrun,
        DestroyCube,
        FillBlank,
        CheckMatch
    }
    public GameMode gameType = GameMode.MoveMap;
    public State state;
    public Direction direction;


    public Sprite[] CubeSprites;
    public CameraButtonManager[] CameraButton;
    public PuzzleSlot[] MoveSlots;
    public PuzzleSlot[] BattleSlots;

    //UI 오브젝트
    public GameObject Goal;
    public GameObject MinimapBase;
    public GameObject MoveUI;
    public GameObject BattleUI;
    public GameObject MovePos;
    public GameObject BattlePos;
    public GameObject IllustSlot;
    //메치가 되면 true;
    public bool isMatched = false;
    private FindMatches findMatches;

    //DB
    public bool SlotDown = false;
    public bool CubeEvent = false;
    int SelectNum = 0;
    int OtherNum = 0;
    public bool Test = false; // true면 FillBlank실행
    public PlayerCube Player;

    //DB
    public int Horizontal;
    public int Vertical;
    public int TopLeft;
    public int TopRight;
    public int BottomLeft;
    public int BottomRight;

    public int Battle_Horizontal;
    public int Battle_Vertical;
    public int Battle_TopLeft;
    public int Battle_TopRight;
    public int Battle_BottomLeft;
    public int Battle_BottomRight;


    private ObjectManager theObject;
    private FindMatches theMatch;
    private FadeManager theFade;
    private void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        Player = FindObjectOfType<PlayerCube>();
        theMatch = FindObjectOfType<FindMatches>();
        theObject = FindObjectOfType<ObjectManager>();

        for (int i = 0; i < MoveSlots.Length; i++)
        {
            if (i <= TopRight || i >= BottomLeft || i % Horizontal <= TopLeft || i % Horizontal >= TopRight)
            {
                MoveSlots[i].nodeType = PuzzleSlot.NodeType.Null;
                MoveSlots[i].nodeColor = PuzzleSlot.NodeColor.Null;
            }

            MoveSlots[i].SlotNum = i;
        }

        findMatches = FindObjectOfType<FindMatches>();


    }



    private void Update()
    {
        if (gameType == GameMode.MoveMap)
        {
            if (theFade.FadeEvent == true)
            {
                theFade.FadeEvent = false;
                gameType = GameMode.Battle;
                ChangeGameMode();
            }


            if (state == State.ChangeMatch) //  큐브를 교환하는 상태
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;

                    //매치 조건이 맞는지 확인한다
                    findMatches.FindAllMatches();
                    if (isMatched)
                    {
                        DestroyCube(MoveSlots);
                        return;
                    }


                    //매치가 안될경우
                    if (!isMatched)
                    {
                        ChangeCube(MoveSlots, SelectNum, OtherNum, true);
                        Debug.Log("Not Natched");
                        state = State.ChangeMatchRetrun;
                    }

                }
            }
            else if (state == State.ChangeMatchRetrun)// 매치조건이 없어서 다시 원위치
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    Player.ChangeAnim("Idle", true);
                    state = State.Ready;
                }
            }
            else if (state == State.FillBlank)// 빈칸을 채우는 상태
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    BT_FillBlank(MoveSlots, Horizontal);

                }
            }
            else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
            {
                findMatches.FindAllMatches();
                if (isMatched)
                {
                    DestroyCube(MoveSlots);
                    return;
                }


                //매치가 안될경우
                if (!isMatched)
                {
                    Debug.Log("정지");
                    Player.ChangeAnim("Idle", true);
                    state = State.Ready;
                }
            }
            else if (state == State.DestroyCube)//매치된 큐브 제거
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    BT_FillBlank(MoveSlots, Horizontal);
                }
            }
        }
        else if (gameType == GameMode.Battle)
        {
            if (theFade.FadeEvent == true)
            {
                theFade.FadeEvent = false;
                gameType = GameMode.MoveMap;
                ChangeGameMode();
            }
        }



    }




    //슬롯을 채운다. false일 경우 최초실행, true일 경우 현재 맵에서 리셋
    public void SetSlot(PuzzleSlot[] _Slot,int _Horizontal, int _Vertical, bool Reset = false)
    {
        for (int i = 0; i < _Slot.Length; i++)
        {
            if (_Slot[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                if (Reset == true)
                {
                    _Slot[i].nodeType = PuzzleSlot.NodeType.Normal;
                    _Slot[i].nodeColor = PuzzleSlot.NodeColor.Null;
                }
                else
                {
                    if(_Slot[i].nodeColor != PuzzleSlot.NodeColor.Player)
                        _Slot[i].nodeColor = PuzzleSlot.NodeColor.Null;
                }
                if (_Slot[i].cube != null)
                {
                    _Slot[i].cube.Resetting();
                    _Slot[i].cube = null;
                }
                _Slot[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                _Slot[i].TestText.text = i.ToString();
            }
            else
            {
                _Slot[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                _Slot[i].TestText.text = i.ToString();
                _Slot[i].TestText.color = new Color(1, 1, 1);
            }
        }


        NotMatchSetCube(_Slot,Horizontal);

        for (int i = 0; i < _Slot.Length; i++)
        {
            if (_Slot[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                _Slot[i].TestText.text = i.ToString();
                _Slot[i].cube.Num = i;
            }

        }

        //theMatch.FindAllMatches(false);
        //if (isMatched == true)
        //{
        //    isMatched = false;
        //    SetSlot();
        //    return;
        //}
        if (Reset == false)
        {
            while (true)
            {
                int rand = Random.Range(0, _Horizontal * _Vertical);

                if (_Slot[rand].nodeType != PuzzleSlot.NodeType.Null)
                {
                    _Slot[rand].nodeColor = PuzzleSlot.NodeColor.Player;
                    _Slot[rand].cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    Player.transform.position = _Slot[rand].transform.position;

                    Transform Parent = _Slot[rand].cube.transform;

                    Player.transform.parent = Parent;
                    Player.ChangeDirection(direction);

                    break;
                }
            }
            while (true)
            {
                int rand = Random.Range(0, _Horizontal * _Vertical);

                if (_Slot[rand].nodeType != PuzzleSlot.NodeType.Null &&
                    _Slot[rand].nodeColor != PuzzleSlot.NodeColor.Player)
                {
                    _Slot[rand].nodeType = PuzzleSlot.NodeType.Enemy;

                    break;
                }
            }
            while (true)
            {
                int rand = Random.Range(0, _Horizontal * _Vertical);

                if (_Slot[rand].nodeType != PuzzleSlot.NodeType.Null &&
                    _Slot[rand].nodeColor != PuzzleSlot.NodeColor.Player &&
                    _Slot[rand].nodeType != PuzzleSlot.NodeType.Enemy)
                {
                    _Slot[rand].nodeType = PuzzleSlot.NodeType.Goal;
                    Goal.transform.position = _Slot[rand].transform.position;
                    Transform Parent = _Slot[rand].cube.transform;
                    Goal.transform.parent = Parent;
                    break;
                }
            }
        }

       

    }

    // 처음 매치가 안된 상태로 세팅
    public void NotMatchSetCube(PuzzleSlot[] _Slot,int _Horizontal)
    {
        PuzzleSlot CopySlot = new PuzzleSlot();
        List<int> ColorList = new List<int>();
        ColorList.Add(0);
        ColorList.Add(1);
        ColorList.Add(2);
        ColorList.Add(3);
        ColorList.Add(4);
        ColorList.Add(5);
        for (int i = TopLeft + _Horizontal; i < BottomRight - _Horizontal; i++)
        {
            if (_Slot[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Slot[i].nodeColor  == PuzzleSlot.NodeColor.Null)
            {
                List<int> RandomList = new List<int>(ColorList);
                if (_Slot[i - _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {
                    if (_Slot[i - _Horizontal - _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Slot[i - _Horizontal].nodeColor == _Slot[i - _Horizontal - _Horizontal].nodeColor)
                        {
                            RandomList.Remove((int)_Slot[i - _Horizontal].nodeColor);
                        }
                        else
                        {
                            int rand = Random.Range(0, 2);
                            if (rand == 0)
                            {
                                RandomList.Remove((int)_Slot[i - _Horizontal].nodeColor);
                            }
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            RandomList.Remove((int)_Slot[i - _Horizontal].nodeColor);
                        }
                    }

                   
                }
                if (_Slot[i - 1].nodeType != PuzzleSlot.NodeType.Null)
                {
                    if (_Slot[i - 2].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Slot[i - 1].nodeColor == _Slot[i - 2].nodeColor)
                        {
                            RandomList.Remove((int)_Slot[i - 1].nodeColor);
                        }
                        else
                        {
                            int rand = Random.Range(0, 2);
                            if (rand == 0)
                            {
                                RandomList.Remove((int)_Slot[i - 1].nodeColor);
                            }
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            RandomList.Remove((int)_Slot[i - 1].nodeColor);
                        }
                    }
                    
                    
                }

                int RandColorNum = Random.Range(0, RandomList.Count);
                GameObject Cube = theObject.FindObj("Cube");
                Cube.GetComponent<SpriteRenderer>().sprite = CubeSprites[RandomList[RandColorNum]];
                Cube.GetComponent<Cube>().MinimapSprite.sprite = CubeSprites[RandomList[RandColorNum]];
                _Slot[i].nodeColor = (PuzzleSlot.NodeColor)RandomList[RandColorNum];

                Cube.transform.position = _Slot[i].transform.position;
                _Slot[i].cube = Cube.GetComponent<Cube>();

            }
        }


    }


    // 최초 한번만 실행해서 NULL이 아닌 슬롯에 큐브를 설치
    public void SetCube(GameObject _Cube, PuzzleSlot _Slot)
    {
        int rand = Random.Range(0, CubeSprites.Length);
        _Cube.GetComponent<SpriteRenderer>().sprite = CubeSprites[rand];
        _Slot.nodeColor = (PuzzleSlot.NodeColor)rand;

        _Cube.transform.position = _Slot.transform.position;
        _Slot.cube = _Cube.GetComponent<Cube>();
    }


    //큐브가 움직일 수 있는지 확인
    public void CheckMoveCube(int _Num, Direction _direction,int _Horizontal)
    {

        int ChangeNum = 0;

        if (_direction == Direction.Up) 
        {
            if (MoveSlots[_Num - _Horizontal].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - _Horizontal;
        }
        else if (_direction == Direction.Down)
        {
            if (MoveSlots[_Num + _Horizontal].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num + _Horizontal;
        }
        else if (_direction == Direction.Left)
        {
            if (MoveSlots[_Num -1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - 1;
        }
        else if (_direction == Direction.Right)
        {
            if (MoveSlots[_Num +1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num +1;
        }

        state = State.ChangeMatch;
        ChangeCube(MoveSlots ,_Num, ChangeNum,true);
        SelectNum = _Num;
        OtherNum = ChangeNum;
    }


    public void ChangeCube(PuzzleSlot[] _Slots,int _Num, int _OtherNum,bool _Event = false, float Speed = 0.005f)
    {
        //_Num의 큐브정보 복제
        if (_Slots[_Num].nodeColor == PuzzleSlot.NodeColor.Player ||
            _Slots[_OtherNum].nodeColor == PuzzleSlot.NodeColor.Player)
        {
            Debug.Log("런");
            Player.ChangeAnim("Run", true);
        }

        Vector2 Vec = _Slots[_Num].cube.transform.position;
        GameObject Cube = _Slots[_Num].cube.gameObject;
        PuzzleSlot.NodeColor nodeColor = _Slots[_Num].nodeColor;

        //_Num의 정보를 _OtherNum의 정보로 덮어쓰기
        _Slots[_Num].cube.MoveCube(_Slots[_OtherNum].transform.position, false, Speed);

        _Slots[_Num].cube = _Slots[_OtherNum].cube;
        _Slots[_Num].nodeColor = _Slots[_OtherNum].nodeColor;

        //_OtherNum을 복제정보로 덮어쓰기
        _Slots[_OtherNum].cube.MoveCube(_Slots[_Num].transform.position, _Event, Speed);
        _Slots[_OtherNum].cube = Cube.GetComponent<Cube>();
        _Slots[_OtherNum].nodeColor = nodeColor;

        _Slots[_Num].cube.Num = _Slots[_Num].SlotNum;
        _Slots[_OtherNum].cube.Num = _Slots[_OtherNum].SlotNum;

        
    }

    public void ChangeGameMode()
    {
        if (gameType == GameMode.Battle)
        {
            IllustSlot.transform.position = BattlePos.transform.position;
            MoveUI.SetActive(false);
            BattleUI.SetActive(true);
        }
        else if (gameType == GameMode.MoveMap)
        {
            IllustSlot.transform.position = MovePos.transform.position;
            MoveUI.SetActive(true);
            BattleUI.SetActive(false);
        }
    }



    public void BT_FillBlank(PuzzleSlot[] _Slot,int _Horizontal)
    {
        state = State.FillBlank;
        bool FirstEvent = true;
        float Speed = 0.01f;
        bool PlayerMove = false; // 플레이어 큐브가 움직였는지 확인 true면 움직임
        if (direction == Direction.Down)
        {
            for (int i = 0; i < TopRight - TopLeft; i++)
            {
                for (int Num = BottomLeft -_Horizontal; Num > TopRight; Num -= _Horizontal)
                {
                    if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Slot[Num + i].cube.gameObject.SetActive(false);
                        
                        if (_Slot[Num + i - _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                                _Slot[Num + i - _Horizontal].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Slot, Num + i, Num + i - _Horizontal, FirstEvent, Speed);
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Slot[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
                            SetCube(NewCube, MoveSlots[Num + i]);
                            NewCube.transform.position = MoveSlots[Num + i - _Horizontal].transform.position;
                            MoveSlots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(MoveSlots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }

        }
        else if (direction == Direction.Up)
        {
            for (int i = 0; i < TopRight - TopLeft; i++)
            {
                for (int Num = TopLeft + _Horizontal; Num < BottomLeft; Num += _Horizontal)
                {
                    if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Slot[Num + i].cube.gameObject.SetActive(false);

                        if (_Slot[Num + i + _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                               _Slot[Num + i + _Horizontal].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Slot, Num + i, Num + i + _Horizontal, FirstEvent, Speed);
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Slot[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Slot[Num + i]);
                            NewCube.transform.position = _Slot[Num + i + _Horizontal].transform.position;
                            _Slot[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Slot[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (direction == Direction.Left)
        {
            for (int i = TopLeft + _Horizontal; i < BottomLeft; i+=_Horizontal)
            {
                for (int Num = 0; Num <TopRight - TopLeft; Num++)
                {
                    if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Slot[Num + i].cube.gameObject.SetActive(false);

                        if (_Slot[Num + i +1].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                              _Slot[Num + i + 1].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Slot, Num + i, Num + i + 1, FirstEvent, Speed);
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Slot[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Slot[Num + i]);
                            NewCube.transform.position = _Slot[Num + i +1].transform.position;
                            _Slot[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Slot[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (direction == Direction.Right)
        {
            for (int i = TopRight + _Horizontal; i < BottomLeft; i += _Horizontal)
            {
                for (int Num = 0; Num > -(TopRight - TopLeft); Num--)
                {
                    if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Slot[Num + i].cube.gameObject.SetActive(false);

                        if (_Slot[Num + i - 1].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                              _Slot[Num + i - 1].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Slot, Num + i, Num + i - 1, FirstEvent, Speed);
                            if (_Slot[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Slot[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Slot[Num + i]);
                            NewCube.transform.position = _Slot[Num + i - 1].transform.position;
                            _Slot[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Slot[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }

        if (PlayerMove == false)
        {
            Debug.Log("정지");
            Player.ChangeAnim("Idle");
        }

        if (FirstEvent == true)
        {
            state = State.CheckMatch;
        }
    }


    public void DestroyCube(PuzzleSlot[] _slots)
    {


        state = State.DestroyCube;
        isMatched = false;
        bool Event = true;
        for (int i = 0; i < MoveSlots.Length; i++)
        {
            if (_slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _slots[i].nodeColor == PuzzleSlot.NodeColor.Blank)
            {
                _slots[i].cube.DestroyCube(Event);

                Event = false;
            }
        }
    }


    public void DeadlockCheck(PuzzleSlot[] _Slot, int _Horizontal, int _Vertical)
    {
        PuzzleSlot testPuzzle = new PuzzleSlot();

        for (int i = 0; i < _Horizontal * _Vertical; i++)
        {
            if (_Slot[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                // 상
                if (_Slot[i - _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = _Slot[i - _Horizontal].nodeColor;
                    _Slot[i - _Horizontal].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(false);

                    _Slot[i - _Horizontal].nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {
                        isMatched = false;
                        return;
                    }
                }
                // 하
                if (_Slot[i + _Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = _Slot[i + _Horizontal].nodeColor;
                    _Slot[i + _Horizontal].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(false);

                    _Slot[i + _Horizontal].nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {


                        isMatched = false;
                        return;
                    }
                }
                // 좌
                if (_Slot[i - 1].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = _Slot[i - 1].nodeColor;
                    _Slot[i - 1].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(false);

                    _Slot[i - 1].nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {


                        isMatched = false;
                        return;
                    }
                }
                // 우
                if (_Slot[i + 1].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = _Slot[i + 1].nodeColor;
                    _Slot[i + 1].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(false);

                    _Slot[i + 1].nodeColor = _Slot[i].nodeColor;
                    _Slot[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {


                        isMatched = false;
                        return;
                    }
                }

            }
        }
    }
    public void BT_ShowSlotText()
    {
        bool Active = !MoveSlots[0].GetComponentInChildren<Text>().enabled;


        if (Active == true)
            GetComponentInParent<Canvas>().sortingOrder = 5;
        else
            GetComponentInParent<Canvas>().sortingOrder = 0;

        for (int i = 0; i < Horizontal * Vertical; i++)
        {
            MoveSlots[i].GetComponentInChildren<Text>().enabled = Active;
        }

    }
    public void BT_ChangeDirection(int _Num)
    {
        direction = (Direction)_Num;
        for (int i = 0; i < 4; i++)
        {
            CameraButton[i].ButtonChange(_Num);
        }
        Player.ChangeDirection(direction);
    }
    public void BT_SetSlot()
    {
        SetSlot(MoveSlots,Horizontal,Vertical);

    }
    public void BT_Minimap()
    {
        MinimapBase.SetActive(!MinimapBase.activeSelf);
    }


}
