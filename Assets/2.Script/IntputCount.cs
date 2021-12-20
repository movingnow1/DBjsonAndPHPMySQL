using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntputCount : MonoBehaviour
{

    public Text inputCountText;

    [SerializeField]
    private string stringCount;

    int intCount;

    public List<GameObject> schedulars = new List<GameObject>();



    
    public List<GameObject> schedulars2 = new List<GameObject>();  //연습용으로 먼저.레이로 각 버튼 먼저 인식
    int numberBtn1 = 0;         //버튼 인식 후 클릭 시 int로 먼저 지정
    int numberBtn2 = 1;
    int numberBtn3 = 2;
    public List<int> scedularsInt = new List<int>();//연결리스트. 버튼 클릭 순서 기억


    //input text값을 받아 int로 변환
    public void OnCountclick()
    {
        stringCount = inputCountText.text;
        intCount = int.Parse(stringCount); //text를 int로 담음

        SetActives(intCount); //2개 입력 시 가끔 1개로 나오므로 완전하지 않음
    }



    //수치 적고 입력버튼 누를 시 그 수치 만큼 랜덤으로 버튼 보임(유니티 카페 도움 받음)
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






    public void Update()
    {
        OnSaveSquence();

    }

    //버튼 클릭할 때마다 순서 기억(연결리스트에 지정된 int를 Add추가)
    public void OnSaveSquence()
    {
        //레이 인식 후, 버튼 클릭할 때마다 연결리스트로 저장(인스펙트 창에 순서대로 추가됨)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.gameObject.CompareTag("ADL1") /*==schedulars[0] 또는   연습용으로는 schedulars2[0]*/)
            {
                //numberBtn1 = 0;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn1); //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
            else if (hit.transform.gameObject.CompareTag("ADL2") /* schedulars2[1]*/)
            {
                //numberBtn2 = 1;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn2);  //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
            else if (hit.transform.gameObject.CompareTag("ADL3")/*schedulars2[2]*/)
            {
                //numberBtn3 = 2;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn3);  //연결 리스트를 쓰면 클릭 순서대로 저장 가능하다고 함
                }
            }
        }
    }

    //씬 로드 위해 int로 ~번째 인덱스의 값 받음
    public int numberFristScene; //맨 처음 씬 로드를 위해  int로 찾음
    public int numberSecondScene; //이후 두번째 씬 로드를 위해 미리 int로 찾음
    public int numberThirdScene; //마지막 세번째 씬 로드를 위해 미리 int로 찾음


    //실행 클릭 시, 리스트 중 1번째만 먼저 씬 전환
    public void OnExecutionSequence()
    {
        #region (참고, 사용안함) 특정 값에 해당되는 인덱스 번호 찾기
        for (int i = 0; i < scedularsInt.Count; i++)
        {

            //잘 안된거. int numbers = scedularsInt.FindIndex(i => i == 0);
            //int numbers = scedularsInt.IndexOf(0); // 값이 0인 인덱스 번호를 찾아라가 됨.
            //Debug.Log(numbers);

        }
        #endregion


//---------사용자 입력숫자 만큼만 나와 index리스트에 넣으므로, 부족하여 에러 발생됨. 고쳐야 함
      
        numberFristScene = scedularsInt.ElementAt(0); //0번째인덱스의 값을 찾기,첫번째 씬전환 목적저 
        Debug.Log(numberFristScene);

        #region 두,세번째 씬전환 위해 미리 인덱스 번호 찾음

        numberSecondScene = scedularsInt.ElementAt(1); //1번째인덱스의 값을 찾기,두번째 씬전환 목적
        numberThirdScene = scedularsInt.ElementAt(2); //1번째인덱스의 값을 찾기,두번째 씬전환 목적
        Debug.Log(numberSecondScene);
        Debug.Log(numberThirdScene);

        #endregion


        #region 첫번째 씬전환가능. 전환되는 씬은 어떤 버튼인지에 따라 결정 
        if (numberFristScene == 0) //0번째인덱스의 값이 0이라면 
        {
          SceneManager.LoadScene(1); 
        }
        else if(numberFristScene == 1) //0번째인덱스가 값1이라면
        {
           SceneManager.LoadScene(2);
        }

        else { SceneManager.LoadScene(3); }
        #endregion
    }
}
