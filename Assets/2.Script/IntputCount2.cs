using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntputCount2 : MonoBehaviour
{
    // 처음 씬에서 버튼 순서 누른대로, 이 버튼을 누르면 2번째 버튼에 해당되는 씬으로 이동한다.
    public void OnclickSeconds()
    {
        IntputCount.instance.OnClickScecondsBtn();
    }

}
