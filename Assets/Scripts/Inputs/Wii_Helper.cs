using System;
using System.Collections;
using System.Collections.Generic;

using WiimoteApi;

//Extention / helper class
public static class Wii_Helper
{
    public static void Init(this Wiimote wii)
    {
        if(wii.Is_Exist())
            wii.SetupIRCamera(IRDataType.BASIC);
    }

    public static void Get_Data(this Wiimote wii)
    {
        if(wii != null)
        {
            //Get wii data
            int ret;
            do
            {
                ret = wii.ReadWiimoteData();

            } while (ret > 0);
        }
    }

    public static float[] Get_IrPos(this Wiimote wii) 
    {
        return wii.Ir.GetPointingPosition();
    }

    public static bool Is_Exist(this Wiimote wii)
    {
        return wii.hidapi_handle != IntPtr.Zero;
    }

    public static bool Is_Pressed(this Wiimote wii, string button_name)
    {
        string button = button_name.ToLower();

        switch(button)
        {
            case "a"   : return wii.Button.a;
            case "b"   : return wii.Button.b;
            case "home": return wii.Button.home;
            default:     return false;
        }
    }

    public static void CleanUp(this Wiimote wii)
    {
        //Clean up wiimote
        WiimoteManager.Cleanup(wii);
        wii = null;
    }
}
