using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class Screen_Win : MonoBehaviour
{
    [SerializeField]
    private string[] keys;

    [SerializeField]
    private bool isWinScreen;

    [SerializeField]
    private TextMeshProUGUI win;

    [SerializeField]
    private GameObject[] player;
    private Cursor[] cursor = new Cursor[2];

    [SerializeField]
    private Animator[] anim;

    [SerializeField]
    private FX_ScreenTransition FX_transition;
    private bool isStarted = false;


    void Start()
    {
        if(isWinScreen)
        {
            if(Game.win_p1 > Game.win_p2)
            {
                win.text = "Player 1 \nwins!";
                player[0].SetActive(true);
            }
            else if (Game.win_p1 < Game.win_p2)
            {
                win.text = "Player 2 \nwins!";
                player[1].SetActive(true);
            }
        }

        for(int i = 0; i < 2; i++)
        {
            anim[i].SetBool("isWaiting", true);
            anim[i].SetLayerWeight(i + 1, 0.6f);
        }
    }

    void Update()
    {
        if(!isStarted &&
          (
            (Input_Ext.Get_WiiPress(0, "a") || Input_Ext.Get_WiiPress(1, "a")) ||
            (Input_Ext.Get_KeyDown(keys[0]) || Input_Ext.Get_KeyDown(keys[1]))
          )
          )
        {
            isStarted = true;

            for(int i = 0; i < 2; i++)
            {
                anim[i].SetBool("isWaiting", false);
                anim[i].SetTrigger("Reflect");
            }

            FX_transition.LoadScene("Scenes/Main_Menu");
        }
    }
}