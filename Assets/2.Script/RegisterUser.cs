using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField confrimPasswardInput;
    public Button submitButton;


    void Start()
    {
        //��ư�� �̺�Ʈ�Լ� ����, ��������Ʈ�� ���ٽ� �����
        submitButton.onClick.AddListener(() => {
            StartCoroutine(Main.instance.web.RegisterUser(usernameInput.text, passwordInput.text, confrimPasswardInput.text));
          
        });
    }
}
