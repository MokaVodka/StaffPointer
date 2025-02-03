using UnityEngine;
using TMPro;

public class PreBattle : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Draw_Pad[] drawpads;

    [SerializeField]
    private Shield[] shields;

    public void Enable_Prebattle()
    {
        text.text = "ready";

        drawpads[0].enabled = drawpads[1].enabled =
        shields[0].enabled  = shields[1].enabled  = 
            false;
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void Play_SFX()
    {
        sfx.Play();
    }

    public void Disable_Prebattle()
    {
        //timer.canStart = true;
        drawpads[0].enabled = drawpads[1].enabled =
        shields[0].enabled  = shields[1].enabled  = 
            true;

        gameObject.SetActive(false);
    }
}
