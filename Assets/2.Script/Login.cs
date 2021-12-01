using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Login : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
   
    public Button creatNewButton; //���� ����

    public GameObject loginUi;
    public GameObject firstUi;

    void Start()
    {
        //��ư�� �̺�Ʈ�Լ� ����, ��������Ʈ�� ���ٽ� �����
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
