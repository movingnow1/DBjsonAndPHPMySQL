using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;
    
    public string userID { get; private set; }
    string userName;
    string userPassword;
    string Level;
    string Conis;




    //��Ʈ�� R 2�� ������ �Լ� �̸��� ��� ��ũ��Ʈ���� ���氡����
    public void SetCredentials(string username,string userpassword)
    {
        userName = username;
        userPassword = userpassword;

    }
    public void Coins(string coin)
    {
        Conis = coin;
    }


    //�α��� ���� �� ���� ���̵� ����� �ڵ带 ���� ����������
    public void SetId(string id)
    {
        userID = id;
      
    }
}
