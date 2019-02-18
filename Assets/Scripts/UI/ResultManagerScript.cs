using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManagerScript : MonoBehaviour
{
    static ResultManagerScript main;
    public ResultScript resultScript0;
    public ResultScript resultScript1;
    public ResultScript resultScript2;
    public ResultScript resultScript3;
    public ResultScript resultScript4;

    public GameObject SpecialResult;

    public int specialCount;
    public void Awake()
    {
        main = this;  
    }

    public static ResultManagerScript Instant
    {
        get { return main; }
    }

    public void BallImgSetting(int head, int index, int value)
    {
        switch(head)
        {
            case 0:
                resultScript0.SetImage(index, value);
                break;
            case 1:
                resultScript1.SetImage(index, value);
                break;
            case 2:
                resultScript2.SetImage(index, value);
                break;
            case 3:
                resultScript3.SetImage(index, value);
                break;
            case 4:
                resultScript4.SetImage(index, value);
                break;
        }
    }

    // 스페셜 세팅에서 쓰이는 이미지
    public void SpecialImgSetting(int index)
    {
        resultScript4.SetImage(index, 0);
        SpecialCountCheck();
    }

    public void SpecialCountCheck()
    {
        specialCount++;
        Debug.Log(specialCount);

        if(specialCount == 6)
        {
            //SpecialResult.SetActive(true);
            specialCount = 0;
        }
    }

    public void SpecialResultActive(bool isActive)
    {
        SpecialResult.SetActive(isActive);
    }
}
