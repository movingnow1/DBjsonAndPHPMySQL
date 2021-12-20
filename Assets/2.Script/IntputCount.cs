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



    
    public List<GameObject> schedulars2 = new List<GameObject>();  //���������� ����.���̷� �� ��ư ���� �ν�
    int numberBtn1 = 0;         //��ư �ν� �� Ŭ�� �� int�� ���� ����
    int numberBtn2 = 1;
    int numberBtn3 = 2;
    public List<int> scedularsInt = new List<int>();//���Ḯ��Ʈ. ��ư Ŭ�� ���� ���


    //input text���� �޾� int�� ��ȯ
    public void OnCountclick()
    {
        stringCount = inputCountText.text;
        intCount = int.Parse(stringCount); //text�� int�� ����

        SetActives(intCount); //2�� �Է� �� ���� 1���� �����Ƿ� �������� ����
    }



    //��ġ ���� �Է¹�ư ���� �� �� ��ġ ��ŭ �������� ��ư ����(����Ƽ ī�� ���� ����)
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






    public void Update()
    {
        OnSaveSquence();

    }

    //��ư Ŭ���� ������ ���� ���(���Ḯ��Ʈ�� ������ int�� Add�߰�)
    public void OnSaveSquence()
    {
        //���� �ν� ��, ��ư Ŭ���� ������ ���Ḯ��Ʈ�� ����(�ν���Ʈ â�� ������� �߰���)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.gameObject.CompareTag("ADL1") /*==schedulars[0] �Ǵ�   ���������δ� schedulars2[0]*/)
            {
                //numberBtn1 = 0;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn1); //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
            else if (hit.transform.gameObject.CompareTag("ADL2") /* schedulars2[1]*/)
            {
                //numberBtn2 = 1;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn2);  //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
            else if (hit.transform.gameObject.CompareTag("ADL3")/*schedulars2[2]*/)
            {
                //numberBtn3 = 2;

                if (Input.GetMouseButtonUp(0))
                {
                    scedularsInt.Add(numberBtn3);  //���� ����Ʈ�� ���� Ŭ�� ������� ���� �����ϴٰ� ��
                }
            }
        }
    }

    //�� �ε� ���� int�� ~��° �ε����� �� ����
    public int numberFristScene; //�� ó�� �� �ε带 ����  int�� ã��
    public int numberSecondScene; //���� �ι�° �� �ε带 ���� �̸� int�� ã��
    public int numberThirdScene; //������ ����° �� �ε带 ���� �̸� int�� ã��


    //���� Ŭ�� ��, ����Ʈ �� 1��°�� ���� �� ��ȯ
    public void OnExecutionSequence()
    {
        #region (����, ������) Ư�� ���� �ش�Ǵ� �ε��� ��ȣ ã��
        for (int i = 0; i < scedularsInt.Count; i++)
        {

            //�� �ȵȰ�. int numbers = scedularsInt.FindIndex(i => i == 0);
            //int numbers = scedularsInt.IndexOf(0); // ���� 0�� �ε��� ��ȣ�� ã�ƶ� ��.
            //Debug.Log(numbers);

        }
        #endregion


//---------����� �Է¼��� ��ŭ�� ���� index����Ʈ�� �����Ƿ�, �����Ͽ� ���� �߻���. ���ľ� ��
      
        numberFristScene = scedularsInt.ElementAt(0); //0��°�ε����� ���� ã��,ù��° ����ȯ ������ 
        Debug.Log(numberFristScene);

        #region ��,����° ����ȯ ���� �̸� �ε��� ��ȣ ã��

        numberSecondScene = scedularsInt.ElementAt(1); //1��°�ε����� ���� ã��,�ι�° ����ȯ ����
        numberThirdScene = scedularsInt.ElementAt(2); //1��°�ε����� ���� ã��,�ι�° ����ȯ ����
        Debug.Log(numberSecondScene);
        Debug.Log(numberThirdScene);

        #endregion


        #region ù��° ����ȯ����. ��ȯ�Ǵ� ���� � ��ư������ ���� ���� 
        if (numberFristScene == 0) //0��°�ε����� ���� 0�̶�� 
        {
          SceneManager.LoadScene(1); 
        }
        else if(numberFristScene == 1) //0��°�ε����� ��1�̶��
        {
           SceneManager.LoadScene(2);
        }

        else { SceneManager.LoadScene(3); }
        #endregion
    }
}
