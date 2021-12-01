using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using UnityEngine.UI;
using System;

public class LoadImage : MonoBehaviour
{
    string path;
    public RawImage rawImage;
   


    public void OpenFileExplorer()
    {
        //path = EditorUtility.OpenFilePanel("Show all imag(.png)", "", "png");
        //StartCoroutine(GetTexture());

        var bp = new BrowserProperties();
        bp.filter = "Image file (*.jpg,*.jpeg, *.jpe, *jfif, *png)|*.jpg,*.jpeg, *.jpe, *jfif, *png";
        bp.filterIndex = 0;

        //=>는 람다식으로, ()안의 것을 받아서(게터 getter) => 리턴한다는 뜻
        //람다식과 IEnumerable 참고 https://kwangyulseo.com/2016/10/16/ienumerator-ienumerable%EC%9D%98-%EC%9D%98%EB%AF%B8/
        //유뷰트 https://www.youtube.com/watch?v=Z1qT65GL-6Q&list=PLoMwqpIBpk_c0_mXrOCJ--ANh39iL2UZi&index=3
        new FileBrowser().OpenFileBrowser(bp, path =>
         {
             StartCoroutine(LoadImages(path));

         });

    }

    //람다식으로 ()의 것을 받기 위해서(게터를 리턴하는 게터라고 함)IEnumerable을 쓴다.
    private void StartCoroutine(IEnumerable enumerable)
    {
        throw new NotImplementedException();
    }

    IEnumerable LoadImages(string path)
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
                rawImage.texture = uwrTexture;
             
            }
        }
    }
    //IEnumerator GetTexture()
    //{
    //    //유니티에서 웹 요청
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        rawImage.texture = myTexture;
    //    }
    //}
}
