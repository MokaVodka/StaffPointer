using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Button_WiiExt : MonoBehaviour
{
    private Cursor[] curs = new Cursor[2];
    private RectTransform[] cur_trfs = new RectTransform[2];
    private Rect[] cur_rects = new Rect[2];

    private Canvas canvas;

    private Rect area;

    private Graphic graphic;

    [SerializeField]
    private Color32 col_default, col_hover;

    [SerializeField]
    private float spd_colChange;

    //Callback functions
    [SerializeField]
    private UnityEvent method_ToCall;


    void Start()
    {
        canvas = transform.root.gameObject.GetComponent<Canvas>();

        //Set up button rect
        RectTransform area_trf = GetComponent<RectTransform>();

        float x = area_trf.anchorMin.x * canvas.pixelRect.width;
        float y = area_trf.anchorMin.y * canvas.pixelRect.height;
        float w = area_trf.rect.width * canvas.scaleFactor;
        float h = area_trf.rect.height * canvas.scaleFactor;

        area = new Rect(x, y, w, h);

        graphic = GetComponent<Graphic>();

        curs         = Game.Get_Cursor();
        cur_trfs[0]  = curs[0].gameObject.GetComponent<RectTransform>();
        cur_trfs[1]  = curs[1].gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Get_InputPos();

        //Cursor is outside of click area
        if(graphic.color != col_default)
            Change_Color(col_default, spd_colChange);

        //Cursor is inside of click area
        if (isRectCollide(cur_rects[0]) || isRectCollide(cur_rects[1]))
        {
            Debug.Log("Something overlaped, Button: " + gameObject.name);
            if(graphic.color != col_hover)
                Change_Color(col_hover, spd_colChange);
            
            //Check for click input
            if(Input_Ext.Get_WiiPress(0, "b") ||
               Input_Ext.Get_WiiPress(1, "b"))
                On_Click();
        }
    }


    void Get_InputPos()
    {
        if(Input_Ext.IsInputMode("wiimotes"))
        {
            for(int i = 0; i < 2; i++)
            {
                float x = cur_trfs[i].anchorMin.x * canvas.pixelRect.width;
                float y = cur_trfs[i].anchorMin.y * canvas.pixelRect.height;
                float w = cur_trfs[i].rect.width * canvas.scaleFactor;
                float h = cur_trfs[i].rect.height * canvas.scaleFactor;

                cur_rects[i] = new Rect(x, y, w, h);
            }
        }
    }

    bool isRectCollide(Rect other)
    {
        return (area.x < other.x + other.width &&
            area.x + area.width > other.x &&
            area.y < other.y + other.height &&
            area.y + area.height > other.y);
    }

    void Change_Color(Color32 col_targ, float spd)
    {
        graphic.color = Color32.Lerp(graphic.color, col_targ, spd);
    }

    void On_Click()
    {
        //Call assigned function
        method_ToCall?.Invoke();

        //Reset color
        graphic.color = col_default;
    }
}
