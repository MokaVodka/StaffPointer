//Nhi's Magic Recgonizer

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using Spells;

using QDollar;

public class Draw_Pad : MonoBehaviour
{
    //For testing
    public    int ID;

    //Cursor reference
    [SerializeField]
    protected GameObject _cursor;
    protected Cursor cursor;

    //For drawing
    [SerializeField]
    protected Transform draw_Stroke;

    protected Bounds draw_Area;
	protected BoxCollider2D draw_Area_cld;

    protected List<Point> points = new List<Point>();
    protected int strokeId = -1;

    protected Vector3 input_Pos, drawing_Pos;

    protected int vertexCount = 0;

    protected List<LineRenderer> draw_Lines    = new List<LineRenderer>();
    private   List<LineRenderer> draw_Lines_FX = new List<LineRenderer>();
    protected LineRenderer curr_draw_Lines;

    //Timer and accuracy
    [SerializeField]
    private float time, recTime, percision;

    //Spell dictionary
    [SerializeField]
    private SpellDict spells;
    private List<string> runes_InCD = new List<string>();

    //Spawner
    [SerializeField]
    private SpellSpawner spawner;

    //UI spells array
    [SerializeField]
    private List<GameObject> CDBubbles = new List<GameObject>();


    void Start()
    {
        //Set up drawing area
        draw_Area_cld = GetComponent<BoxCollider2D>();
        draw_Area     = draw_Area_cld.bounds;

        //Get cursor script
        Cursor[] curs = _cursor.GetComponents<Cursor>();

        for(int i = 0; i < curs.Length; i++)
        {
            cursor = curs[i];
            if(cursor.enabled == true)
                return;
        }
    }

    void Update()
    {
        //Update cooldown list
        foreach (string rune in runes_InCD)
        {
            if(Time.time > spells.dict[rune].nextFireTime)
            {
                runes_InCD.Remove(rune);
                runes_InCD.TrimExcess();
            }
        }

        //Drawing...
        Get_InputPos();
        Draw_Stroke();

		//After finish a stroke, start timer
        if (draw_Lines.Count > 0)
        {
            if(Input_Ext.Get_MouseFree(ID) ||
               Input_Ext.Get_WiiUp(ID, "b"))
            {
                if(time < recTime)
                    time += Time.deltaTime;
            }

            else
                time = 0;

            //After x sec drawing inactivity, recognize
            if(time >= recTime)
            {
                Recognize();
                time = 0;
            }
		}
    }


    protected void Get_InputPos()
    {
        if(Input_Ext.Get_MouseHold(ID) ||
           Input_Ext.Get_WiiDown(ID, "b"))
            input_Pos = cursor.pos;
        
        //Transform the z pos to be visible to the camera
        input_Pos.z = draw_Area.max.z;
        drawing_Pos = input_Pos;
        drawing_Pos.z -= 0.5f;
    }

    protected void Draw_Stroke()
    {
        if(draw_Area.Contains(input_Pos))
        {
            if(Input_Ext.Get_MouseDown(ID) ||
               Input_Ext.Get_WiiPress(ID, "b"))
            {
                ++strokeId;

                Transform tmpGesture = Instantiate(draw_Stroke, transform.position, transform.rotation) as Transform;
                curr_draw_Lines = tmpGesture.GetComponent<LineRenderer>();

                draw_Lines.Add(curr_draw_Lines);

                vertexCount = 0;
            }

            if(Input_Ext.Get_MouseHold(ID) ||
               Input_Ext.Get_WiiDown(ID, "b"))
            {
                points.Add(new Point(input_Pos.x, -input_Pos.y, strokeId));

                curr_draw_Lines.SetVertexCount(++vertexCount);
                curr_draw_Lines.SetPosition(vertexCount - 1, drawing_Pos);
            }
        }        
    }


    void Recognize()
    {
		Gesture candidate    = new Gesture(points.ToArray());
		Result gestureResult = QPointCloudRecognizer.Classify(candidate, Game.runeSet.ToArray());

        string runeName      = gestureResult.GestureClass;
        float confidence     = gestureResult.Score;

		print(runeName + " " + confidence);

        //Do spell

        /*
            Lower confidence = more accurate
            Break function if confidence is high
            Break function if rune is not in dictionary
            Break function if rune is in cooldown
        */
        if(confidence >= percision ||
           !spells.dict.ContainsKey(runeName) ||
           runes_InCD.Contains(runeName))
        {
            Animate_DrawLine("fail");
            Clear_Drawing();

            StartCoroutine(CastFail());
            return;
        }

        //If the rune name exist in the dictionary
        //AND not in cooldown
        if(spells.dict.ContainsKey(runeName) &&
           !runes_InCD.Contains(runeName))
           {
                Animate_DrawLine(spells.dict[runeName].type);
                Clear_Drawing();

                StartCoroutine(CastSpell_Start(runeName));
           }
    }


    protected void Clear_Drawing()
    {
        //Reset vars
        strokeId   = -1;
        points.Clear();

        Clear_DrawLineList(draw_Lines);
    }

    protected void Clear_DrawLineList(List<LineRenderer> line_list)
    {
        foreach (LineRenderer lineRenderer in line_list)
        {
            lineRenderer.SetVertexCount(0);
            Destroy(lineRenderer.gameObject);
        }

        line_list.Clear();
    }

    void Animate_DrawLine(string type)
    {
        for(int i = 0; i < draw_Lines.Count; i++)
        {
            //Copy lines
            Transform tmpGesture = Instantiate(draw_Stroke, transform.position, transform.rotation) as Transform;
            var line = tmpGesture.GetComponent<LineRenderer>();

            line.positionCount = draw_Lines[i].positionCount;

            Vector3[] newPos = new Vector3[draw_Lines[i].positionCount];
            draw_Lines[i].GetPositions(newPos);
            line.SetPositions(newPos);

            draw_Lines_FX.Add(line);

            //Start Animate Coroutine
            var anim = line.gameObject.GetComponent<Draw_Line>();
            anim.Animate_Color(type);
        }
    }

    IEnumerator CastFail()
    {
        bool isFinished = false;

        while(!isFinished)
        {
            foreach(var line in draw_Lines_FX)
            {
                var anim = line.gameObject.GetComponent<Draw_Line>();
                isFinished = anim.isFinished;
            }

            yield return null;
        }

        Clear_DrawLineList(draw_Lines_FX);
    }
    
    IEnumerator CastSpell_Start(string runeName)
    {
        bool isFinished = false;

        while(!isFinished)
        {
            foreach(var line in draw_Lines_FX)
            {
                var anim   = line.gameObject.GetComponent<Draw_Line>();
                isFinished = anim.isFinished;
            }

            yield return null;
        }

        Clear_DrawLineList(draw_Lines_FX);

        CastSpell(runeName);
    }

    void CastSpell(string runeName)
    {
        //Instantiate the prefab associate with the rune name
        GameObject spell_ball = Instantiate(spells.dict[runeName].spell_ball,
                                            spawner.transform.position,
                                            Quaternion.identity);

        Spell spell = spell_ball.GetComponent<Spell>();

        //Spell set up
        if(ID == 0) spell.dir = 1;
        else        spell.dir = -1;
            
        spell.Init();

        //Play spawn animation
        spawner.Activate_Slot(spell_ball, spells.dict[runeName].type);


        //Put rune into cooldown

        //Stop if rune is easter egg
        if (runeName == "heart" || runeName == "go")
            return;

        //Next fire time = current time + cooldown duration
        spells.dict[runeName].nextFireTime = Time.time + spell.cd_dur;
        runes_InCD.Add(runeName);

        //loop to find the inactive bubble
        foreach (GameObject CD_bubble in CDBubbles)
        {
            if (!CD_bubble.activeInHierarchy)
            {
                CD_bubble.SetActive(true);

                CoolDownBubble cd_bubble = CD_bubble.GetComponent<CoolDownBubble>();
                cd_bubble.Enable_Cooldown(spells.dict[runeName].type, spell.cd_dur, spells.dict[runeName].icon);

                break;
            }
        }
    }
}