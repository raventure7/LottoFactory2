using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    public int head;
    public Image ballImg1;
    public Image ballImg2;
    public Image ballImg3;
    public Image ballImg4;
    public Image ballImg5;
    public Image ballImg6;


    public void SetImage(int index, int value)
    {
        switch(index)
        {
            case 1:
                ballImg1.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
            case 2:
                ballImg2.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
            case 3:
                ballImg3.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
            case 4:
                ballImg4.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
            case 5:
                ballImg5.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
            case 6:
                ballImg6.sprite = Resources.Load<Sprite>("Images/ball_" + value);
                break;
                
        }
    }

    // 리셋 이미지
    public void ResetImage()
    {
        ballImg1.sprite = Resources.Load<Sprite>("Images2/badge_silver");
        ballImg2.sprite = Resources.Load<Sprite>("Images2/badge_silver");
        ballImg3.sprite = Resources.Load<Sprite>("Images2/badge_silver");
        ballImg4.sprite = Resources.Load<Sprite>("Images2/badge_silver");
        ballImg5.sprite = Resources.Load<Sprite>("Images2/badge_silver");
        ballImg6.sprite = Resources.Load<Sprite>("Images2/badge_silver");
    }
}
