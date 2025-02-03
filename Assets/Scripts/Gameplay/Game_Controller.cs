using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{
    //Round counter, determine winner
    //[SerializeField]
    //private Timer timer;

    [SerializeField]
    private Player hp_p1, hp_p2;

    [SerializeField]
    private FX_ScreenTransition FX_transition;

    private bool roundIsFinished = false;

    public GameObject GetWii;


    void Start()
    {
        if(Game.round == 0) Game.Init();
    }

    void Update()
    {
        try
        {
            if(Input_Ext.IsInputMode("wiimotes"))
                Game.Check_Wiimotes();
        }
        finally
        {


        if(!roundIsFinished)
        {
            if(Input_Ext.IsInputMode("wiimotes") &&
              (Input_Ext.Get_WiiPress(0, "home") || Input_Ext.Get_WiiPress(1, "home")))
                GetWii.SetActive(true);

            //Score for the person with more health
            /*
            if(timer.currentTime <= 0)
            {
                if(hp_p1.currentHealth < hp_p2.currentHealth)
                    Game.win_p2++;
                else if(hp_p2.currentHealth < hp_p1.currentHealth)
                    Game.win_p1++;

                ToScoreScreen();
            }
            */

            //Both player dies
            if(hp_p1.currentHealth <= 0 && hp_p2.currentHealth <= 0)
            {
                Game.win_p1++;
                Game.win_p2++;
                ToScoreScreen();
            }

            //Either player dies
            else if(hp_p1.currentHealth <= 0)
            {
                Game.win_p2++;
                ToScoreScreen();
            }
            else if(hp_p2.currentHealth <= 0)
            {
                Game.win_p1++;
                ToScoreScreen();
            }
        }

        }

    }

    void ToScoreScreen()
    {
        roundIsFinished = true;

        Game.round++;
        Game.sfx.SFX_Over();

        FX_transition.LoadScene("Scenes/Screen_Score");
    }
}