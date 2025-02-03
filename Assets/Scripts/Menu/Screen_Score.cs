using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class Screen_Score : MonoBehaviour
{
    [SerializeField]
    private string[] keys;

    [SerializeField]
    private TextMeshProUGUI score, round;

    [SerializeField]
    private Animator[] anim;

    [SerializeField]
    private FX_ScreenTransition FX_transition;
    private bool isStarted = false;


    void Start()
    {
        score.text = Game.win_p1 + " - " + Game.win_p2;
        round.text = "Round " + Game.round;

        for(int i = 0; i < 2; i++)
        {
            anim[i].SetBool("isWaiting", true);
            anim[i].SetLayerWeight(i + 1, 0.6f);
        }
    }

    void Update()
    {
        //Bool to prevent FX transition playing multiple times
        if(!isStarted)
        {
            //Move to win / tied screen
            if(Game.round >= 3)
            {
                if(Game.win_p1 != Game.win_p2)
                    FX_transition.LoadScene("Scenes/Screen_Win");
                else
                    FX_transition.LoadScene("Scenes/Screen_Tie");

                isStarted = true;
            }

            //Move to win if either player has a score difference of 2
            else if(Mathf.Abs(Game.win_p1 - Game.win_p2) >= 2)
            {
                FX_transition.LoadScene("Scenes/Screen_Win");            
                isStarted = true;
            }

            //Move to next round
            else if (
                        (Input_Ext.Get_WiiPress(0, "a") || Input_Ext.Get_WiiPress(1, "a")) ||
                        (Input_Ext.Get_KeyDown(keys[0]) || Input_Ext.Get_KeyDown(keys[1]))
                    )
            {
                for(int i = 0; i < 2; i++)
                {
                    anim[i].SetBool("isWaiting", false);
                    anim[i].SetTrigger("Reflect");
                }

                FX_transition.LoadScene("Scenes/Main_Game");
                isStarted = true;
            }
        }
    }
}