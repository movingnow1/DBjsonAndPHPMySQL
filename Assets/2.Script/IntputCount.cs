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
    }


    public void SetActives(int intCount)
    {
        //int selectCount = 2; // 켜질 버튼의 수
        int totalButtonCount = 3; // 버튼리스트.Count
        var shuffledItems = Enumerable.Range(0, totalButtonCount)
        .OrderBy(i => Guid.NewGuid())
        .Take(intCount);
        foreach (var i in shuffledItems)
        {
            // 버튼리스트[i].setActive(true);
            schedulars[i].SetActive(true);
        }

    }




    public List<GameObject> schedulars2 = new List<GameObject>();  //버튼 레이로 먼저 인식
    int numberBtn;         //버튼 인식 후 클릭 시 int로 먼저 지정
    public List<int> scedularsInt = new List<int>();//연결리스트. 버튼 클릭 순서 기억


    public void Update()
    {
        OnSaveSquence();
    
    }

    //버튼 클릭할 때마다 순서 기억(연결리스트)
    public void OnSaveSquence()
    {
        //레이 인식 후, 버튼 클릭할 때마다 연결리스트로 저장(인스펙트 창에 순서대로 추가됨)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
   
            if (hit.transform.gameObject == schedulars2[0])
            {
                numberBtn = 0; 

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn); //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
            if (hit.transform.gameObject == schedulars2[1])
            {
                numberBtn = 1;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn);  //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
            if (hit.transform.gameObject == schedulars2[2])
            {
                numberBtn = 2;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn);  //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
        }
    }

  
    //실행 클릭 시, 연결리스트로 기억한 클릭순서대로 씬 전환(오브젝트 생성)
    public void OnExecutionSequence()
    {
        for (int i = 0; i < scedularsInt.Count; i++)
        {
            // int numbers=scedularsInt.FindIndex(i=>i==numberBtn);
            int numbers = scedularsInt.IndexOf(i); //클릭한 숫자 순서대로 debug성공
            Debug.Log(numbers);
        }
    }
}
