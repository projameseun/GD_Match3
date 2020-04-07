using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;  //Newtonsoft제이슨 파싱방법
using UnityEngine.UI;
using System.IO;    //Json파일로 파일
class Person
{
    public string name;
    public int age;

    public Person(string name, int age)
    {
        this.name = name;
        this.age = age;
    }
}


//JObject j = new JObject();

//j["age"] = human.age;
//j["name"] = human.name;
//j["height"] = human.height;
public class StlManager : MonoBehaviour
{
    //Person m_Person = new Person("철수", 15);
    //List<Person> data = new List<Person>();
    //Dictionary<string, Person> data = new Dictionary<string, Person>();

    //Dictionary<string, int> data = new Dictionary<string, int>();

    public Text m_txt;
    List<Person> data = new List<Person>();

    private void Start()
    {   //직렬화 제이슨으로 변경하는법

        //data.Add(new Person("철수", 22));
        //data.Add(new Person("영희", 30));
        //data.Add(new Person("진구", 15));

        //딕션어리 클래스 Json방법
        //data["data1"] = new Person("철수", 22);
        //data["data2"] = new Person("영희", 32);
        //data["data3"] = new Person("진구", 42);

        //딕션어리 int Json방법
        //data["철수"] = 15;
        //data["영희"] = 20;
        //data["진구"] = 30;

        //string jdata = JsonConvert.SerializeObject(data);
        //print(jdata);

        //m_txt = null;

        data.Add(new Person("철수", 15));
        data.Add(new Person("영희", 30));
        data.Add(new Person("진구", 49));
    }

    public void JsonSave()
    {
        //Json으로 변경방법
        string jdata = JsonConvert.SerializeObject(data);
        //암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);

        //새 파일을 만들고 파일에 내용을 쓴 다음 파일을 닫습니다.
        //    대상 파일이 이미 있으면 덮어씁니다.
        //1.경로-> 이름.json
        //2.데이터 
        //File.WriteAllText(Application.dataPath + "/James.json", jdata);
        File.WriteAllText(Application.dataPath + "/8.GD/James/James.json", format);

        Debug.Log("세이브테스트");
    }

    public void JsonLoad()
    {
        
            string jdata = File.ReadAllText(Application.dataPath + "/8.GD/James/James.json");
            
        //암호화 불러오기
        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes);


        //m_txt.text = jdata;
        ////Json을 다시 월래대로 list식ㅇ로
        //data = JsonConvert.DeserializeObject<List<Person>>(jdata);

        // print(data[0].name);

        m_txt.text = reformat;
        data = JsonConvert.DeserializeObject<List<Person>>(reformat);
    }
}
