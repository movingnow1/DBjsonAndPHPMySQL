using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SimpleJSON;

public class ItemsManager : MonoBehaviour
{ //Ctrl+R 누르면 이름 전체 수정 가능
    Action<string> _creatItemsCallback;

    void Start()
    {
        //요청한 문자열 매개변수에 이름을 지정, 람다식
        // json 배열 기반으로 항목을 생성하도록 함수를 지정
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
        //문자열을 제이슨 배열 문자열로 전환
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //create local variables
            bool isDone = false; //are we done downloading? false가 기본값
            string itemId = jsonArray[i].AsObject["itemID"]; //돌린 제이슨 배열 중 제이슨 객체에서 문자열 키로 담음
            string id = jsonArray[i].AsObject["ID"];
            JSONObject itemInfoJson = new JSONObject();

            //create call to get the information from web.cs
            //웹 클래스에서 정보 가져오기 위해 콜백을 생성
            //콜백이 웹 클래스에서 호출
            Action<string> getItemInfoCallback = (itemInfo) => {
                isDone = true; //웹클래스에서 정보 가져오면 다운이 완료되고 true됨
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Main.instance.web.GetItem(itemId, getItemInfoCallback));

            //콜백 받을 때까지 기다리기...웹 정보 완료 다운로드에서 콜백 호출
            //wait until the callback is called from WEB(info finished download)
            yield return new WaitUntil(() => isDone == true); //조건 true일때까지 기다림

            //instantiate gameobject
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;
            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //정보채우기 Fill information 
            itemGo.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            //이미지가 내 컴퓨터 폴더에 없다면 저장할 수 있도록 함
            //1. Action의 Sprite를 가져와 DBImageManager에 전달하도록
            //   바이트 배열을 가져옴
            //2. 이미지 가져오기
            //3.가져올 수 없을 시에만 이미지 다운

            //4. 이미지 다운 시 Device에 저장
            //5. byte를 스프라이트로 반환(Convert)


            //이미지 버전관리 목적(버전 읽고 DBimage스크립트에 전송
            //1.Get image version and send it to DBImage Manager

            int imgVer = itemInfoJson["imgVer"].AsInt;

            byte[] bytes = DBImageManager.instance.LoadImage(itemId,imgVer);

            //DownLoad from Web
            if (bytes.Length == 0 )
            {
                //create call to get the Sprite from web.cs
                //웹 클래스에서 아이템 이미지 가져오기 위해 콜백을 생성
                //콜백이 웹 클래스에서 호출
                Action<byte[]> getItemIconCallback = (downLoadedBytes) => {

                    #region byte를 sprite로 변환 시 
                    ////Create texture2D
                    //Texture2D texture = new Texture2D(2, 2);
                    //texture.LoadImage(downLoadedBytes);

                    ////create sprite(to be placed in ui)
                    //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    #endregion

                    Sprite sprite = DBImageManager.instance.BytesToSprite(downLoadedBytes);
                    itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;  //스프라이트를 byte화 할 경우  = downLoadedBytes;

                    DBImageManager.instance.SaveImage(itemId, downLoadedBytes,imgVer);
                    
                    DBImageManager.instance.SaveVersionJson();//json버전 비교,
                                                              //아이콘을 다운하기 전에도 컴퓨터에서 이미
                                                              //저장하고 있을 수도 있어서 여기에 위치
                                                              //다운 할 때마다 jason버전을 저장한다.
                };
                StartCoroutine(Main.instance.web.GetItemIcon(itemId, getItemIconCallback));

            }
            //Load From Device 
            else
            {
                Sprite sprite = DBImageManager.instance.BytesToSprite(bytes);
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;  //스프라이트를 byte화 할 경우  = downLoadedBytes;

            }




            //sell버튼 판매버튼
            //정보채우기 Fill information 
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
