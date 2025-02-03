using System;

using UnityEngine;

public class Shield : MonoBehaviour
{
    public int ID;

    [SerializeField]
    private string key;

    [SerializeField]
    private GameObject shield;

    [SerializeField]
    private new Camera_Controller camera;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private AudioSource sfx;

    //[SerializeField]
    //private AudioClip[] sfx_clips;

    [SerializeField]
    private float shield_dur, cd_duration;
    private float cd_time;


    void Update()
    {
        if(camera.canShake)
            camera.CameraShake();

        //When it's not in cooldown
        if (cd_time <= 0)
        { 
            //Press the button
            if (Input_Ext.Get_KeyDown(key) ||
                Input_Ext.Get_WiiPress(ID, "a"))
            {
                if (cd_time >= 0) return;

                cd_time = cd_duration;
                shield.SetActive(true);

                camera.canShake = true;
                anim.SetTrigger("Reflect");
                //sfx.clip = sfx_clips[UnityEngine.Random.Range(0, sfx_clips.Length)];
                sfx.Play();

                Invoke("Deactivate", shield_dur); //deactivate after a period of time
            }
        }

        cd_time -= Time.deltaTime;   
    }

    void Deactivate()
    {
        shield.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Magic")
        {
            cd_time = 0f;
        }
    }
}
