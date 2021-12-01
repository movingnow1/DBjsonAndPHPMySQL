
using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Company_Contest_Final : MonoBehaviour
{
    
    public Image image;
    string fileName;
    public InputField inputField;

   private void Update()
    {
        fileName = inputField.text;
    }

    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadImage(path));
        });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
               
                image.sprite = Sprite.Create(uwrTexture, new Rect(0f, 0f, uwrTexture.width,uwrTexture.height), Vector2.zero);
           
                //image.SetNativeSize(); //스프라이트 이미지와 같아 지도록 함

            }
        }
    }


    //<summary>
    //click event - save image to disk
    //</summary>
    public void OnSaveImageButtonClick()
    {
        if (image.sprite == null)
        {
            Debug.LogError("No Image to Save!");
            return;
        }
        WriteImageOndisk();
    }

    //generate texture bytes and save to disk
    private void WriteImageOndisk()
    {
        byte[] textureBytes = image.sprite.texture.EncodeToPNG(); //바이트로 변환하고자 이미지 스프라이트에 먼저 접근

        //php로 업로드 한다면 이 영상 참고-Upload and Receive Images as LongText format Between Unity 3D and Mysql Database Via PHP & C# - 2021
        //String imageString = Convert.ToBase64String(byte[]); 8비트배열을 string으로 전환

        File.WriteAllBytes(Application.persistentDataPath + fileName, textureBytes);
    
        Debug.Log("Filed Written On Disk!");
        fileName = null;

    }

    //<summary>
    //click event  - make image blank
    //</summary>
    public void OnBlankImageButtonClick()
    {
        image.sprite = null;
    }


    // <summary>
    //Click event - load image from disk
    // </sumary>
    public void OnLoadImageFromDiskButtonClickI()
    {
        if (!File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.LogError("File Not Exist!");
            return;
        }
        LoadImageromDist();
    }

    //load texture byte from disk and compose sprite from bytes
    private void LoadImageromDist()
    {
        byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + fileName);
        Texture2D loadedTexture = new Texture2D(0, 0);
        loadedTexture.LoadImage(textureBytes);
       
        image.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
       
       // image.SetNativeSize(); //스프라이트 크기가 같아지도록 하는 거임

    }
}
