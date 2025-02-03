using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        //Manage volume of sfx and music
        Set_Volume( "MASTER",
                    PlayerPrefs.GetFloat(Get_MixType("MASTER"), 1.0f) );

        Set_Volume( "MUSIC",
                    PlayerPrefs.GetFloat(Get_MixType("MUSIC"), 0.75f) );

        Set_Volume( "SFX",
                    PlayerPrefs.GetFloat(Get_MixType("SFX"), 0.75f) );
    }

    public void Set_Volume(string type, float value)
    {
        PlayerPrefs.SetFloat(Get_MixType(type), value);
        mixer.SetFloat( Get_MixType(type), Mathf.Log10(value) * 20 );
    }

    string Get_MixType(string type)
    {
        switch(type)
        {
            case "MASTER": return "Aud_Vol_Master";
            case "MUSIC":  return "Aud_Vol_Music"; 
            case "SFX":    return "Aud_Vol_SFX";

            default: return "null";
        }
    }
}
