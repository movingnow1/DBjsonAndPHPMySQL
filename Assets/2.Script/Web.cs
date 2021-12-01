using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Web : MonoBehaviour
{

    void Start()
    {
       // StartCoroutine(GetDate());
       // StartCoroutine(GetUsers());
       // StartCoroutine(Login("testuseree", "123456"));
       // StartCoroutine(RegisterUser("testuser3","111111"));
    }



    //버튼 시 유저 아이디에 따른 아이템 번호를 제이슨 값으로 받아낸다.
    public void ShowUserItems()
    {
        //StartCoroutine(GetItemsIDs(Main.instance.userinfo.userID));
    }

    //DB 내 이미지 받기
    public IEnumerator GetItemIcon(string itemID, System.Action<byte[]> callback) //원래는 Action<sprite> callback
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItemIcon.php", form))
        {
            yield return www.SendWebRequest(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                //Debug.Log(www.downloadHandler.text); //응답 디버깅 용도
                Debug.Log("DOWNLOADING ICON:" + itemID); //응답 디버깅 용도

                //데이터를 바이트 배열로 저장
                byte[] bytes = www.downloadHandler.data;

                ////Create texture2D
                //Texture2D texture = new Texture2D(2, 2);
                //texture.LoadImage(bytes);

                ////create sprite(to be placed in ui)
                //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(bytes);
            }
        }


    }
    IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/unitybackendtutoiral/GetDate.php"))
        {
            yield return www.Send(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text);

                //데이터를 바이트 배열로 저장
                byte[] results = www.downloadHandler.data;   //응답 디버깅 용도
            }
        }
     
    }
    
    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/unitybackendtutoiral/GetUsers.php"))
        {
            yield return www.Send(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도

                //데이터를 바이트 배열로 저장
                byte[] results = www.downloadHandler.data;
            }
        }

    }

    public Text userName;
    public Text Coins;
    public Text Level;

    public GameObject itemObject;
    //아이템 가진 거 확인 위해 유저 id도 얻어냄
    // 비밀번호가 데이터베이스 비밀번호와 같은지 비교 POST방식사용
    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도
                Main.instance.userinfo.SetCredentials(username, password);

                Main.instance.userinfo.SetId(www.downloadHandler.text);
                



                if (www.downloadHandler.text.Contains("Wrong Credentials")||www.downloadHandler.text.Contains("Username does not exists"))
                { Debug.Log("Try Again"); }
                else
                {
                    Main.instance.userProfile.SetActive(true);

                    userName.text =username + "  님";
                

                    Main.instance.login.gameObject.SetActive(false);
                }
            

            }
        }
    }



    // 비밀번호가 데이터베이스 비밀번호와 같은지 비교 POST방식사용
    public IEnumerator RegisterUser(string username, string password, string password2)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        form.AddField("loginPass", password2);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/RegisterUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도
            }
        }
    }


    //유저 아이디에 따른 아이템 번호를 제이슨 값으로 받아낸다.
    public IEnumerator GetItemsIDs(string userID,System.Action<string>callback)
    {
        WWWForm form = new WWWForm(); //웹에 보내는 양식 WWWForm
        form.AddField("userID", userID);
       
        //해당 php에 form을 제출
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItemsIDs.php", form))
        {
            yield return www.Send(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도


                //IEnumerator 는 Json을 문자열로 반환 불가능 하므로 콜백함수 쓴다
                // Call callback function to pass results
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }

    }


    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm(); //웹에 보내는 양식 WWWForm
        form.AddField("itemID", itemID);

        //해당 php에 form을 제출
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItem.php", form))
        {
            yield return www.Send(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도

                //IEnumerator 는 Json을 문자열로 반환 불가능 하므로 콜백함수 쓴다
                // Call callback function to pass results
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }

    }



    public IEnumerator SellItem(string ID,string itemID, string userID)
    {
        WWWForm form = new WWWForm(); //웹에 보내는 양식 WWWForm
        form.AddField("itemID", itemID); //가격정보 얻기 위해 필요함
        form.AddField("userID", userID);
        form.AddField("ID", ID); //itemId와 userID가 다수면 삭제 시 동시에 다 삭제되므로 필요 
        //해당 php에 form을 제출
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/SellItem.php", form))
        {
            yield return www.Send(); //어느시점에서 서버의 응답을 기다려야 하므로 코루틴 사용
                                     // 한 프레임에서 모두 완료할 수는 없음
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //응답 디버깅 용도

               
                
            }
        }

    }
}
