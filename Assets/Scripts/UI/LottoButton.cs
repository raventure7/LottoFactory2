using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LottoButton : MonoBehaviour
{

    public void level1Button()
    {
        //Debug.Log("버튼 출력");
        FactoryManager.Instant.Level2SetActive(false);
        StateCheck();

    }

    public void level2Button()
    {
        FactoryManager.Instant.Level2SetActive(true);

        StartCoroutine("DelayStart");
    }
    
    public void ActionButton(int num)
    {
        switch (MainState.GetState)
        {
            case MainState.State.Operation:
                GuideManagerScript.Instant.SetStateGuide("[Tip] 작동이 종료 된 후 다시 눌러주세요.", 0);
                StartCoroutine("DelayMsg");
                break;
            default:
                if(num == 1)
                {
                    FactoryManager.Instant.Level2SetActive(false);
                    FactoryManager.Instant.ballManagerScript.Init();
                }
                else if(num ==2 )
                {
                    FactoryManager.Instant.Level2SetActive(true);
                    StartCoroutine("DelayStart");
                }
                
                break;
        }
    }

    public void StateCheck()
    {
        switch (MainState.GetState)
        {
            case MainState.State.Operation:
                GuideManagerScript.Instant.SetStateGuide("[Tip] 작동이 종료 된 후 다시 눌러주세요.", 0);
                StartCoroutine("DelayMsg");
                break;
            default:
                FactoryManager.Instant.ballManagerScript.Init();
                break;
        }
    }

    IEnumerator DelayStart()
    {
        GuideManagerScript.Instant.SetStateGuide("잠시만 기다려주세요.", 0);
        yield return new WaitForSeconds(1.0f);
        FactoryManager.Instant.ballManagerScript.Init();
    }

    IEnumerator DelayMsg()
    {
        yield return new WaitForSeconds(0.5f);
        GuideManagerScript.Instant.SetStateGuide("제외수 알고리즘이 작동 중입니다.", 0);
    }
}
