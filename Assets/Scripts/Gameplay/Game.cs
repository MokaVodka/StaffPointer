using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using WiimoteApi;
using QDollar;

//Data container and helper functions to modify data
public class Game : MonoBehaviour
{
    //Input mode
    public enum InputMode { ONE_MOUSE, WIIMOTES, TWO_MICE };
    public static InputMode inputMode = InputMode.ONE_MOUSE;

    //Wiimotes
    public static List<Wii> wii   = new List<Wii>();

    //Runes
    public static List<Gesture> runeSet = new List<Gesture>();

    //Rounds
    public static int round, win_p1, win_p2;

    //SFX, FX manager
    public static Game_SFX sfx;
    public static Game_FX  fx;

    //Times menu scene is accessed
    public static bool menu_isFirstTime = true;

    public static void Init()
    {
        /*
            1. Get the wiis, set them up and store them to the public static Game.wii
            2. Load the runes, and store them to a list
            3. Set up Global variables
        */

        //Get Wiimotes
        try
        {
            WiimoteManager.FindWiimotes();

            wii.Add(Get_Wiimote(0));
            wii.Add(Get_Wiimote(1));

            inputMode = InputMode.WIIMOTES;
        }

        finally
        {
            //Load rune sets
            Get_Runes();

            //Global vars set
            Reset_GlobalVars();

            menu_isFirstTime = false;
        }
    }

    public static void Get_Wiimotes()
    {
        //Get Wiimotes
        WiimoteManager.FindWiimotes();

        wii[0] = Get_Wiimote(0);
        wii[1] = Get_Wiimote(1);
    }

    public static Wii Get_Wiimote(int ID)
    {
        Wii wiimote = new Wii();

        wiimote.ID  = ID;
        wiimote.wiimote = WiimoteManager.Wiimotes
                          [PlayerPrefs.GetInt("wii_" + ID.ToString(), ID)];
        wiimote.Init();

        return wiimote;
    }

    public static void Check_Wiimotes()
    {
        for(int i = 0; i < 2; i++)
        {
            if(wii[i] == null || wii[i].Check_CannotGetIRpos(0.5f))
            {
                WiimoteManager.FindWiimotes();
                wii[i] = Get_Wiimote(i);
            }
        }
    }

    public static void Get_Runes()
    {
        //Clean rune list
        runeSet.Clear();
        runeSet.TrimExcess();

        TextAsset[] runesXml;

        //Load runeSet
        runesXml = Resources.LoadAll<TextAsset>("Runes");
        foreach (TextAsset runeXml in runesXml)
        {
            //Debug.Log(AssetDatabase.GetAssetPath(runeXml));
            runeSet.Add(GestureIO.ReadGestureFromXML(runeXml.text));
        }

        runesXml = Resources.LoadAll<TextAsset>("Invalid");
        foreach (TextAsset runeXml in runesXml)
        {
            //Debug.Log(AssetDatabase.GetAssetPath(runeXml));
            runeSet.Add(GestureIO.ReadGestureFromXML(runeXml.text));
        }
    }

    public static Cursor[] Get_Cursor()
    {
        return FindObjectsOfType<Cursor>();
    }

    public static void CleanUp()
    {
        for(int i = 0; i < wii.Count; i++)
            wii[i].CleanUp();
    }

    public static void Reset_GlobalVars()
    {
        round  = 0;
        win_p1 = win_p2 = 0;
    }
}
