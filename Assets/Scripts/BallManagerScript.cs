using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class LottoResult
{
    public int head;
    public int num1;
    public int num2;
    public int num3;
    public int num4;
    public int num5;
    public int num6;
}

public class BallManagerScript : MonoBehaviour
{
    public GameObject spwanPoint;
    public ArrayList lottoBallList;
    public ArrayList delLottoBallList;
    public List<LottoResult> LottoResultList = new List<LottoResult>();
    public int[] lottoResult;
    public int ballCount;

    private void Awake()
    {
        lottoBallList = new ArrayList();
        delLottoBallList = new ArrayList();
        lottoResult = new int[6] { 1, 2, 3, 4, 5, 6 };
    }

    void Start()
    {
        //Init();
    }

    // 로또 볼 생성
    public void Init()
    {
        MainState.SetState(MainState.State.Operation); // 작동 중으로 상태 변경
        ballCount = 0;
        LottoResultList.Clear(); // 결과 배열 초기화
        ArrayInit(); // 배열 초기화
        ClearBall(); // 오브젝트 지우기
        FactoryManager.Instant.FactoryReset(); // 팩토리 초기화

        ResultManagerScript.Instant.resultScript0.ResetImage();
        ResultManagerScript.Instant.resultScript1.ResetImage();
        ResultManagerScript.Instant.resultScript2.ResetImage();
        ResultManagerScript.Instant.resultScript3.ResetImage();
        ResultManagerScript.Instant.resultScript4.ResetImage();
        ResultManagerScript.Instant.SpecialResult.SetActive(false);

        StartCoroutine("CreateLottoBalls"); // 공 생성
        result(); // 결과 만들기.
    }

    // 배열 초기화
    void ArrayInit()
    {
        lottoBallList.Clear();
        delLottoBallList.Clear();

        for (int i = 1; i<= 45; i++)
        {
            lottoBallList.Add(i);
        }
    }

    // 공 삭제
    void ClearBall()
    {
        Transform[] childList = GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
    }

    // 공 만들기
    IEnumerator CreateLottoBalls()
    {
        for (int i = 1; i <= 45; i++)
        {
            GameObject pBall = Resources.Load("Prefabs/ball") as GameObject;
            pBall.GetComponent<LottoBall>().number = i;
            pBall.name = "Ball_" + i;
            Instantiate(pBall, spwanPoint.transform.position, Quaternion.identity).transform.SetParent(this.transform);
            //pBall.GetComponent<LottoBall>().Setting(i);
            // 공 45개 생성이 끝나면 결과 만들기
            /*
            if(i == 45)
            {
                result();
            }
            */
            
            yield return new WaitForSeconds(0.1f);            
        }
    }

    // 결과 만들기
    void result()
    {
        Debug.Log("delLottoBallList  : " + delLottoBallList.Count);
        Debug.Log("lottoBallList  : " + lottoBallList.Count);

        for(int i = 0; i< 4; i++)
        {
            DefaultLottoResult(i);
        }
        SpecialLottoResult();
        //LottoBallResult(4); // 스페셜 로또 번호 강제 넣기.
    }

    // 기본 결과 만들기
    void DefaultLottoResult(int head)
    {
        bool check;
        int num = 0;

        for (int i = 0; i <= 5; i++)
        {
            check = false;
            int rand = UnityEngine.Random.Range(0, lottoBallList.Count - 1);
            num = int.Parse(lottoBallList[rand].ToString());

            for (int j = 0; j <= lottoResult.Length - 1; j++)
            {
                //Debug.Log(j+"/"+lottoResult.Length+": "+ lottoResult[j]);                
                if (lottoResult[j] == num)
                {
                    check = true;
                    j = 100; // j 루프 탈출
                    i--; // 다시 루프 들어가기
                }

            }

            if (check == false)
            {
                lottoResult[i] = num;
            }
        }

        int k = 0;

        // 정렬
        foreach (int sort in lottoResult.OrderBy(sorted => sorted))
        {
            lottoResult[k++] = sort;
        }

        LottoResultList.Add(new LottoResult
        {
            head = head,
            num1 = lottoResult[0],
            num2 = lottoResult[1],
            num3 = lottoResult[2],
            num4 = lottoResult[3],
            num5 = lottoResult[4],
            num6 = lottoResult[5]
        });

    }
    // 스페셜 결과 만들기
    void SpecialLottoResult()
    {
        bool check = false;

        for (int i = 0; i <= 5; i++)
        {
            check = false;
            int rand = UnityEngine.Random.Range(0, delLottoBallList.Count - 1);
            int num = int.Parse(delLottoBallList[rand].ToString());

            for (int j = 0; j <= lottoResult.Length - 1; j++)
            {
                //Debug.Log(j+"/"+lottoResult.Length+": "+ lottoResult[j]);                
                if (lottoResult[j] == num)
                {
                    check = true;
                    j = 100; // j 루프 탈출
                    i--; // 다시 루프 들어가기
                }
            }
            if (check == false)
            {
                lottoResult[i] = num;
            }
        }


        int k = 0;

        // 정렬
        foreach (int sort in lottoResult.OrderBy(sorted => sorted))
        {
            lottoResult[k++] = sort;
        }
        LottoResultList.Add(new LottoResult
        {
            head = 4,
            num1 = lottoResult[0],
            num2 = lottoResult[1],
            num3 = lottoResult[2],
            num4 = lottoResult[3],
            num5 = lottoResult[4],
            num6 = lottoResult[5]
        });
    }

    // 공 개수 체크하기
    public void LottoBallCountCheck()
    {
        
        ballCount++;
        if(ballCount == lottoBallList.Count)
        {
            //공 개수와 로또볼 리스트 개수가 동일하면 종료 처리
            Debug.Log("번호 추출 완료");
            ResultManagerScript.Instant.SpecialResultActive(true);
            MainState.SetState(MainState.State.End);
        }
        //ResultManagerScript.Instant.SpecialImgSetting(ballCount);
    }
    // 결과 체크 하기
    public void LottoBallCheck(int num)
    {
        for(int i = 0; i < 4; i++)
        {
            int index = IndexCheck(i, num);
            if(index >= 1)
            {
                ResultManagerScript.Instant.BallImgSetting(i, index, num);
            }
            //Debug.Log("HEAD: " + i + " / INDEX: " + index);
        }
    }

    // 결과 넣기 2
    public void LottoBallResult(int head)
    {
        ResultManagerScript.Instant.BallImgSetting(head, 1, LottoResultList[head].num1);
        ResultManagerScript.Instant.BallImgSetting(head, 2, LottoResultList[head].num2);
        ResultManagerScript.Instant.BallImgSetting(head, 3, LottoResultList[head].num3);
        ResultManagerScript.Instant.BallImgSetting(head, 4, LottoResultList[head].num4);
        ResultManagerScript.Instant.BallImgSetting(head, 5, LottoResultList[head].num5);
        ResultManagerScript.Instant.BallImgSetting(head, 6, LottoResultList[head].num6);
    }

    public int IndexCheck(int head, int value)
    {
        if(LottoResultList[head].num1 == value)
        {
            return 1;
        }
        if (LottoResultList[head].num2 == value)
        {
            return 2;
        }
        if (LottoResultList[head].num3 == value)
        {
            return 3;
        }
        if (LottoResultList[head].num4 == value)
        {
            return 4;
        }
        if (LottoResultList[head].num5 == value)
        {
            return 5;
        }
        if (LottoResultList[head].num6 == value)
        {
            return 6;
        }
        return 0;
    }
}
