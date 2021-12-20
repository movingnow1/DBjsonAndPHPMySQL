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
    }


    public void SetActives(int intCount)
    {
        //int selectCount = 2; // ���� ��ư�� ��
        int totalButtonCount = 3; // ��ư����Ʈ.Count
        var shuffledItems = Enumerable.Range(0, totalButtonCount)
        .OrderBy(i => Guid.NewGuid())
        .Take(intCount);
        foreach (var i in shuffledItems)
        {
            // ��ư����Ʈ[i].setActive(true);
            schedulars[i].SetActive(true);
        }

    }




    public List<GameObject> schedulars2 = new List<GameObject>();  //��ư ���̷� ���� �ν�
    int numberBtn;         //��ư �ν� �� Ŭ�� �� int�� ���� ����
    public List<int> scedularsInt = new List<int>();//���Ḯ��Ʈ. ��ư Ŭ�� ���� ���


    public void Update()
    {
        OnSaveSquence();
    
    }

    //��ư Ŭ���� ������ ���� ���(���Ḯ��Ʈ)
    public void OnSaveSquence()
    {
        //���� �ν� ��, ��ư Ŭ���� ������ ���Ḯ��Ʈ�� ����(�ν���Ʈ â�� ������� �߰���)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
   
            if (hit.transform.gameObject == schedulars2[0])
            {
                numberBtn = 0; 

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn); //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
            if (hit.transform.gameObject == schedulars2[1])
            {
                numberBtn = 1;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn);  //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
            if (hit.transform.gameObject == schedulars2[2])
            {
                numberBtn = 2;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn);  //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
        }
    }

  
    //���� Ŭ�� ��, ���Ḯ��Ʈ�� ����� Ŭ��������� �� ��ȯ(������Ʈ ����)
    public void OnExecutionSequence()
    {
        for (int i = 0; i < scedularsInt.Count; i++)
        {
            // int numbers=scedularsInt.FindIndex(i=>i==numberBtn);
            int numbers = scedularsInt.IndexOf(i); //Ŭ���� ���� ������� debug����
            Debug.Log(numbers);
        }
    }
}
