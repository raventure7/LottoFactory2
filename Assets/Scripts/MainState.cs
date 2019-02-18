using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState 
{
    public enum State
    {
        Wait,
        Operation,
        End
    }
    public static State state;
    public static void SetState(State newState)
    {
        Debug.Log("상태 변경 : " + state);
        state = newState;

        switch(state)
        {
            case State.Operation:
                GuideManagerScript.Instant.SetTitleGuide("로또 당첨을 위해서 공장이 작동 중 입니다.",1);
                GuideManagerScript.Instant.SetStateGuide("제외수 알고리즘이 작동 중입니다.", 0);
                break;
            case State.End:
                GuideManagerScript.Instant.SetTitleGuide("결과를 확인 해주세요.", 0);
                GuideManagerScript.Instant.SetStateGuide("아래버튼을 클릭하여 다시 공장을 가동 할 수 있습니다.",0);
                break;
        }
    }
    public static State GetState
    {
        get { return state; }
    }
}
