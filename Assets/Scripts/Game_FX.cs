using UnityEngine;

public class Game_FX : MonoBehaviour
{
    [SerializeField]
    private Animator[] fx_cancel, fx_crit, fx_freeze;

    [SerializeField]
    private ParticleSystem[] fx_go;

    [SerializeField]
    private Color ice_col, fire_col;

    void Start()
    {
        Game.fx = this;
    }


    public Animator Get_Animator(string fx_name)
    {
        Animator[] anims;

        switch(fx_name)
        {
            case "cancel": anims = fx_cancel;    break;
            case "crit"  : anims = fx_crit;      break;
            case "freeze": anims = fx_freeze;    break;
            default      : return null;
        }

        foreach(Animator anim in anims)
        {
            if(!anim.IsPlaying(0, fx_name))
                return anim;
        }

        return null;
    }

    public ParticleSystem Get_PS(string fx_name)
    {
        ParticleSystem[] pss;

        switch(fx_name)
        {
            case "go" : pss = fx_go;        break;
            default   : return null;
        }

        foreach(ParticleSystem ps in pss)
        {
            if(!ps.isPlaying)
                return ps;
        }

        return null;
    }


    public void PlayFX(Animator anim, string fx_name)
    {
        anim.SetTrigger(fx_name);
    }

    public void PlayCancelPS(ParticleSystem ps, string type)
    {
        Color ps_col = type == "fire" ? fire_col : 
                       type == "ice"  ? ice_col  : 
                       new Color(1, 1, 1, 1);

        var mainModule = ps.main;
        mainModule.startColor = ps_col;

        ps.Play();
    }

    public void PlayPS(ParticleSystem ps)
    {
        ps.Play();
    }
}
