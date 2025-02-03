using UnityEngine;
using UnityEngine.UI;

using Spells;


public class CoolDownBubble : MonoBehaviour
{
    //Cooldown UI
    [SerializeField]
    private Image fill, rune;

    [SerializeField]
    private Color col_fire, col_ice;

    [SerializeField]
    private Animator anim;

    private float cd_time, cd_dur;


    void Start()
    {
        fill.fillAmount  = 0f;
    }

    void Update()
    {
        float ratio = cd_time / cd_dur;

        if(ratio < 0f)
        {
            gameObject.SetActive(false);
            rune.color  = new Color(1f, 1f, 1f, 0.5f);
        }

        if(ratio < 0.25f &&
           !anim.IsPlaying(0))
            anim.Play("Cooldown_charged");

        fill.fillAmount = ratio;

        cd_time -= Time.deltaTime;
    }

    public void Enable_Cooldown(string type, float _cd_dur, Sprite img)
    {
        cd_time = _cd_dur;
        cd_dur  = _cd_dur;

        if(type == "fire") fill.color = col_fire;
        if(type == "ice")  fill.color = col_ice;

        rune.sprite = img;
        rune.color  = new Color(1f, 1f, 1f, 0.5f);
    }
}
