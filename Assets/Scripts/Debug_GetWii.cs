using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Debug_GetWii : MonoBehaviour
{
    public Image[] wii1_ports, wii2_ports;


    void OnEnable()
    {
        PortButtonColor();
    }

    void PortButtonColor()
    {
        for(int i = 0; i<4; i++)
        {
            if(i == PlayerPrefs.GetInt("wii_1", 0))
                wii1_ports[i].color = Color.yellow;
            else 
                wii1_ports[i].color = Color.white;

            if(i == PlayerPrefs.GetInt("wii_2", 1))
                wii2_ports[i].color = Color.yellow;
            else 
                wii2_ports[i].color = Color.white;
        }        
    }

    public void Set_Wii1(int index)
    {
        PlayerPrefs.SetInt("wii_1", index);
        PortButtonColor();
    }

    public void Set_Wii2(int index)
    {
        PlayerPrefs.SetInt("wii_2", index);
        PortButtonColor();
    }

    public void Set_Wiimotes()
    {
        //Reset & Set new wiimotes
        Game.wii[0] = Game.wii[1] = null;
        Game.Get_Wiimotes();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
