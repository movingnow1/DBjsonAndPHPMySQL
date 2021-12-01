using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
   
    public Button creatNewButton; //계정 생성

    public GameObject loginUi;
    public GameObject firstUi;

    void Start()
    {
        //버튼에 이벤트함수 적용, 델리게이트나 람다식 사용함
        loginButton.onClick.AddListener(() => {
            StartCoroutine(Main.instance.web.Login(usernameInput.text, passwordInput.text));
            //StartCoroutine(Main.instance.web.GetUsers());
        });


        creatNewButton.onClick.AddListener(delegate { ChangeUi(); }) ;
       
    }

    void ChangeUi()
    {
        loginUi.SetActive(false);
        firstUi.SetActive(true);
    }

  
}
