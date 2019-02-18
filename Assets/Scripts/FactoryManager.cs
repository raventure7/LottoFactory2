using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    static FactoryManager main;
    public BallManagerScript ballManagerScript;

    public WayPoint firstWaypoint;
    public Factory[] factorys;
    public GameObject level2;


    public float ballMoveSpeed;
    public float ballRotSpeed;

    private void Awake()
    {
        Application.targetFrameRate = 60;   // 프레임 고정
        Time.timeScale = 1; //실시간
        main = this;
        MainState.SetState(MainState.State.Wait);
    }

    public static FactoryManager Instant
    {
        get { return main; }
    }

    public void FactoryReset()
    {
        for(int i = 0; i <= factorys.Length-1; i++)
        {
            //Debug.Log(factorys[i].gameObject.name);
            factorys[i].Reset();
            //factorys[i].GetComponent<Factory>().Reset();
        }
    }
    public void Level2SetActive(bool isActive)
    {
        level2.SetActive(isActive);

    }
}
