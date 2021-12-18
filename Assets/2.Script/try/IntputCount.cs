using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IntputCount : MonoBehaviour
{
  
    public Text inputCountText;

    [SerializeField]
    private string stringCount;

    int intCount;

    public List<GameObject> schedulars = new List<GameObject>();


    public void OnCountclick()
    {
    
        stringCount = inputCountText.text;
        intCount = int.Parse(stringCount); //text�� int�� ����

        SetActives(intCount); //2�� �Է� �� ���� 1���� �����Ƿ� �������� ����

        //�� ������ŭ ��Ÿ������

        //for (int i = 0; i < schedulars.Length; i++)
        //{
        //    //for (int j = 0; j <= intCount; j++)
        //    //{

        //    //    schedulars[j].SetActive(true);
        //    //}

        //}


    }


    public void SetActives(int intCount)
    {

        int selectCount = 2; // ���� ��ư�� ��
        int totalButtonCount = 3; // ��ư����Ʈ.Count
        var shuffledItems = Enumerable.Range(0, totalButtonCount)
        .OrderBy(i => Guid.NewGuid())
        .Take(selectCount)
        ;
        foreach (var i in shuffledItems)
        {
            // ��ư����Ʈ[i].setActive(true);
            schedulars[i].SetActive(true);

        }


        //for (int i = 0; i < intCount; i++) //input 2��� �Է��� int�� ������ ���鼭 
        //{

        //    schedulars[Random.Range(0, 3)].SetActive(true);  //0-2 ���� ������ ������Ʈ Ȱ��ȭ  but 1���� ���ö��� ����

        //}
    }
}
