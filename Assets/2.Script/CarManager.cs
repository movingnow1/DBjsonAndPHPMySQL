using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;//파일로 저장
using Newtonsoft.Json;
using UnityEngine.UI;



[System.Serializable] //직렬화 - 순서대로 한줄로 나열, 안의 변수를 인스펙터 창에 다 보이게 할려면
public class Items
{
    //생성자 
    //클래스와 같은 이름을 사용한다. 메소드처럼 매개변수를 가질 수 있으나, 메소드와 달리 반환x
    //원래는 컴파일러가 직접 만들어준다. 우리가 생성 안해도 만들어줌
    public Items(string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; }

    public string Type, Name, Explain, Number; //json으로 파싱할때 number는 int형식은 잘안됨
    public bool isUsing;
}

public class CarManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Items> AllItemList;  //클래스를 list로 다 가져오면 직렬화한것들이 묶음으로...
                                     //List<변수/클래스 가능>

    public List<Items> MyItemList; //내가 가지고 있는 아이템 

    //자식 설명, 개수
    public GameObject[] Slot,  UsingImage;//이미지활성화 여부버튼

    //탭 클릭 시 슬롯 흰 배경 이미지
    public Image[] TabImage, ItemImage;

    //탭 누를때와 안누를때의 sprite
    public Sprite TabIdleSprite, TabSelectSprite;

    public Sprite[] ItemSprite; //실제 아이템 이미지

    public GameObject ExplainPanel;
    public RectTransform CanvasRect; 
    public Vector2 v;

    void Start()
    {
        //엑셀 utf8메모장으로 가져온 데이터가 뛰어쓰기 되어있다면
        //text.length -1 로 해야함
        //string.split로 구분기호(엔터 = 줄바꿈 \n)로 나눈 값을 배열로 저장
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length).Split('\n');

        //줄로 나눈 line을 for돌면서 Tab(\t)으로 나눔
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t'); //문자 배열로 담음

            AllItemList.Add(new Items(row[0], row[1], row[2], row[3], row[4] == "TRUE")); //생성자를 안하면, Add치고 빨간줄, 매개변수Item에 해당되는 제공 인수가 없음으로 나옴
        }

        //Save(); //json으로 string 직렬화값으로 변경하여 저장함

        Load(); //MyItemList에 string직렬화값을 list로 남아서 가져옴
                //내가 가진 아이템
    }

    private void Update()
    {
         //설명창 뜨는 위치 
        //그래서 ScreenPointToLocalPointInRectangle 새로운 사각형(캔버스) 위치로 변환 >Update로 이동
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = anchoredPos + v;
    }


    //slot마다 넘버 부여=slotNum
    public void SlotClick(int slotNum)
    {
        print(CurItemList[slotNum].Name);
        Items CurItem = CurItemList[slotNum];
        Items UsingItem = CurItemList.Find(x => x.isUsing == true);

        if (curType == "Car") //1. car탭 선택한 상황에서   2.SlotClick()=어떤슬롯을 선택 시,
        {
            if (UsingItem != null) UsingItem.isUsing = false;
            CurItem.isUsing = true; //현재 아이템을 isUsing을 true로 시키고
        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing; //true를 false로변경
            if (UsingItem != null) UsingItem.isUsing = false;

        }
        Save();
    }




    /// <summary>-----------------------------------------------
    /// 아이템메뉴 상단 탭에 따라 아이템 다르게 나오게 만들기
    /// </summary>
    /// 1. 버튼에 string tabName에 Car와 Particle로 지정해둠.
    public string curType = "Car";//초반부터 바로 Car가 보이게
                                  //인스펙터가 비어있다면 그 값을 참조하므로 
                                  //직접 넣어줘야 함
    public List<Items> CurItemList;


    public void TabClick(string tabName)
    {
        curType = tabName;

        CurItemList = MyItemList.FindAll(x => x.Type == tabName);
        //FindAll은 안의 조건이 만족하는 값을 모두 가져온다.
        //MyItemList[0].Type 대신 사용할 수 있는 법.


        for (int i = 0; i < Slot.Length; i++)
        {
            //슬롯과 텍스트 보이게 하기
            bool isExist = i < CurItemList.Count; //불값 = 조건식도 가능...
            Slot[i].SetActive(isExist);
            //탭에 따른 CurItemList type값(예:3) 보다 작을때만 슬롯 text 개수가 0,1,2로 나오게

            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name /*+ "/" + CurItemList[i].isUsing */ : ""; //작다면 true,이름쓰고 / 크다면 빈공간


            if (isExist)
            {
                 //제이슨 메모장 이름 순서와 배열 이미지 순서가 같아야 한다.

                 //배열 인덱스로 비교 name 중에 0번째인가 1번째인가 찾아 비교해줌
                 //빈 슬롯 이미지 배열  =   배열로 담은 아이템이미지 [줄,탭으로 구분한 정보가 모두 담긴 list
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
               
                //클릭 할 시 체크 박스 보이게 
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        
        
        }

        //탭 선택 시 이미지 변경
        int tabNum = 0;
        switch (tabName) //탭 마다 지정된 sting값 마다 int tabNum를 지정
        {
            case "Car": tabNum = 0; break;
            case "Particle": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            // 탭을 선택한 상황이니까, i는 Switch에서 지정한 tabNum로 정하고, 조건문.
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }

    }

    
    IEnumerator PointerCoroutine;
    //이미지 슬롯 위에 설명 ui 나오게 > slot오브젝트에 Event Trigger 컴포넌트 추가함
    public void PointerEnter(int sloutNum)
    {
        PointerCoroutine = PointerEnterDelay(sloutNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[sloutNum].Name; //CurItemList배열 아닌 매개변수 슬롯int값을 받아서 
                                                                                       // 위의 CurItemList[index]번호와 비교가 되는가봄

      
       ExplainPanel.transform.GetChild(2).GetComponentInChildren<Image>().sprite = Slot[sloutNum].transform.GetChild(1).GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[sloutNum].Number + "개";
        ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[sloutNum].Explain;


        //패널 창 위치 > Update함수로 이동됨
        //마우스 위치로 설명 창이 위치되도록  //input.mousePosition은 0,0부터 1920,1080까지 감
        // ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;

        //그래서 ScreenPointToLocalPointInRectangle 새로운 사각형(캔버스) 위치로 변환 >Update로 이동

    }
    //마우스가 0.5 후에 바로 다른 곳으로 이동하면,
    //나타나고나서는 안사라지게 된다.
    //0.5초 전에는 false로 하고(Eixt함수), 그 이후 true되는데
    //이미 fasle Eixt함수가 작동되므로, ture가 되면 다시 꺼질 수 없게 됨
    IEnumerator PointerEnterDelay(int sloutNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }


    public void PointerExit(int sloutNum)
    {
        StopCoroutine(PointerCoroutine); //그래서 0.5초 후 발동시키려면, 
                                                    // Exit함수가 아닌 저장해둔 Dalay를 멈추고
                                                    // StopCoroutine만 쓰면 Delay함수를 찾지 못하므로, 
                                                    // IEnumerator PointerCoroutine;를 위에 하나 만들어 놓고, Dalay함수를 저장해둠
        ExplainPanel.SetActive(false);
    }



    void Save()
    {
        //SerializeObject도 직렬화 시켜주는 것임
        //서버 플레이팹에 저장하고 싶다면, jdata를 스트링 형식으로 넘기면 된다함
        // json으로 string형식으로 변환 후, 서버나 로컬(내컴퓨터)에 저장가능
        //json은 저장을 의미하는 것은 아님, string으로 변환하는 과정까지임
        string jdata = JsonConvert.SerializeObject(MyItemList);
        print(jdata);


        //Json이 mysql,xml 보다 쉽고 보편적
        //Application.dataPath - 모바일이면 모바일, pc면 pc좌표
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);


        //시작 하자마자 반영된 아이템이 업데이트 되게
        TabClick(curType);


    }

    //직렬화(한줄) 만든 제이슨 string값을 역직렬화 하여 MyItemList에 가져오기
    //string에서 list로 변환(역직렬화)
    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Items>>(jdata);

        //시작 하자마자 바로 Car 탭에서 적절한 아이템이 잘 보이도록
        TabClick(curType);
    }
}
