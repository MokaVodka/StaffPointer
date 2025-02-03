using UnityEngine;

public class Game_SFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource src_over;

    [SerializeField]
    private AudioSource[] src_cancel, src_crit;


    void Start()
    {
        Game.sfx = this;
    }

    public void Mute_Everything()
    {
        var aud_srcs = FindObjectsOfType<AudioSource>();

        foreach(var src in aud_srcs)
        {
            src.mute = true;
        }
    }

    public void SFX_Over()
    {
        Mute_Everything();

        //Play horn
        src_over.mute = false;
        src_over.Play();
    }

    public void SFX_Cancel()
    {
        foreach(AudioSource aud_src in src_cancel)
        {
            if(!aud_src.isPlaying)
            {
                aud_src.Play();
                break;
            }
        }
    }

    public void SFX_Crit()
    {
        foreach(AudioSource aud_src in src_crit)
        {
            if(!aud_src.isPlaying)
            {
                aud_src.Play();
                break;
            }
        }
    }

}