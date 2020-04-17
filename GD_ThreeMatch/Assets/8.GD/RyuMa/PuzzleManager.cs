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
        Battle,
        Null
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
    public GameMode gameMode = GameMode.MoveMap;
    public State state;


    public MapManager theMoveMap;
    public MapManager theBattleMap;



    public Sprite[] CubeSprites;
    public CameraButtonManager[] CameraButton;


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
    public PlayerCube Player;

    //DB


    private ObjectManager theObject;
    private FindMatches theMatch;
    private FadeManager theFade;
    private CameraManager theCamera;
    private BattleManager theBattle;
    private void Start()
    {
        theBattle = FindObjectOfType<BattleManager>();
        theFade = FindObjectOfType<FadeManager>();
        Player = FindObjectOfType<PlayerCube>();
        theMatch = FindObjectOfType<FindMatches>();
        theObject = FindObjectOfType<ObjectManager>();
        theCamera = FindObjectOfType<CameraManager>();

      

        findMatches = FindObjectOfType<FindMatches>();


    }



    private void Update()
    {
        if (gameMode == GameMode.MoveMap)
        {
            if (theFade.FadeEvent == true)
            {
                theFade.FadeEvent = false;
                gameMode = GameMode.Battle;
                SetSlot(theBattleMap, true);
                theBattle.SetBattle(1);
                ChangeGameMode();
            }


            if (state == State.ChangeMatch) //  큐브를 교환하는 상태
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;

                    //매치 조건이 맞는지 확인한다
                    findMatches.FindAllMatches(theMoveMap);
                    if (isMatched)
                    {
                        DestroyCube(theMoveMap);
                        return;
                    }


                    //매치가 안될경우
                    if (!isMatched)
                    {
                        ChangeCube(theMoveMap, SelectNum, OtherNum, true);
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
                    BT_FillBlank(theMoveMap);

                }
            }
            else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
            {
                findMatches.FindAllMatches(theMoveMap);
                if (isMatched)
                {
                    DestroyCube(theMoveMap);
                    return;
                }


                //매치가 안될경우
                if (!isMatched)
                {
                    Player.ChangeAnim("Idle", true);
                    state = State.Ready;
                }
            }
            else if (state == State.DestroyCube)//매치된 큐브 제거
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    BT_FillBlank(theMoveMap);
                }
            }
        }
        else if (gameMode == GameMode.Battle)
        {
            if (theFade.FadeEvent == true)
            {
                theFade.FadeEvent = false;
                gameMode = GameMode.MoveMap;
                ChangeGameMode();
            }

            if (state == State.ChangeMatch) //  큐브를 교환하는 상태
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;

                    //매치 조건이 맞는지 확인한다
                    findMatches.FindAllMatches(theBattleMap);
                    if (isMatched)
                    {
                        DestroyCube(theBattleMap);
                        return;
                    }


                    //매치가 안될경우
                    if (!isMatched)
                    {
                        ChangeCube(theBattleMap, SelectNum, OtherNum, true);
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
                    state = State.Ready;
                }
            }
            else if (state == State.FillBlank)// 빈칸을 채우는 상태
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    BT_FillBlank(theBattleMap);

                }
            }
            else if (state == State.CheckMatch)// 빈칸을 채운 후 매치 확인
            {
                findMatches.FindAllMatches(theBattleMap);
                if (isMatched)
                {
                    DestroyCube(theBattleMap);
                    return;
                }


                //매치가 안될경우
                if (!isMatched)
                {
                    
                    state = State.Ready;
                }
            }
            else if (state == State.DestroyCube)//매치된 큐브 제거
            {
                if (CubeEvent == true)
                {
                    CubeEvent = false;
                    BT_FillBlank(theBattleMap);
                }
            }


        }



    }




    //슬롯을 채운다. false일 경우 최초실행, true일 경우 현재 맵에서 리셋
    public void SetSlot(MapManager _Map, bool Reset = false)
    {

        for (int i = 0; i < _Map.Slots.Length; i++)
        {
            if (i <= _Map.TopRight ||
                i >= _Map.BottomLeft ||
                i % _Map.Horizontal <= 0 ||
                i % _Map.Horizontal >= _Map.Horizontal -1)
            {
                _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Null;
                _Map.Slots[i].nodeColor = PuzzleSlot.NodeColor.Null;
            }

            _Map.Slots[i].SlotNum = i;
        }





        for (int i = 0; i < _Map.Slots.Length; i++)
        {
            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                if (Reset == true)
                {
                    _Map.Slots[i].nodeType = PuzzleSlot.NodeType.Normal;
                    _Map.Slots[i].nodeColor = PuzzleSlot.NodeColor.Null;
                }
                else
                {
                    if(_Map.Slots[i].nodeColor != PuzzleSlot.NodeColor.Player)
                        _Map.Slots[i].nodeColor = PuzzleSlot.NodeColor.Null;
                }

                if (_Map.Slots[i].cube != null&& _Map.Slots[i].nodeColor != PuzzleSlot.NodeColor.Player)
                {
                    _Map.Slots[i].cube.Resetting();
                    _Map.Slots[i].cube = null;
                }
                _Map.Slots[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                _Map.Slots[i].TestText.text = i.ToString();
            }
            else
            {
                _Map.Slots[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                _Map.Slots[i].TestText.text = i.ToString();
                _Map.Slots[i].TestText.color = new Color(1, 1, 1);
            }
        }


        NotMatchSetCube(_Map);

        for (int i = 0; i < _Map.Slots.Length; i++)
        {
            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                _Map.Slots[i].TestText.text = i.ToString();
                _Map.Slots[i].cube.Num = i;
            }

        }


        if (Reset == false)
        {
            while (true)
            {
                int rand = Random.Range(0, _Map.Horizontal * _Map.Vertical);

                if (_Map.Slots[rand].nodeType != PuzzleSlot.NodeType.Null)
                {
                    _Map.Slots[rand].nodeColor = PuzzleSlot.NodeColor.Player;
                    _Map.Slots[rand].cube.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    Player.transform.position = _Map.Slots[rand].transform.position;

                    Transform Parent = _Map.Slots[rand].cube.transform;

                    Player.transform.parent = Parent;
                    Player.ChangeDirection(_Map.direction);

                    break;
                }
            }
            while (true)
            {
                int rand = Random.Range(0, _Map.Horizontal * _Map.Vertical);

                if (_Map.Slots[rand].nodeType != PuzzleSlot.NodeType.Null &&
                    _Map.Slots[rand].nodeColor != PuzzleSlot.NodeColor.Player)
                {
                    _Map.Slots[rand].nodeType = PuzzleSlot.NodeType.Enemy;

                    break;
                }
            }
            while (true)
            {
                int rand = Random.Range(0, _Map.Horizontal * _Map.Vertical);

                if (_Map.Slots[rand].nodeType != PuzzleSlot.NodeType.Null &&
                    _Map.Slots[rand].nodeColor != PuzzleSlot.NodeColor.Player &&
                    _Map.Slots[rand].nodeType != PuzzleSlot.NodeType.Enemy)
                {
                    _Map.Slots[rand].nodeType = PuzzleSlot.NodeType.Goal;
                    Goal.transform.position = _Map.Slots[rand].transform.position;
                    Transform Parent = _Map.Slots[rand].transform;
                    Goal.transform.parent = Parent;
                    break;
                }
            }
        }

        if (gameMode == GameMode.MoveMap)
        {
            //Vector2 vec = new Vector2(Player.transform.position.x, Player.transform.position.y + 0.5f);
            theCamera.SetBound(_Map, _Map.transform.position, true);
        }
        else if (gameMode == GameMode.Battle)
        {
            Vector2 CameraPos = new Vector2(
                theBattleMap.transform.position.x, theBattleMap.transform.position.y - 2);
            theCamera.SetBound(_Map, CameraPos, false);
        }

    }

    // 처음 매치가 안된 상태로 세팅
    public void NotMatchSetCube(MapManager _Map)
    {

        PuzzleSlot CopySlot = new PuzzleSlot();
        List<int> ColorList = new List<int>();
        ColorList.Add(0);
        ColorList.Add(1);
        ColorList.Add(2);
        ColorList.Add(3);
        ColorList.Add(4);
        ColorList.Add(5);
        for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.BottomRight - _Map.Horizontal; i++)
        {
            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor  == PuzzleSlot.NodeColor.Null)
            {
                List<int> RandomList = new List<int>(ColorList);
                if (_Map.Slots[i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {
                    if (_Map.Slots[i - _Map.Horizontal - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Map.Slots[i - _Map.Horizontal].nodeColor == _Map.Slots[i - _Map.Horizontal - _Map.Horizontal].nodeColor)
                        {
                            RandomList.Remove((int)_Map.Slots[i - _Map.Horizontal].nodeColor);
                        }
                        else
                        {
                            int rand = Random.Range(0, 2);
                            if (rand == 0)
                            {
                                RandomList.Remove((int)_Map.Slots[i - _Map.Horizontal].nodeColor);
                            }
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            RandomList.Remove((int)_Map.Slots[i - _Map.Horizontal].nodeColor);
                        }
                    }

                   
                }
                if (_Map.Slots[i - 1].nodeType != PuzzleSlot.NodeType.Null)
                {
                    if (_Map.Slots[i - 2].nodeType != PuzzleSlot.NodeType.Null)
                    {
                        if (_Map.Slots[i - 1].nodeColor == _Map.Slots[i - 2].nodeColor)
                        {
                            RandomList.Remove((int)_Map.Slots[i - 1].nodeColor);
                        }
                        else
                        {
                            int rand = Random.Range(0, 2);
                            if (rand == 0)
                            {
                                RandomList.Remove((int)_Map.Slots[i - 1].nodeColor);
                            }
                        }
                    }
                    else
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            RandomList.Remove((int)_Map.Slots[i - 1].nodeColor);
                        }
                    }
                    
                    
                }

                int RandColorNum = Random.Range(0, RandomList.Count);
                GameObject Cube = theObject.FindObj("Cube");
                Cube.GetComponent<SpriteRenderer>().sprite = CubeSprites[RandomList[RandColorNum]];
                Cube.GetComponent<Cube>().MinimapSprite.sprite = CubeSprites[RandomList[RandColorNum]];
                _Map.Slots[i].nodeColor = (PuzzleSlot.NodeColor)RandomList[RandColorNum];

                Cube.transform.position = _Map.Slots[i].transform.position;
                _Map.Slots[i].cube = Cube.GetComponent<Cube>();

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
    public void CheckMoveCube(MapManager _Map,Direction _Direction,int _Num)
    {

        int ChangeNum = 0;

        if (_Direction == Direction.Up) 
        {
            if (_Map.Slots[_Num - _Map.Horizontal].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - _Map.Horizontal;
        }
        else if (_Direction == Direction.Down)
        {
            if (_Map.Slots[_Num + _Map.Horizontal].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num + _Map.Horizontal;
        }
        else if (_Direction == Direction.Left)
        {
            if (_Map.Slots[_Num -1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num - 1;
        }
        else if (_Direction == Direction.Right)
        {
            if (_Map.Slots[_Num +1].nodeType == PuzzleSlot.NodeType.Null)
                return;
            else
                ChangeNum = _Num +1;
        }

        state = State.ChangeMatch;
        ChangeCube(_Map, _Num, ChangeNum,true);
        SelectNum = _Num;
        OtherNum = ChangeNum;
    }


    public void ChangeCube(MapManager _Map,int _Num, int _OtherNum,bool _Event = false, float Speed = 0.005f)
    {
        //_Num의 큐브정보 복제
        if (_Map.Slots[_Num].nodeColor == PuzzleSlot.NodeColor.Player ||
            _Map.Slots[_OtherNum].nodeColor == PuzzleSlot.NodeColor.Player)
        {
            if(gameMode == GameMode.MoveMap)
                Player.ChangeAnim("Run", true);
        }

        Vector2 Vec = _Map.Slots[_Num].cube.transform.position;
        GameObject Cube = _Map.Slots[_Num].cube.gameObject;
        PuzzleSlot.NodeColor nodeColor = _Map.Slots[_Num].nodeColor;

        //_Num의 정보를 _OtherNum의 정보로 덮어쓰기
        _Map.Slots[_Num].cube.MoveCube(_Map.Slots[_OtherNum].transform.position, false, Speed);

        _Map.Slots[_Num].cube = _Map.Slots[_OtherNum].cube;
        _Map.Slots[_Num].nodeColor = _Map.Slots[_OtherNum].nodeColor;

        //_OtherNum을 복제정보로 덮어쓰기
        _Map.Slots[_OtherNum].cube.MoveCube(_Map.Slots[_Num].transform.position, _Event, Speed);
        _Map.Slots[_OtherNum].cube = Cube.GetComponent<Cube>();
        _Map.Slots[_OtherNum].nodeColor = nodeColor;

        _Map.Slots[_Num].cube.Num = _Map.Slots[_Num].SlotNum;
        _Map.Slots[_OtherNum].cube.Num = _Map.Slots[_OtherNum].SlotNum;

        
    }

    public void ChangeGameMode()
    {
        if (gameMode == GameMode.Battle)
        {
            IllustSlot.transform.position = BattlePos.transform.position;
            //theCamera.SetBound(theBattleMap.Bound, theBattleMap.Bound.transform.position,false);
            MoveUI.SetActive(false);
            BattleUI.SetActive(true);

        }
        else if (gameMode == GameMode.MoveMap)
        {
            IllustSlot.transform.position = MovePos.transform.position;
            Vector2 vec = new Vector2(Player.transform.position.x, Player.transform.position.y + 0.5f);
            theCamera.SetBound(theMoveMap, vec, true);
            MoveUI.SetActive(true);
            BattleUI.SetActive(false);
        }
    }



    public void BT_FillBlank(MapManager _Map)
    {

        state = State.FillBlank;
        bool FirstEvent = true;
        float Speed = 0.01f;
        bool PlayerMove = false; // 플레이어 큐브가 움직였는지 확인 true면 움직임
        if (_Map.direction == Direction.Down)
        {
            for (int i = 0; i < _Map.TopRight - _Map.TopLeft; i++)
            {
                for (int Num = _Map.BottomLeft - _Map.Horizontal; Num > _Map.TopRight; Num -= _Map.Horizontal)
                {
                    if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Map.Slots[Num + i].cube.gameObject.SetActive(false);
                        
                        if (_Map.Slots[Num + i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                                _Map.Slots[Num + i - _Map.Horizontal].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Map, Num + i, Num + i - _Map.Horizontal, FirstEvent, Speed);
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
                            SetCube(NewCube, _Map.Slots[Num + i]);
                            NewCube.transform.position = _Map.Slots[Num + i - _Map.Horizontal].transform.position;
                            _Map.Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Map.Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }

        }
        else if (_Map.direction == Direction.Up)
        {
            for (int i = 0; i < _Map.TopRight - _Map.TopLeft; i++)
            {
                for (int Num = _Map.TopLeft + _Map.Horizontal; Num < _Map.BottomRight; Num += _Map.Horizontal)
                {
                    if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Map.Slots[Num + i].cube.gameObject.SetActive(false);

                        if (_Map.Slots[Num + i + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                               _Map.Slots[Num + i + _Map.Horizontal].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Map, Num + i, Num + i + _Map.Horizontal, FirstEvent, Speed);
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Map.Slots[Num + i]);
                            NewCube.transform.position = _Map.Slots[Num + i + _Map.Horizontal].transform.position;
                            _Map.Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Map.Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (_Map.direction == Direction.Left)
        {
            for (int i = _Map.TopLeft + _Map.Horizontal; i < _Map.BottomLeft; i+=_Map.Horizontal)
            {
                for (int Num = 0; Num < _Map.TopRight - _Map.TopLeft; Num++)
                {
                    if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Map.Slots[Num + i].cube.gameObject.SetActive(false);

                        if (_Map.Slots[Num + i +1].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                              _Map.Slots[Num + i + 1].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Map, Num + i, Num + i + 1, FirstEvent, Speed);
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Map.Slots[Num + i]);
                            NewCube.transform.position = _Map.Slots[Num + i +1].transform.position;
                            _Map.Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Map.Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }
        else if (_Map.direction == Direction.Right)
        {
            for (int i = _Map.TopRight + _Map.Horizontal; i < _Map.BottomLeft; i += _Map.Horizontal)
            {
                for (int Num = 0; Num > -(_Map.TopRight - _Map.TopLeft); Num--)
                {
                    if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                    {

                        _Map.Slots[Num + i].cube.gameObject.SetActive(false);

                        if (_Map.Slots[Num + i - 1].nodeType != PuzzleSlot.NodeType.Null)
                        {
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Player ||
                              _Map.Slots[Num + i - 1].nodeColor == PuzzleSlot.NodeColor.Player)
                            {
                                PlayerMove = true;
                            }
                            ChangeCube(_Map, Num + i, Num + i - 1, FirstEvent, Speed);
                            if (_Map.Slots[Num + i].nodeColor == PuzzleSlot.NodeColor.Blank)
                                _Map.Slots[Num + i].cube.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

                            FirstEvent = false;
                        }
                        else
                        {
                            GameObject NewCube = theObject.FindObj("Cube");
                            NewCube.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            SetCube(NewCube, _Map.Slots[Num + i]);
                            NewCube.transform.position = _Map.Slots[Num + i - 1].transform.position;
                            _Map.Slots[Num + i].cube = NewCube.GetComponent<Cube>();
                            NewCube.GetComponent<Cube>().MoveCube(_Map.Slots[Num + i].transform.position, FirstEvent, Speed);
                            FirstEvent = false;
                        }
                    }
                }

            }
        }

        if (PlayerMove == false)
        {
            if(gameMode == GameMode.MoveMap)
                Player.ChangeAnim("Idle");
        }

        if (FirstEvent == true)
        {
            state = State.CheckMatch;
        }
    }


    public void DestroyCube(MapManager _Map)
    {

        state = State.DestroyCube;
        isMatched = false;
        bool Event = true;
        for (int i = 0; i < _Map.Slots.Length; i++)
        {
            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null &&
                _Map.Slots[i].nodeColor == PuzzleSlot.NodeColor.Blank)
            {
                _Map.Slots[i].cube.DestroyCube(Event);

                Event = false;
            }
        }
    }





    public void DeadlockCheck(MapManager _Map)
    {

        PuzzleSlot testPuzzle = new PuzzleSlot();

        for (int i = 0; i < _Map.Horizontal * _Map.Vertical; i++)
        {
            if (_Map.Slots[i].nodeType != PuzzleSlot.NodeType.Null)
            {
                // 상
                if (_Map.Slots[i - _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = _Map.Slots[i - _Map.Horizontal].nodeColor;
                    _Map.Slots[i - _Map.Horizontal].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(theMoveMap, false);

                    _Map.Slots[i - _Map.Horizontal].nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {
                        isMatched = false;
                        return;
                    }
                }
                // 하
                if (_Map.Slots[i + _Map.Horizontal].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = _Map.Slots[i + _Map.Horizontal].nodeColor;
                    _Map.Slots[i + _Map.Horizontal].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(theMoveMap,false);

                    _Map.Slots[i + _Map.Horizontal].nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {


                        isMatched = false;
                        return;
                    }
                }
                // 좌
                if (_Map.Slots[i - 1].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = _Map.Slots[i - 1].nodeColor;
                    _Map.Slots[i - 1].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(_Map,false);

                    _Map.Slots[i - 1].nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = testPuzzle.nodeColor;

                    if (isMatched)
                    {


                        isMatched = false;
                        return;
                    }
                }
                // 우
                if (_Map.Slots[i + 1].nodeType != PuzzleSlot.NodeType.Null)
                {

                    testPuzzle.nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = _Map.Slots[i + 1].nodeColor;
                    _Map.Slots[i + 1].nodeColor = testPuzzle.nodeColor;

                    theMatch.FindAllMatches(_Map,false);

                    _Map.Slots[i + 1].nodeColor = _Map.Slots[i].nodeColor;
                    _Map.Slots[i].nodeColor = testPuzzle.nodeColor;

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
        bool Active = !theMoveMap.Slots[0].GetComponentInChildren<Text>().enabled;


        for (int i = 0; i < theMoveMap.Horizontal * theMoveMap.Vertical; i++)
        {
            theMoveMap.Slots[i].GetComponentInChildren<Text>().enabled = Active;
        }

    }
    public void BT_ChangeDirection(int _Num)
    {
        if (state == State.Ready)
        {
            theMoveMap.direction = (Direction)_Num;
            for (int i = 0; i < 4; i++)
            {
                CameraButton[i].ButtonChange(_Num);
            }
            Player.ChangeDirection(theMoveMap.direction);
        }
    }
    public void BT_SetSlot()
    {
        SetSlot(theMoveMap);

    }
    public void BT_Minimap()
    {
        MinimapBase.SetActive(!MinimapBase.activeSelf);
    }


}
