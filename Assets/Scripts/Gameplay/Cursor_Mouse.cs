using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Cursor_Mouse : Cursor
{
    void Start()
    {
        Init();

        if(!Input_Ext.IsInputMode("one_mouse"))
            this.enabled = false;
    }

    void Update()
    {
        curr_pos.x = Input.mousePosition.x / Screen.width;
        curr_pos.y = Input.mousePosition.y / Screen.height;

        //Outside screen bounds shenanigans
        Check_OutsideBounds();

        //Stablize & smooth stroke
        SmoothStroke();

        //FX, in case StopCoroutine doesn't work
        Check_StopSlowFX();
    }
}