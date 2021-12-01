using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    [Header("InputMyRegistry")]
    public InputField nameField;
    public InputField passField;



    public Button submibutton;

    [System.Obsolete]
    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    [System.Obsolete]
    IEnumerator Register()
    {
        WWWForm form= new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passField.text);

        //WWW www = new WWW("http://localhost/sqlconnect/register.php",form);
        //yield return www;
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);
        yield return www.SendWebRequest();



        if (www.downloadHandler.text == "0")
        {
            Debug.Log("User created sucessfully");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        else
        {
            Debug.Log("User Creation Failed.Error #" + www.downloadHandler.text);
        }
    }
    public void VertifyInputs()
    {
        submibutton.interactable = (nameField.text.Length >= 8 && passField.text.Length >= 8);
    }    

}
