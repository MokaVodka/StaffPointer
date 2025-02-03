using UnityEngine;

//Input extention class
public class Input_Ext
{
    //Input mode shortcuts
    private Game.InputMode oneMouse = Game.InputMode.ONE_MOUSE;
    private Game.InputMode wiimotes = Game.InputMode.WIIMOTES;
    private Game.InputMode twoMice  = Game.InputMode.TWO_MICE;


    //Get input mode
    public static bool IsInputMode(string inputMode_name)
    {
        string inputMode = inputMode_name.ToLower();

        switch(inputMode)
        {
            case "one_mouse": return Game.inputMode == Game.InputMode.ONE_MOUSE;
            case "wiimotes" : return Game.inputMode == Game.InputMode.WIIMOTES;
            case "two_mice" : return Game.inputMode == Game.InputMode.TWO_MICE;
            default: return false;
        }
    }


    //One mouse inputs
    public static bool Get_MouseDown(int ID)
    {
        return
            IsInputMode("one_mouse") &&
            Input.GetMouseButtonDown(ID);
    }

    public static bool Get_MouseUp(int ID)
    {
        return
            IsInputMode("one_mouse") &&
            Input.GetMouseButtonUp(ID);
    }

    public static bool Get_MouseHold(int ID)
    {
        return
            IsInputMode("one_mouse") &&
            Input.GetMouseButton(ID);
    }

    public static bool Get_MouseFree(int ID)
    {
        return
            IsInputMode("one_mouse") &&
            !Input.GetMouseButton(ID);
    }


    public static bool Get_KeyDown(string key)
    {
        return
            IsInputMode("one_mouse") &&
            Input.GetKeyDown(key);
    }


    //Wiimote inputs
    public static Wii.Button_State Get_WiiButtonState(int ID, string button_name)
    {
        Wii.Button_State buttonState = new Wii.Button_State();
        
        string button = button_name.ToLower();

        switch(button)
        {
            case "a"   : buttonState = Game.wii[ID].A_State;     break;
            case "b"   : buttonState = Game.wii[ID].B_State;     break;
            case "home": buttonState = Game.wii[ID].Home_State;  break;
            default:     break;
        }

        return buttonState;
    }

    public static bool Get_WiiPress(int ID, string button_name)
    {
        return
            IsInputMode("wiimotes") &&
            Get_WiiButtonState(ID, button_name).isPress();
    }

    public static bool Get_WiiHold(int ID, string button_name)
    {
        return
            IsInputMode("wiimotes") &&
            Get_WiiButtonState(ID, button_name).isHold();
    }

    public static bool Get_WiiUp(int ID, string button_name)
    {
        return
            IsInputMode("wiimotes") &&
            Get_WiiButtonState(ID, button_name).isUp();
    }

    public static bool Get_WiiDown(int ID, string button_name)
    {
        return
            IsInputMode("wiimotes") &&
            Get_WiiButtonState(ID, button_name).isDown();
    }
}