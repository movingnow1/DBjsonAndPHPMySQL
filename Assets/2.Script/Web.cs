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



    //��ư �� ���� ���̵� ���� ������ ��ȣ�� ���̽� ������ �޾Ƴ���.
    public void ShowUserItems()
    {
        //StartCoroutine(GetItemsIDs(Main.instance.userinfo.userID));
    }

    //DB �� �̹��� �ޱ�
    public IEnumerator GetItemIcon(string itemID, System.Action<byte[]> callback) //������ Action<sprite> callback
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItemIcon.php", form))
        {
            yield return www.SendWebRequest(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                //Debug.Log(www.downloadHandler.text); //���� ����� �뵵
                Debug.Log("DOWNLOADING ICON:" + itemID); //���� ����� �뵵

                //�����͸� ����Ʈ �迭�� ����
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
            yield return www.Send(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text);

                //�����͸� ����Ʈ �迭�� ����
                byte[] results = www.downloadHandler.data;   //���� ����� �뵵
            }
        }
     
    }
    
    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/unitybackendtutoiral/GetUsers.php"))
        {
            yield return www.Send(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵

                //�����͸� ����Ʈ �迭�� ����
                byte[] results = www.downloadHandler.data;
            }
        }

    }

    public Text userName;
    public Text Coins;
    public Text Level;

    public GameObject itemObject;
    //������ ���� �� Ȯ�� ���� ���� id�� ��
    // ��й�ȣ�� �����ͺ��̽� ��й�ȣ�� ������ �� POST��Ļ��
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
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵
                Main.instance.userinfo.SetCredentials(username, password);

                Main.instance.userinfo.SetId(www.downloadHandler.text);
                



                if (www.downloadHandler.text.Contains("Wrong Credentials")||www.downloadHandler.text.Contains("Username does not exists"))
                { Debug.Log("Try Again"); }
                else
                {
                    Main.instance.userProfile.SetActive(true);

                    userName.text =username + "  ��";
                

                    Main.instance.login.gameObject.SetActive(false);
                }
            

            }
        }
    }



    // ��й�ȣ�� �����ͺ��̽� ��й�ȣ�� ������ �� POST��Ļ��
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
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵
            }
        }
    }


    //���� ���̵� ���� ������ ��ȣ�� ���̽� ������ �޾Ƴ���.
    public IEnumerator GetItemsIDs(string userID,System.Action<string>callback)
    {
        WWWForm form = new WWWForm(); //���� ������ ��� WWWForm
        form.AddField("userID", userID);
       
        //�ش� php�� form�� ����
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItemsIDs.php", form))
        {
            yield return www.Send(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵


                //IEnumerator �� Json�� ���ڿ��� ��ȯ �Ұ��� �ϹǷ� �ݹ��Լ� ����
                // Call callback function to pass results
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }

    }


    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm(); //���� ������ ��� WWWForm
        form.AddField("itemID", itemID);

        //�ش� php�� form�� ����
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/GetItem.php", form))
        {
            yield return www.Send(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵

                //IEnumerator �� Json�� ���ڿ��� ��ȯ �Ұ��� �ϹǷ� �ݹ��Լ� ����
                // Call callback function to pass results
                string jsonArray = www.downloadHandler.text;

                callback(jsonArray);
            }
        }

    }



    public IEnumerator SellItem(string ID,string itemID, string userID)
    {
        WWWForm form = new WWWForm(); //���� ������ ��� WWWForm
        form.AddField("itemID", itemID); //�������� ��� ���� �ʿ���
        form.AddField("userID", userID);
        form.AddField("ID", ID); //itemId�� userID�� �ټ��� ���� �� ���ÿ� �� �����ǹǷ� �ʿ� 
        //�ش� php�� form�� ����
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unitybackendtutoiral/SellItem.php", form))
        {
            yield return www.Send(); //����������� ������ ������ ��ٷ��� �ϹǷ� �ڷ�ƾ ���
                                     // �� �����ӿ��� ��� �Ϸ��� ���� ����
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }

            else
            {
                Debug.Log(www.downloadHandler.text); //���� ����� �뵵

               
                
            }
        }

    }
}
