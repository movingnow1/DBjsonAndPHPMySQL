using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;
    
    public string userID { get; private set; }
    string userName;
    string userPassword;
    string Level;
    string Conis;




    //컨트롤 R 2번 누르면 함수 이름을 모든 스크립트에서 변경가능함
    public void SetCredentials(string username,string userpassword)
    {
        userName = username;
        userPassword = userpassword;

    }
    public void Coins(string coin)
    {
        Conis = coin;
    }


    //로그인 성공 시 유자 아이디를 비쥬얼 코드를 통해 가져오도록
    public void SetId(string id)
    {
        userID = id;
      
    }
}
