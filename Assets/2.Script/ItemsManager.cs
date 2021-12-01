using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SimpleJSON;

public class ItemsManager : MonoBehaviour
{ //Ctrl+R ������ �̸� ��ü ���� ����
    Action<string> _creatItemsCallback;

    void Start()
    {
        //��û�� ���ڿ� �Ű������� �̸��� ����, ���ٽ�
        // json �迭 ������� �׸��� �����ϵ��� �Լ��� ����
        _creatItemsCallback = (jsonArrayString) => {
            StartCoroutine(CreatItemsRoutine(jsonArrayString));
        };

        CreateItems();
    }

    
   public void CreateItems()
    {
        string userId = Main.instance.userinfo.userID;
        StartCoroutine(Main.instance.web.GetItemsIDs(userId,_creatItemsCallback));
    }

    IEnumerator CreatItemsRoutine(string jsonArrayString)
    {
        //Parsing json arry string as an array
        //���ڿ��� ���̽� �迭 ���ڿ��� ��ȯ
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //create local variables
            bool isDone = false; //are we done downloading? false�� �⺻��
            string itemId = jsonArray[i].AsObject["itemID"]; //���� ���̽� �迭 �� ���̽� ��ü���� ���ڿ� Ű�� ����
            string id = jsonArray[i].AsObject["ID"];
            JSONObject itemInfoJson = new JSONObject();

            //create call to get the information from web.cs
            //�� Ŭ�������� ���� �������� ���� �ݹ��� ����
            //�ݹ��� �� Ŭ�������� ȣ��
            Action<string> getItemInfoCallback = (itemInfo) => {
                isDone = true; //��Ŭ�������� ���� �������� �ٿ��� �Ϸ�ǰ� true��
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Main.instance.web.GetItem(itemId, getItemInfoCallback));

            //�ݹ� ���� ������ ��ٸ���...�� ���� �Ϸ� �ٿ�ε忡�� �ݹ� ȣ��
            //wait until the callback is called from WEB(info finished download)
            yield return new WaitUntil(() => isDone == true); //���� true�϶����� ��ٸ�

            //instantiate gameobject
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;
            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //����ä��� Fill information 
            itemGo.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            //�̹����� �� ��ǻ�� ������ ���ٸ� ������ �� �ֵ��� ��
            //1. Action�� Sprite�� ������ DBImageManager�� �����ϵ���
            //   ����Ʈ �迭�� ������
            //2. �̹��� ��������
            //3.������ �� ���� �ÿ��� �̹��� �ٿ�

            //4. �̹��� �ٿ� �� Device�� ����
            //5. byte�� ��������Ʈ�� ��ȯ(Convert)


            //�̹��� �������� ����(���� �а� DBimage��ũ��Ʈ�� ����
            //1.Get image version and send it to DBImage Manager

            int imgVer = itemInfoJson["imgVer"].AsInt;

            byte[] bytes = DBImageManager.instance.LoadImage(itemId,imgVer);

            //DownLoad from Web
            if (bytes.Length == 0 )
            {
                //create call to get the Sprite from web.cs
                //�� Ŭ�������� ������ �̹��� �������� ���� �ݹ��� ����
                //�ݹ��� �� Ŭ�������� ȣ��
                Action<byte[]> getItemIconCallback = (downLoadedBytes) => {

                    #region byte�� sprite�� ��ȯ �� 
                    ////Create texture2D
                    //Texture2D texture = new Texture2D(2, 2);
                    //texture.LoadImage(downLoadedBytes);

                    ////create sprite(to be placed in ui)
                    //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    #endregion

                    Sprite sprite = DBImageManager.instance.BytesToSprite(downLoadedBytes);
                    itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;  //��������Ʈ�� byteȭ �� ���  = downLoadedBytes;

                    DBImageManager.instance.SaveImage(itemId, downLoadedBytes,imgVer);
                    
                    DBImageManager.instance.SaveVersionJson();//json���� ��,
                                                              //�������� �ٿ��ϱ� ������ ��ǻ�Ϳ��� �̹�
                                                              //�����ϰ� ���� ���� �־ ���⿡ ��ġ
                                                              //�ٿ� �� ������ jason������ �����Ѵ�.
                };
                StartCoroutine(Main.instance.web.GetItemIcon(itemId, getItemIconCallback));

            }
            //Load From Device 
            else
            {
                Sprite sprite = DBImageManager.instance.BytesToSprite(bytes);
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;  //��������Ʈ�� byteȭ �� ���  = downLoadedBytes;

            }




            //sell��ư �ǸŹ�ư
            //����ä��� Fill information 
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>{
                string idInventory = id;
                string iId=itemId;
                string userId=Main.instance.userinfo.userID;
                StartCoroutine(Main.instance.web.SellItem(idInventory,itemId, userId));
            });
          

            //contine to the next item

        }
     

        yield return null;
    }
   
}
