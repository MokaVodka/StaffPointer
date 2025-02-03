using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_PlayersReady : MonoBehaviour
{
    [SerializeField]
    private string[] keys;

    [SerializeField]
    private Animator[] anim;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Timer timer;
    private bool isStarted = false;
    private bool[] ready = new bool[2];

    [SerializeField]
    private FX_ScreenTransition FX_transition;


    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            anim[i].SetBool("isWaiting", true);
            anim[i].SetLayerWeight(i + 1, 0.6f);

            ready[i] = false;
        }
    }


    void Update()
    {
        if(timer != null)
        {
            if(timer.currentTime > 0)
                Get_Input();
            else
            {
                gameObject.SetActive(false);
                menu.SetActive(true);
            }
        }

        else
            Get_Input();
    }

    void OnEnable()
    {
        timer.Reset();
        menu.SetActive(false);
    }

    void Get_Input()
    {
        for(int i = 0; i < 2; i++)
        {
            if(
               !ready[i] &&
               (Input_Ext.Get_WiiPress(i, "a") ||
               Input_Ext.Get_KeyDown(keys[i]))
              )
            {
                ready[i] = true;

                anim[i].SetBool("isWaiting", false);
                anim[i].SetTrigger("Reflect");
            }
        }

        if(!isStarted && ready[0] && ready[1])
        {
            isStarted = true;
            FX_transition.LoadScene("Scenes/Main_Game");
        }
    }
}