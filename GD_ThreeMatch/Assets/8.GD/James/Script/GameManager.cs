using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Newtonsoft.Json;
using UnityEngine.UI;
//직렬화 Inspector창에 보이게됨 (public만)
[System.Serializable]
public class Item
{
    public string Type, Name, Explain, Number,Index;
    //Number를 string으로 한이유는
    //나중에 JSon으로 파싱하기때문에 
    public bool is_Using;

    public Item(string type, string name, string explain, string number, bool is_Using, string _Index)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Number = number;
        this.is_Using = is_Using;
        Index = _Index;
    }
}

public class GameManager : MonoBehaviour
{
    //텍스트파일을 에셋에 나타나게 해줌
    public TextAsset IteamDatabase;

    //public Item My_Item;
    public List<Item> m_ItemList,My_ItemList,CurItemList;

    //탭에클릭햇을대 어떤게 담아져잇는지확인하는 변수
    //public string curType;
    public string curType = "Character";

    //슬롯추가
    public GameObject[] Slot,UsingImage;

    //이미지추가
    public Image[] TabImage,ItemImage;
    public Sprite TabIdleSprite, TabSelectSprite;
    public Sprite[] ItemSprite;

    public GameObject ExplainPanel;

    public RectTransform CanvasRect;    //변환위치

    public InputField ItemNameInput, ItemNumberInput;

    IEnumerator PointerCoroutine;

    RectTransform ExplainRect;

    //JsonUtilty
    string filePath;

    //Text.text.substring 기능
    //문자열을 컨트롤
    //text.substirng(시작위치,종료위치)
    //text.substirng(시작위치); //시작위치부터끝까지나옴 
    //문자열의 시작위치는 0 부터 시작 

    //Split 문자열자르기
    //string str "ID:test,PW:1234:
    //stirng[]sp = str.Split(',');  //이기준으로짤림
    //foreach(string s in sp)
    //{
    //  Console.WriteLine(s);
    //}

    void Start()
    {
        //print(IteamDatabase);
        string[] Line = IteamDatabase.text.Substring(0, 
                    IteamDatabase.text.Length - 1).Split('\n');
        //-1을하는이유는 엑셀에서 엔터기능이들어가기 엔터를지우는작업

        //print(Line.Length);
        for(int i=0; i<Line.Length; i++)
        {
            //탭으로 나눔
            string[] row = Line[i].Split('\t');

            //여기서 아이템형식을 넣어야되서 생성자함수를이용함
            m_ItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE",row[5]));
        }

        //jsonutilty
        filePath = Application.persistentDataPath + "/MyItem2.txt";
        //print(filePath);
        Load();

        ExplainRect = ExplainPanel.GetComponent<RectTransform>();   //캐싱
    }
    void Update()
    {
        //업데이트에서 을 쓰면 사양을 많이먹는다 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        //ExplainPanel.GetComponent<RectTransform>().anchoredPosition = anchoredPos + new Vector2(-180,-165); 사양많이먹음
        ExplainRect.anchoredPosition = anchoredPos + new Vector2(-180, -165);
    }

    //필드
    public void GetItemClick()
    {
        Item curItem = My_ItemList.Find(x => x.Name == ItemNameInput.text);
        ItemNumberInput.text = ItemNumberInput.text == "" ? "1" : ItemNumberInput.text; 
        if (curItem != null)
        {
            curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
        }
        else
        {
            //전체에서 얻을 아이템을 찾아 내 아이템추가
            Item curAllItem = m_ItemList.Find(x => x.Name == ItemNameInput.text);
            if (curAllItem != null)
            {
                curAllItem.Number = ItemNumberInput.text;       //이래야 원하는값이 들어감
                My_ItemList.Add(curAllItem);
            }
        }

        My_ItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));

        Save();
        //print(curItem.Name);
    }

    public void RemoveItemClick()
    {
        Item curItem = My_ItemList.Find(x => x.Name == ItemNameInput.text);
        if(curItem != null)
        {
            int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text == "" ? "1" : ItemNumberInput.text);

            if (curNumber <= 0) My_ItemList.Remove(curItem);
            else curItem.Number = curNumber.ToString();
         }
        My_ItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
        Save();
    }

    public void ResetItemClick()
    {
        Item BasicItem = m_ItemList.Find(x => x.Name == "Pig");
        BasicItem.is_Using = true;
        My_ItemList = new List<Item>() { BasicItem };
        Save();
        //JsonUtilty
        Load();
    }

    public void SlotClick(int slotNum)
    {
        //슬롯 이미지 버튼추가
        //print(CurItemList[slotNum].Name);
        Item CurItem = CurItemList[slotNum];
        //Find는 하나 Findall전체
        Item UsingItem = CurItemList.Find(x => x.is_Using == true);

        if(curType == "Character")
        {
            if (UsingItem != null) UsingItem.is_Using = false;
            CurItem.is_Using = true;
        }
        else
        {
            CurItem.is_Using = !CurItem.is_Using;
            if (UsingItem != null) UsingItem.is_Using = false;
        }

        Save();
    }

    //Tabclick
    public void TabClick(string TabName)
    {
        //현재 아이템 리스트에 클릭한 타입만추가 
        curType = TabName; //확인
        CurItemList = My_ItemList.FindAll(x => x.Type == TabName);

        //슬롯과 텍스트보이기
        for(int i=0; i<Slot.Length; i++)
        {
            bool isExist = i < CurItemList.Count;
            //  Slot[i].SetActive(i < CurItemList.Count);
            Slot[i].SetActive(isExist);
            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name : "";//"/" + CurItemList[i].is_Using

            //아이템 이미지와 사용중인지 아닌지 
            if (isExist)
            {
                ItemImage[i].sprite = ItemSprite[m_ItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                UsingImage[i].SetActive(CurItemList[i].is_Using);
            }
        }

        //탭이미지
        int TabNum = 0;
        switch (TabName)
        {
            case "Character": TabNum = 0;
                //Debug.Log("캐릭터");
                break;
            case "Balloon": TabNum = 1;
                break;
        }
        for(int i=0; i<TabImage.Length; i++)
        {
            TabImage[i].sprite = i == TabNum ? TabSelectSprite : TabIdleSprite;
           //Debug.Log(TabIdleSprite);
        }
        
    }

    public void PointEnter(int slotNum)
    {
        //슬롯에 마우스를 올리면 0.5초뒤에 설명창띄움
        PointerCoroutine = PointerEnterDelay(slotNum);
        //StartCoroutine(PointerEnterDelay(slotNum));
        StartCoroutine(PointerCoroutine);
        //마우스위치에 나온다 여기까지하게되면 이상위치에 나오는데  해상도의 반이 캔버스이기 ㄸ문이다  Input.MousPosition은 1920 1080기준으로나와서 변환시켜야됨
        //ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;

        ////변환 out은 변환후위치
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        //ExplainPanel.GetComponent<RectTransform>().anchoredPosition = anchoredPos;

        //텍스트추가
        ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[slotNum].Name;
        //이미지추가 자식의 자식은 안된다 기억
        ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = Slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
        //갯수추가
        ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[slotNum].Number + "개";
        //설명추가
        ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[slotNum].Explain;

    }

    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
        //print(slotNum + "슬롯 들어옴");
    }

    public void PointExit(int slotNum)
    {
        //print(slotNum + "슬롯 나감");
        //StopCoroutine(PointerEnterDelay(slotNum));
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }

    void Save()
    {
        //print(Application.dataPath);
        // string jdata = JsonConvert.SerializeObject(m_ItemList); //스트링형식 으로저장
        // string jdata = JsonConvert.SerializeObject(My_ItemList); 
        //Json 은저장을 의미하는게아니라 string으로 변환하는 과정을 말한다  
        // print(jdata);
        // File.WriteAllText(Application.dataPath + "/8.GD/James/MyItem.txt", "hi\ntest");
        //File.WriteAllText(Application.dataPath + "/8.GD/James/MyItem.txt", jdata);



        //JsonUtilty방법
        //datapath는 에셋에서만참조한다 
        File.WriteAllText(filePath,"HI");

        TabClick(curType);
    }

    void Load()
    {
        // string jdata = File.ReadAllText(Application.dataPath + "/8.GD/James/MyItem.txt");
        //My_ItemList = JsonConvert.DeserializeObject<List<Item>> (jdata);


        //json utitlty
        if(!File.Exists(filePath))
        {
            ResetItemClick();
            return;
        }
        string jdata = File.ReadAllText(filePath);

        //나중확인
        TabClick(curType);

       
    }

}
