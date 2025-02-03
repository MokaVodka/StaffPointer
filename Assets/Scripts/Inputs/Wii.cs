using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using WiimoteApi;

//Wiimote wraper class
public class Wii
{
    //The index of the wii the script references
    public int ID;
    public Wiimote wiimote;

    //Wii button state
    public enum State {PRESS, HOLD, UP};

    public struct Button_State
    {
        public State  prev_state, curr_state;

        public bool isPress() { return this.curr_state == State.PRESS; }
        public bool isHold()  { return this.curr_state == State.HOLD; }
        public bool isDown()  { return this.curr_state != State.UP; }
        public bool isUp()    { return this.curr_state == State.UP; }
    }

    public Button_State A_State    = new Button_State();
    public Button_State B_State    = new Button_State();
    public Button_State Home_State = new Button_State();

    //IR pos
    private Vector2 prev_IRpos, curr_IRpos;
    private float timeSinceNoIRChange;

    public Wii()
    {
        ID      = 0;
        wiimote = null;

        A_State    = new Button_State();
        B_State    = new Button_State();
        Home_State = new Button_State();
    }

    public void Init()
    {
        wiimote.Init();
    }

    public void Get_Data()
    {
        wiimote.Get_Data();
    }

    public float[] Get_IrPos() 
    {
        return wiimote.Get_IrPos();
    }

    public bool Is_Exist()
    {
        return wiimote.Is_Exist();
    }

    public bool Is_Pressed(string button_name)
    {
        return wiimote.Is_Pressed(button_name);
    }

    public void CleanUp()
    {
        wiimote.CleanUp();
    }

    public void Update_ButtonState(string button_name)
    {
        Button_State buttonState = new Button_State();

        string button = button_name.ToLower();
        
        switch(button)
        {
            case "a"   : buttonState = A_State;     break;
            case "b"   : buttonState = B_State;     break;
            case "home": buttonState = Home_State;  break;
            default:     return;
        }

        bool isDown = Is_Pressed(button);

        if(isDown)
        {
            switch(buttonState.prev_state)
            {
                case State.UP:
                    buttonState.curr_state = State.PRESS;
                    break;
                case State.PRESS:
                    buttonState.curr_state = State.HOLD;
                    break;
                default:
                    break;
            }

        }
        else
            buttonState.curr_state = State.UP;

        buttonState.prev_state = buttonState.curr_state;
    }

    //Check if the IR pos is still being received
    public bool Check_CannotGetIRpos(float maxTimeOut)
    {
        float[] pointer = Get_IrPos();
        curr_IRpos.x = pointer[0];
        curr_IRpos.y = pointer[1];

        //If the IR pos doesn't change, increase timer
        if(curr_IRpos == prev_IRpos)
            timeSinceNoIRChange += Time.deltaTime;
        else
            timeSinceNoIRChange = 0;

        prev_IRpos = curr_IRpos;

        //Confirm that IR did not change after some time
        return timeSinceNoIRChange >= maxTimeOut;
    }
}
