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
        intCount = int.Parse(stringCount); //text를 int로 담음

        SetActives(intCount); //2개 입력 시 가끔 1개로 나오므로 완전하지 않음

        //그 개수만큼 나타나도록

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

        int selectCount = 2; // 켜질 버튼의 수
        int totalButtonCount = 3; // 버튼리스트.Count
        var shuffledItems = Enumerable.Range(0, totalButtonCount)
        .OrderBy(i => Guid.NewGuid())
        .Take(selectCount)
        ;
        foreach (var i in shuffledItems)
        {
            // 버튼리스트[i].setActive(true);
            schedulars[i].SetActive(true);

        }


        //for (int i = 0; i < intCount; i++) //input 2라고 입력한 int값 내에서 돌면서 
        //{

        //    schedulars[Random.Range(0, 3)].SetActive(true);  //0-2 사이 랜덤한 오브젝트 활성화  but 1개만 나올때도 있음

        //}
    }
}
