using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //입력과 출력 나타나는 시스템
using SimpleJSON;

public class DBImageManager : MonoBehaviour
{
    public static DBImageManager instance;

    string _basePath; //기본경로

    string _versionJsonPath; //제이슨 파일 경로 
    JSONObject _versionJson; //제이슨 개체 생성, 버전 Json대조


   // 이미지 버전 업데이트 
    //1. Json으로 각 이미지 버전 추적 Create Version Dictionary(Json)
    //2. 버전사전 만들기
      // 버전 추가위한 목적으로 버전에 대한 열이 있어야 하고 
      // 이미지 업데이트할 때마다 업데이트 시키기
      // Add version in DB
     //3. php스크립트로 버전 가져오기 Get Version in our Item Info(PHP)
     //4. 새 버전 저장, 현재 버전이 기존 버전인지 비교 Save version and Compare Version
     //5. 버전 확인 후 결정, 디바이스에 다운저장 Check version json and decide if we should download or load from device
     //6. 게임 열고 닫을 때 Json파일에 무슨 일이 있었는지 알기 위해 Json파일 저장 로드 Save and Load Json File
    




    /* 이미지가 폴더에 없을때만 저장시키기
    0. 이미지에 대한 경로를 만든 후 
    1. 이미지가 이미 있는지 체크/확인 하는 함수 생성
    2. 이미지 저장
    3. 이미지 로드 함수 (IO 사용, byte배열로 받음)
    4.  거의 완료된 이미지 잘 가져오기 Try to get Image */


    void Start()
    {
        if (instance != null)
        {
            GameObject.Destroy(this); //인스텐스를 파괴
        }
        instance = this;



        //0.이미지에 대한 경로를 만든 후
        //기본경로 = 어플리케이션.영구데이터 경로와 같게 설정, 이미지라고 부를 디렉토리 추가
        //슬래시를 하면 여기로 이동함
        _basePath = Application.persistentDataPath + "/Images/";
        //C:\Users\누굽니까\AppData\LocalLow\DefaultCompany\MetaPc2019\Images


        //IO 기능 중 폴더 생성,관리,삭제하는 기능담당 - Directory
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }

        _versionJson = new JSONObject();
        _versionJsonPath = _basePath + "VersionJson";
        if (File.Exists(_versionJsonPath))
        {
            string jsonString = File.ReadAllText(_versionJsonPath);
            _versionJson = JSON.Parse(jsonString) as JSONObject;
        }

    }

    //bool 함수는 밑에 If 조건으로도 사용 할  수 있다.
    //1. 이미지가 이미 있는지 체크/확인 하는 함수 생성
    bool ImageExistst(string name) //name = 아이콘에 해당하는 항목 ID
    {
       
        return File.Exists(_basePath + name); //파일이 존재한다면
                                              
    }


    // 2. 이미지 저장
    // 바이트 배열 저장 -> Void 함수로 반환
    public void SaveImage(string name,byte[] bytes, int imgVer)
    {
        File.WriteAllBytes(_basePath + name, bytes); //바이트를 이 경로에 저장
        UpdateVersionJson(name, imgVer);
    }

    /// <summary>
    /// 이미지 찾을 수 없거나 최신 이미지가 아닐 때
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imgVer"></param>
    /// <returns></returns>
    // 3. 이미지 로드 함수 (IO 사용)
    public byte[] LoadImage(string name, int imgVer)
    {

        byte[] bytes = new byte[0]; //빈 바이트 배열

        //Compare version 버전관리
        if (!IsImageUpToDate(name, imgVer))
        {
            return bytes; //업데이트 할 것이 없으면 빈 배열 반환하기
        }

        if (ImageExistst(name))
        {
            bytes= File.ReadAllBytes(_basePath + name);

        }
        return bytes;
        //이미지 로드했지만 배열이 비어있어 이미지 찾을 수 없다는 것을 알려줄 것
    }

    //
   public Sprite BytesToSprite(byte[] bytes)
    {
        //Create texture2D
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        //create sprite(to be placed in ui)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }


    void UpdateVersionJson(string name,int ver)
    {
        _versionJson[name] = ver; //name키 넣고 ver 업데이트만 하는거임
    }

    //현재 버전과 이전버전 비교
    bool IsImageUpToDate(string name,int ver)
    {   if (_versionJson[name] != null)
        {
            return _versionJson[name].AsInt == ver;
            //버전을 반환하고 싶은 json이름 가져와서 int로 변환 후 비교 
        }
        return false;
    }

    //Json버전 저장 (경로,콘텐츠,문자열전환)
    public void SaveVersionJson()
    {
        File.WriteAllText(_versionJsonPath, _versionJson.ToString());
    }
}
