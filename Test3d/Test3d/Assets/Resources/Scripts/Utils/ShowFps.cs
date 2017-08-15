using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowFps : MonoBehaviour
{
    public Color Color = Color.green;
    public Rect Rect = new Rect(200, 5, 100, 25);
    public bool Display = true;

    float LastTime;
    int FPS, Number;

    void Start()
    {
        Application.targetFrameRate = 60;
    }
    
    void Update()
    {
        if (Display)
        {
            if (Time.time - LastTime > 1)
            {
                LastTime = Time.time;
                FPS = Number;
                Number = 0;
            }
            else
            {
                Number++;
            }
        }
    }

    // 这个函数导出ios工程会报错，所以关掉
    //void OnGUI()
    //{
    //    if (Display)
    //    {
    //        GUI.contentColor = Color;
    //        GUI.Label(Rect, "FPS:" + FPS.ToString());
    //    }
    //}
}