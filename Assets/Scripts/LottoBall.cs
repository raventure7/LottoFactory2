using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LottoBall : MonoBehaviour
{
    //private static FactoryManager factoryManager;

    private WayPoint currentWaypoint;
    private Rigidbody2D rb2D;
    private const float changeDist = 0.02f;

    private SpriteRenderer renderer;
    public int number;
    bool goal;

    private void Start()
    {
        goal = false;
        currentWaypoint = FactoryManager.Instant.firstWaypoint;
        rb2D = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Images/ball_" + number);
        //Setting(1);
    }
    public void Setting(int num)
    {
        Debug.Log(num);
        number = num;
        //img.texture = Resources.Load("Images/ball_" + num);
        //renderer.sprite = Resources.Load<Sprite>("Images/ball_" + number);
    }

    private void FixedUpdate()
    {
        if(currentWaypoint == null)
        {
            if(goal == false)
            {
                //Debug.Log(number + " 번호는 골라인 도착");
                
                FactoryManager.Instant.ballManagerScript.LottoBallCheck(number);
                FactoryManager.Instant.ballManagerScript.LottoBallCountCheck();
                goal = true;
                //Destroy(this);

            }
            return;
        }

        float dist = Vector2.Distance(transform.position, currentWaypoint.GetPosition());
        if(dist <= changeDist)
        {
            currentWaypoint = currentWaypoint.GetnextWaypoint();
        }
        else
        {
            MoveTowards(currentWaypoint.GetPosition());
        }
    }

    private void MoveTowards(Vector3 destination)
    {
        float step = FactoryManager.Instant.ballMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
        transform.Rotate(Vector3.forward, 10.0f, Space.World);
        //rb2D.MovePosition(Vector3.MoveTowards(transform.position, destination, step));
        //rb2D.MoveRotation(rb2D.rotation + FactoryManager.Instant.ballRotSpeed * Time.fixedDeltaTime);
    }
}
