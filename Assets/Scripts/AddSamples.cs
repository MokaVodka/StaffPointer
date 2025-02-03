using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using QDollar;
using TMPro;


public class AddSamples : Draw_Pad
{
    private List<Gesture> trainingSet = new List<Gesture>();

    public  TextMeshProUGUI string_output;
    public  TMP_InputField  string_input;
    private string name;

    void Start()
    {
        if(ID == 0) Game.Init();
        
        //Set up drawing area
        draw_Area_cld = GetComponent<BoxCollider2D>();
        draw_Area     = draw_Area_cld.bounds;

		//Load samples
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Runes");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

		gesturesXml = Resources.LoadAll<TextAsset>("Invalid");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    void Update()
    {
        //Drawing...
        Get_InputPos();
        Draw_Stroke();
    }

    public void Recognize()
    {
		Gesture candidate    = new Gesture(points.ToArray());
		Result gestureResult = QPointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        string runeName      = gestureResult.GestureClass;
        float confidence     = gestureResult.Score;

		string_output.text = runeName + " " + confidence;
    }

    public void AddSample()
    {
        name = string_input.text;

		if (points.Count > 0 && name != "")
        {
			string fileName = String.Format("{0}/{1}-{2}.xml", Application.dataPath + "/Resources", name, DateTime.Now.ToFileTime());

			#if !UNITY_WEBPLAYER
				GestureIO.WriteGesture(points.ToArray(), name, fileName);
			#endif

			trainingSet.Add(new Gesture(points.ToArray(), name));

			name = string_input.text = "";
		}
    }

    public void Clear()
    {
        Clear_Drawing();
        string_output.text = "";
    }
}
