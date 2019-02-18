using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public int count;
    int rand;
    int[] randArray;
    bool check;

    public void Start()
    {
        Reset();

        //Debug.Log("배열 갯수 : " + FactoryManager.Instant.ballManagerScript.delLottoBallList.Count);
    }
    public void Reset()
    {
        if(gameObject.activeInHierarchy == true)
        {
            Debug.Log("[" + this.name + "] 팩토리 제거 시작(" + gameObject.activeInHierarchy + ")");
            randArray = new int[count];
            for (int i = 0; i <= count - 1; i++)
            {
                rand = Random.Range(1, 45);
                if (!FactoryManager.Instant.ballManagerScript.delLottoBallList.Contains(rand))
                {
                    randArray[i] = rand;
                    FactoryManager.Instant.ballManagerScript.delLottoBallList.Add(rand);
                    FactoryManager.Instant.ballManagerScript.lottoBallList.Remove(rand);
                }
                else
                {
                    i--;
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        for(int i = 0; i<=count-1; i++)
        {
           
            if(collision.GetComponent<LottoBall>().number == randArray[i])
            {
                //FactoryManager.Instant.ballManagerScript.lottoBallList.Remove(collision.GetComponent<LottoBall>().number);
                //Debug.Log("삭제 :" + collision.gameObject.name);
                Destroy(collision.gameObject);

                if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num1)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(1);
                }
                else if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num2)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(2);
                }
                else if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num3)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(3);
                }
                else if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num4)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(4);
                }
                else if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num5)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(5);
                }
                else if (randArray[i] == FactoryManager.Instant.ballManagerScript.LottoResultList[4].num6)
                {
                    ResultManagerScript.Instant.SpecialImgSetting(6);
                }
            }
        }
        



    }
}
