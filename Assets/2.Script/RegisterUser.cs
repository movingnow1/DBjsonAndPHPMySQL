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
        //버튼에 이벤트함수 적용, 델리게이트나 람다식 사용함
        submitButton.onClick.AddListener(() => {
            StartCoroutine(Main.instance.web.RegisterUser(usernameInput.text, passwordInput.text, confrimPasswardInput.text));
          
        });
    }
}
