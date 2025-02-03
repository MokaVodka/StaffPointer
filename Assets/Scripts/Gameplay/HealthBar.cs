using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Player hp;

    [SerializeField]
    private Image heart_rdr, fill_rdr;

    [SerializeField]
    private Sprite[] hp_bars;
    
    [SerializeField]
    private Gradient fill_cols;

    [SerializeField]
    private float fill_spd;
    private float curr_fill;
    

    void Update()
    {
        float hpRatio = hp.currentHealth/hp.maxHealth;

        if(hpRatio <= 0.25f)
            heart_rdr.sprite = hp_bars[2];
        else if(hpRatio <= 0.5f)
            heart_rdr.sprite = hp_bars[1];
        else
            heart_rdr.sprite = hp_bars[0];

        fill_rdr.color      = fill_cols.Evaluate(hpRatio);

        float curr_fill = fill_rdr.fillAmount;
        curr_fill = Mathf.Lerp(curr_fill, hpRatio, fill_spd * deltaTime());
        fill_rdr.fillAmount = curr_fill;
    }

    float deltaTime()
    {
        return Mathf.Round(Time.deltaTime * 100f) * 0.01f;
    }
}