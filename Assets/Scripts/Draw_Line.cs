using System;
using System.Collections;

using UnityEngine;

public class Draw_Line : MonoBehaviour
{
    [HideInInspector]
    public bool isFinished = false;

    [HideInInspector]
    public float elaspTime = 0f;

    [SerializeField]
    private float anim_spd;

    [SerializeField]
    private Color fire_col, ice_col, fail_col, heart_col, go_col;
    private Color targ_col;

    private LineRenderer line;
    private GradientColorKey[] colKeys;
    private GradientAlphaKey[] alphKeys;


    public void Animate_Color(string type)
    {
        switch(type)
        {
            case "fire" : targ_col = fire_col;  break;
            case "ice"  : targ_col = ice_col;   break;
            case "heart": targ_col = heart_col; break;
            case "go"   : targ_col = go_col;    break;
            case "fail" : targ_col = fail_col;  break;

            default: return;
        }
        
        line     = GetComponent<LineRenderer>();
        colKeys  = line.colorGradient.colorKeys;
        alphKeys = line.colorGradient.alphaKeys;

        StartCoroutine(ChangeColor());
    }


    IEnumerator ChangeColor()
    {
        while(!isFinished)
        {
            //Change color
            for(int i = 0; i < colKeys.Length; i++)
            {
                var curr_col = new Color(colKeys[i].color.r,
                                        colKeys[i].color.g,
                                        colKeys[i].color.b);

                curr_col = Color.Lerp(curr_col, targ_col, anim_spd * RoundFloat(Time.deltaTime));
                colKeys[i].color = curr_col;

                isFinished = IsSameCol(colKeys[i].color, targ_col);
            }
            
            Gradient _gradient = new Gradient();
            _gradient.colorKeys = colKeys;
            _gradient.alphaKeys = alphKeys;

            line.colorGradient = _gradient;

            elaspTime += Time.deltaTime;

            yield return null;
        }
    }

    bool IsSameCol(Color current, Color target)
    {
        return
            EqualFloat(current.r, target.r) &&
            EqualFloat(current.g, target.g) &&
            EqualFloat(current.b, target.b);
    }

    bool EqualFloat(float _value1, float _value2)
    {
        float value1 = RoundFloat(_value1);
        float value2 = RoundFloat(_value2);

        float difference = 0.05f;
        return Mathf.Abs(value1 - value2) <= difference;
    }

    float RoundFloat(float _float)
    {
        return Mathf.Round(_float * 100f) * 0.01f;
    }
}
