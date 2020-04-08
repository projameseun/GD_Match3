using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//직렬화 Inspector창에 보이게됨 (public만)
[System.Serializable]
public class Item
{
    public string Type, Name, Explain, Number;
    //Number를 string으로 한이유는
    //나중에 JSon으로 파싱하기때문에 
    public bool is_Using;

    public Item(string type, string name, string explain, string number, bool is_Using)
    {
        Type = type;
        Name = name;
        Explain = explain;
        Number = number;
        this.is_Using = is_Using;
    }
}

public class GameManager : MonoBehaviour
{
    //텍스트파일을 에셋에 나타나게 해줌
    public TextAsset IteamDatabase;

    //public Item My_Item;
    public List<Item> m_ItemList,My_ItemList;

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
            m_ItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        Save();
    }

    void Save()
    {
        //print(Application.dataPath);
        File.WriteAllText(Application.dataPath + "/8.GD/James/MyItem.txt", "hi\ntest");
    }

}
