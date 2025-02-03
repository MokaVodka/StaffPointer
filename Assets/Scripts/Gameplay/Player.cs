using System.Collections;

using UnityEngine;


public class Player : MonoBehaviour
{
    public int ID;

    public Cursor cursor;
    
    public float currentHealth, maxHealth;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject ps_burn, ps_freeze;

    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private AudioClip hit_fire, hit_ice;

    void Start()
    {
        currentHealth = maxHealth;

        //Set man or woman animation
        anim.SetLayerWeight(ID + 1, 0.6f);

        ps_burn.SetActive(false);
        ps_freeze.SetActive(false);
    }

    void Update()
    {
        Update_Health();

        if (currentHealth == 0)
            gameObject.SetActive(false);     

        //Animate
        if(Input_Ext.Get_MouseDown(ID) ||
           Input_Ext.Get_WiiPress(ID, "b"))
            anim.SetTrigger("Draw_Start");

        if(Input_Ext.Get_MouseUp(ID) ||
           Input_Ext.Get_WiiUp(ID, "b"))
           {
                if(anim.IsPlayingLoop(ID + 1, "Draw"))
                    anim.SetTrigger("Draw_End");
                else
                    anim.ResetTrigger("Draw_End");
           }
    }

    void Update_Health()
    {
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        if (currentHealth <= 0f)
            currentHealth = 0;
    }


    void OnCollisionEnter2D(Collision2D collision)   
    {
        int layer = collision.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Spell Fire"))
            sfx.clip = hit_fire;

        else if (layer == LayerMask.NameToLayer("Spell Ice"))
            sfx.clip = hit_ice;

        else
            sfx.clip = null;

        sfx.Play();
    }

    public void Enable_FX(string triggerName)
    {
        anim.SetLayerWeight(anim.GetLayerIndex("FX"), 0.6f);
        anim.SetTrigger(triggerName);

        Set_Active_PS(triggerName, true);
    }

    public void Set_Active_PS(string type, bool isActive)
    {
        if(type == "burn")   ps_burn.SetActive(isActive);
        if(type == "freeze") ps_freeze.SetActive(isActive);
    }

    public void Disable_FX(float time)
    {
        StopCoroutine("StopFX");
        StartCoroutine(StopFX(time));
    }

    IEnumerator StopFX(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        anim.SetLayerWeight(anim.GetLayerIndex("FX"), 0f);
        ps_burn.SetActive(false);
        ps_freeze.SetActive(false);
    }
}