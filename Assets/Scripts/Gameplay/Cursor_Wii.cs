using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Cursor_Wii : Cursor
{
    void Start()
    {
        Init();

        if(!Input_Ext.IsInputMode("wiimotes"))
            this.enabled = false;
    }

    void Update()
    {
        //Get wii's data
        Game.wii[ID].Get_Data();

        Game.wii[ID].Update_ButtonState("a");
        Game.wii[ID].Update_ButtonState("b");
        Game.wii[ID].Update_ButtonState("home");

        //Get Ir pos
        float[] pointer = Game.wii[ID].Get_IrPos();
        curr_pos.x = pointer[0];
        curr_pos.y = pointer[1];

        //Outside screen bounds shenanigans
        Check_OutsideBounds();

        //Stablize & smooth stroke
        SmoothStroke();

        //FX, in case StopCoroutine doesn't work
        Check_StopSlowFX();
    }
}