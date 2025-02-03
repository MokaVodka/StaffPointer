using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    //The player/controller index
    public int ID;

    //For stroke stabilize
    [Header("Shake / Jitter distance")]
    [SerializeField]
    protected float min_radius;

    [SerializeField]
    protected AnimationCurve curve_norm, curve_slow;

    //Slow
    [SerializeField]
    protected bool isSlowed = false;
    protected float slow_factor = 1;

    protected ParticleSystem ps;

    //Position calculation
    protected RectTransform trf;

    [HideInInspector]
    public  Vector3 pos;

    /*
        curr_pos   = ir pos of current frame
        prev_pos   = cursor pos of last frame
        smooth_pos = new cursor pos (with smoothing calculation)
    */
    protected Vector2 prev_pos, curr_pos, smooth_pos, velocity;

    //FX
    protected Image img;

    [SerializeField]
    protected Color col_def, col_change;


    protected void Init()
    {
        trf = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        ps  = GetComponent<ParticleSystem>();
    }

    protected void Check_OutsideBounds()
    {
        /*
            When ir is out of bounds,
            Keep cursor position inside screen and exit loop
        */

        if(curr_pos.x < 0 || curr_pos.x > 1||
           curr_pos.y < 0 || curr_pos.y > 1)
        {
            //Restrict x || y pos
            for(int i = 0; i < 2; i++)
            {
                if(curr_pos[i] < 0)  curr_pos[i] = 0;
                if(curr_pos[i] > 1)  curr_pos[i] = 1;
            }

            //Assign cursor's pos
            trf.anchorMin = trf.anchorMax = curr_pos;
            prev_pos      = curr_pos;

            return;
        }
    }

    protected void SmoothStroke()
    {
        //Move distance
        float dis = Mathf.Abs(Vector2.Distance(curr_pos, prev_pos));

        //Exit loop if distance < min distance (jitter)
        if(dis <= min_radius)
            return;
        
        //Calculate smoothing path
        else
        {
            //When dis gets big, make cursor moves faster (less drag lag)
            float smoothness = 0;

            if (!isSlowed) smoothness = curve_norm.Evaluate(dis);
            else           smoothness = curve_slow.Evaluate(dis) * slow_factor;

            //Calculate smooth pos
            smooth_pos = Vector2.SmoothDamp(prev_pos, curr_pos, ref velocity, smoothness); 

            //Assign cursor's pos
            trf.anchorMin = trf.anchorMax = smooth_pos;
            prev_pos      = smooth_pos;
            pos           = trf.position;
        }
    }

    public void Start_SlowStroke(float slow_time, float slow_mod)
    {
        StopCoroutine("Switch_SlowStroke");
        StartCoroutine(Switch_SlowStroke(slow_time, slow_mod));
    }

    protected IEnumerator Switch_SlowStroke(float slow_time, float slow_mod)
    {
        isSlowed    = true;
        slow_factor = slow_mod;

        img.color = col_change;
        ps.Play(true);

        yield return new WaitForSecondsRealtime(slow_time);

        isSlowed = false;

        img.color = col_def;
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    
    protected void Check_StopSlowFX()
    {
        //FX, in case StopCoroutine doesn't work
        if(!isSlowed)
        {
            img.color = col_def;

            if(ps.isEmitting)
                ps.Stop(isSlowed, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }


        /*

            TO DO:
            - Cursor sensitivity setting
            - Dead zones
            - Increase IR range

        */
}