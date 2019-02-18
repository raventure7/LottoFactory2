using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideManagerScript : MonoBehaviour
{
    static GuideManagerScript main;
    public Text titleGuide;
    public Text stateGuide;


    public Vector3 scaleZoom = new Vector3(1.5f, 1.5f, 1.5f);
    float timeZoomOut = 0.5f;
    float timeZoomIn = 0.6f;

    bool running = false;
    bool stateChange = false;
    bool titleChange = false;

    private void OnEnable()
    {
        running = false;
        //transform.localScale = Vector3.one;
    }

    private void Awake()
    {
        main = this;
    }

    public static GuideManagerScript Instant
    {
        get { return main; }
    }


    public void SetTitleGuide(string msg, int state)
    {
        titleChange = true;
        if (state == 1)
        {
            StartCoroutine(DynamicFont(titleGuide));
        }
        titleGuide.text = msg;
    }

    public void SetStateGuide(string msg, int state)
    {
        stateChange = true;
        if (state == 1)
        {
            StartCoroutine(DynamicFont(stateGuide));
        }
        stateGuide.text = msg;
    }


    IEnumerator DynamicFont(Text str)
    {
        //Debug.Log(str.transform.localScale);
        //yield return new WaitForSeconds(0.1f);
        stateChange = false;
        titleChange = false;
        while (!titleChange)
        {
            str.transform.localScale = Vector3.one;
            running = true;
            float currentTime = 0;
            while (currentTime < timeZoomOut)
            {
                str.transform.localScale = Vector3.Lerp(Vector3.one, scaleZoom, currentTime / timeZoomOut);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            //MainAudio.Main.PlaySound(TypeAudio.SoundClick);
            currentTime = 0;
            while (currentTime < timeZoomIn)
            {
                str.transform.localScale = Vector3.Lerp(scaleZoom, Vector3.one, currentTime / timeZoomIn);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            running = false;
            if (running == false)
            {
                str.transform.localScale = Vector3.one;
            }
        }
        /*
        str.transform.localScale = Vector3.one;        
        running = true;
        float currentTime = 0;
        while (currentTime < timeZoomOut)
        {
            str.transform.localScale = Vector3.Lerp(Vector3.one, scaleZoom, currentTime / timeZoomOut);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }
        //MainAudio.Main.PlaySound(TypeAudio.SoundClick);
        currentTime = 0;
        while (currentTime < timeZoomIn)
        {
            str.transform.localScale = Vector3.Lerp(scaleZoom, Vector3.one, currentTime / timeZoomIn);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        running = false;
        if (running == false)
        {
            str.transform.localScale = Vector3.one;
        }
        */
        
    }

}
