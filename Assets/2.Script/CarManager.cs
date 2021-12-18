using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;//���Ϸ� ����
using Newtonsoft.Json;
using UnityEngine.UI;



[System.Serializable] //����ȭ - ������� ���ٷ� ����, ���� ������ �ν����� â�� �� ���̰� �ҷ���
public class Items
{
    //������ 
    //Ŭ������ ���� �̸��� ����Ѵ�. �޼ҵ�ó�� �Ű������� ���� �� ������, �޼ҵ�� �޸� ��ȯx
    //������ �����Ϸ��� ���� ������ش�. �츮�� ���� ���ص� �������
    public Items(string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; }

    public string Type, Name, Explain, Number; //json���� �Ľ��Ҷ� number�� int������ �߾ȵ�
    public bool isUsing;
}

public class CarManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Items> AllItemList;  //Ŭ������ list�� �� �������� ����ȭ�Ѱ͵��� ��������...
                                     //List<����/Ŭ���� ����>

    public List<Items> MyItemList; //���� ������ �ִ� ������ 

    //�ڽ� ����, ����
    public GameObject[] Slot,  UsingImage;//�̹���Ȱ��ȭ ���ι�ư

    //�� Ŭ�� �� ���� �� ��� �̹���
    public Image[] TabImage, ItemImage;

    //�� �������� �ȴ������� sprite
    public Sprite TabIdleSprite, TabSelectSprite;

    public Sprite[] ItemSprite; //���� ������ �̹���

    public GameObject ExplainPanel;
    public RectTransform CanvasRect; 
    public Vector2 v;

    void Start()
    {
        //���� utf8�޸������� ������ �����Ͱ� �پ�� �Ǿ��ִٸ�
        //text.length -1 �� �ؾ���
        //string.split�� ���б�ȣ(���� = �ٹٲ� \n)�� ���� ���� �迭�� ����
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length).Split('\n');

        //�ٷ� ���� line�� for���鼭 Tab(\t)���� ����
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t'); //���� �迭�� ����

            AllItemList.Add(new Items(row[0], row[1], row[2], row[3], row[4] == "TRUE")); //�����ڸ� ���ϸ�, Addġ�� ������, �Ű�����Item�� �ش�Ǵ� ���� �μ��� �������� ����
        }

        //Save(); //json���� string ����ȭ������ �����Ͽ� ������

        Load(); //MyItemList�� string����ȭ���� list�� ���Ƽ� ������
                //���� ���� ������
    }

    private void Update()
    {
         //����â �ߴ� ��ġ 
        //�׷��� ScreenPointToLocalPointInRectangle ���ο� �簢��(ĵ����) ��ġ�� ��ȯ >Update�� �̵�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = anchoredPos + v;
    }


    //slot���� �ѹ� �ο�=slotNum
    public void SlotClick(int slotNum)
    {
        print(CurItemList[slotNum].Name);
        Items CurItem = CurItemList[slotNum];
        Items UsingItem = CurItemList.Find(x => x.isUsing == true);

        if (curType == "Car") //1. car�� ������ ��Ȳ����   2.SlotClick()=������� ���� ��,
        {
            if (UsingItem != null) UsingItem.isUsing = false;
            CurItem.isUsing = true; //���� �������� isUsing�� true�� ��Ű��
        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing; //true�� false�κ���
            if (UsingItem != null) UsingItem.isUsing = false;

        }
        Save();
    }




    /// <summary>-----------------------------------------------
    /// �����۸޴� ��� �ǿ� ���� ������ �ٸ��� ������ �����
    /// </summary>
    /// 1. ��ư�� string tabName�� Car�� Particle�� �����ص�.
    public string curType = "Car";//�ʹݺ��� �ٷ� Car�� ���̰�
                                  //�ν����Ͱ� ����ִٸ� �� ���� �����ϹǷ� 
                                  //���� �־���� ��
    public List<Items> CurItemList;


    public void TabClick(string tabName)
    {
        curType = tabName;

        CurItemList = MyItemList.FindAll(x => x.Type == tabName);
        //FindAll�� ���� ������ �����ϴ� ���� ��� �����´�.
        //MyItemList[0].Type ��� ����� �� �ִ� ��.


        for (int i = 0; i < Slot.Length; i++)
        {
            //���԰� �ؽ�Ʈ ���̰� �ϱ�
            bool isExist = i < CurItemList.Count; //�Ұ� = ���ǽĵ� ����...
            Slot[i].SetActive(isExist);
            //�ǿ� ���� CurItemList type��(��:3) ���� �������� ���� text ������ 0,1,2�� ������

            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name /*+ "/" + CurItemList[i].isUsing */ : ""; //�۴ٸ� true,�̸����� / ũ�ٸ� �����


            if (isExist)
            {
                 //���̽� �޸��� �̸� ������ �迭 �̹��� ������ ���ƾ� �Ѵ�.

                 //�迭 �ε����� �� name �߿� 0��°�ΰ� 1��°�ΰ� ã�� ������
                 //�� ���� �̹��� �迭  =   �迭�� ���� �������̹��� [��,������ ������ ������ ��� ��� list
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
               
                //Ŭ�� �� �� üũ �ڽ� ���̰� 
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        
        
        }

        //�� ���� �� �̹��� ����
        int tabNum = 0;
        switch (tabName) //�� ���� ������ sting�� ���� int tabNum�� ����
        {
            case "Car": tabNum = 0; break;
            case "Particle": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            // ���� ������ ��Ȳ�̴ϱ�, i�� Switch���� ������ tabNum�� ���ϰ�, ���ǹ�.
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }

    }

    
    IEnumerator PointerCoroutine;
    //�̹��� ���� ���� ���� ui ������ > slot������Ʈ�� Event Trigger ������Ʈ �߰���
    public void PointerEnter(int sloutNum)
    {
        PointerCoroutine = PointerEnterDelay(sloutNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel.GetComponentInChildren<Text>().text = CurItemList[sloutNum].Name; //CurItemList�迭 �ƴ� �Ű����� ����int���� �޾Ƽ� 
                                                                                       // ���� CurItemList[index]��ȣ�� �񱳰� �Ǵ°���

      
       ExplainPanel.transform.GetChild(2).GetComponentInChildren<Image>().sprite = Slot[sloutNum].transform.GetChild(1).GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[sloutNum].Number + "��";
        ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = CurItemList[sloutNum].Explain;


        //�г� â ��ġ > Update�Լ��� �̵���
        //���콺 ��ġ�� ���� â�� ��ġ�ǵ���  //input.mousePosition�� 0,0���� 1920,1080���� ��
        // ExplainPanel.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;

        //�׷��� ScreenPointToLocalPointInRectangle ���ο� �簢��(ĵ����) ��ġ�� ��ȯ >Update�� �̵�

    }
    //���콺�� 0.5 �Ŀ� �ٷ� �ٸ� ������ �̵��ϸ�,
    //��Ÿ�������� �Ȼ������ �ȴ�.
    //0.5�� ������ false�� �ϰ�(Eixt�Լ�), �� ���� true�Ǵµ�
    //�̹� fasle Eixt�Լ��� �۵��ǹǷ�, ture�� �Ǹ� �ٽ� ���� �� ���� ��
    IEnumerator PointerEnterDelay(int sloutNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }


    public void PointerExit(int sloutNum)
    {
        StopCoroutine(PointerCoroutine); //�׷��� 0.5�� �� �ߵ���Ű����, 
                                                    // Exit�Լ��� �ƴ� �����ص� Dalay�� ���߰�
                                                    // StopCoroutine�� ���� Delay�Լ��� ã�� ���ϹǷ�, 
                                                    // IEnumerator PointerCoroutine;�� ���� �ϳ� ����� ����, Dalay�Լ��� �����ص�
        ExplainPanel.SetActive(false);
    }



    void Save()
    {
        //SerializeObject�� ����ȭ �����ִ� ����
        //���� �÷����տ� �����ϰ� �ʹٸ�, jdata�� ��Ʈ�� �������� �ѱ�� �ȴ���
        // json���� string�������� ��ȯ ��, ������ ����(����ǻ��)�� ���尡��
        //json�� ������ �ǹ��ϴ� ���� �ƴ�, string���� ��ȯ�ϴ� ����������
        string jdata = JsonConvert.SerializeObject(MyItemList);
        print(jdata);


        //Json�� mysql,xml ���� ���� ������
        //Application.dataPath - ������̸� �����, pc�� pc��ǥ
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);


        //���� ���ڸ��� �ݿ��� �������� ������Ʈ �ǰ�
        TabClick(curType);


    }

    //����ȭ(����) ���� ���̽� string���� ������ȭ �Ͽ� MyItemList�� ��������
    //string���� list�� ��ȯ(������ȭ)
    void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Items>>(jdata);

        //���� ���ڸ��� �ٷ� Car �ǿ��� ������ �������� �� ���̵���
        TabClick(curType);
    }
}
