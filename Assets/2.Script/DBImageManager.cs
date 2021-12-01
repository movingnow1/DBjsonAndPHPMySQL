using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�Է°� ��� ��Ÿ���� �ý���
using SimpleJSON;

public class DBImageManager : MonoBehaviour
{
    public static DBImageManager instance;

    string _basePath; //�⺻���

    string _versionJsonPath; //���̽� ���� ��� 
    JSONObject _versionJson; //���̽� ��ü ����, ���� Json����


   // �̹��� ���� ������Ʈ 
    //1. Json���� �� �̹��� ���� ���� Create Version Dictionary(Json)
    //2. �������� �����
      // ���� �߰����� �������� ������ ���� ���� �־�� �ϰ� 
      // �̹��� ������Ʈ�� ������ ������Ʈ ��Ű��
      // Add version in DB
     //3. php��ũ��Ʈ�� ���� �������� Get Version in our Item Info(PHP)
     //4. �� ���� ����, ���� ������ ���� �������� �� Save version and Compare Version
     //5. ���� Ȯ�� �� ����, ����̽��� �ٿ����� Check version json and decide if we should download or load from device
     //6. ���� ���� ���� �� Json���Ͽ� ���� ���� �־����� �˱� ���� Json���� ���� �ε� Save and Load Json File
    




    /* �̹����� ������ �������� �����Ű��
    0. �̹����� ���� ��θ� ���� �� 
    1. �̹����� �̹� �ִ��� üũ/Ȯ�� �ϴ� �Լ� ����
    2. �̹��� ����
    3. �̹��� �ε� �Լ� (IO ���, byte�迭�� ����)
    4.  ���� �Ϸ�� �̹��� �� �������� Try to get Image */


    void Start()
    {
        if (instance != null)
        {
            GameObject.Destroy(this); //�ν��ٽ��� �ı�
        }
        instance = this;



        //0.�̹����� ���� ��θ� ���� ��
        //�⺻��� = ���ø����̼�.���������� ��ο� ���� ����, �̹������ �θ� ���丮 �߰�
        //�����ø� �ϸ� ����� �̵���
        _basePath = Application.persistentDataPath + "/Images/";
        //C:\Users\�����ϱ�\AppData\LocalLow\DefaultCompany\MetaPc2019\Images


        //IO ��� �� ���� ����,����,�����ϴ� ��ɴ�� - Directory
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

    //bool �Լ��� �ؿ� If �������ε� ��� ��  �� �ִ�.
    //1. �̹����� �̹� �ִ��� üũ/Ȯ�� �ϴ� �Լ� ����
    bool ImageExistst(string name) //name = �����ܿ� �ش��ϴ� �׸� ID
    {
       
        return File.Exists(_basePath + name); //������ �����Ѵٸ�
                                              
    }


    // 2. �̹��� ����
    // ����Ʈ �迭 ���� -> Void �Լ��� ��ȯ
    public void SaveImage(string name,byte[] bytes, int imgVer)
    {
        File.WriteAllBytes(_basePath + name, bytes); //����Ʈ�� �� ��ο� ����
        UpdateVersionJson(name, imgVer);
    }

    /// <summary>
    /// �̹��� ã�� �� ���ų� �ֽ� �̹����� �ƴ� ��
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imgVer"></param>
    /// <returns></returns>
    // 3. �̹��� �ε� �Լ� (IO ���)
    public byte[] LoadImage(string name, int imgVer)
    {

        byte[] bytes = new byte[0]; //�� ����Ʈ �迭

        //Compare version ��������
        if (!IsImageUpToDate(name, imgVer))
        {
            return bytes; //������Ʈ �� ���� ������ �� �迭 ��ȯ�ϱ�
        }

        if (ImageExistst(name))
        {
            bytes= File.ReadAllBytes(_basePath + name);

        }
        return bytes;
        //�̹��� �ε������� �迭�� ����־� �̹��� ã�� �� ���ٴ� ���� �˷��� ��
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
        _versionJson[name] = ver; //nameŰ �ְ� ver ������Ʈ�� �ϴ°���
    }

    //���� ������ �������� ��
    bool IsImageUpToDate(string name,int ver)
    {   if (_versionJson[name] != null)
        {
            return _versionJson[name].AsInt == ver;
            //������ ��ȯ�ϰ� ���� json�̸� �����ͼ� int�� ��ȯ �� �� 
        }
        return false;
    }

    //Json���� ���� (���,������,���ڿ���ȯ)
    public void SaveVersionJson()
    {
        File.WriteAllText(_versionJsonPath, _versionJson.ToString());
    }
}
