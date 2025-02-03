using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen, readyScreen;

    [SerializeField]
    private AudioSource sfx;

    void Start()
    {
        //Initialize global vars
        if(Game.menu_isFirstTime)
            Game.Init();
        else
            Game.Reset_GlobalVars();
    }

    void SFX_Play()
    {
        sfx.Play();
    }

    void TransitScreen(GameObject from, GameObject to)
    {
        SFX_Play();
        to.SetActive(true);
        from.SetActive(false);
    }

    public void Return(GameObject from)
    {
        TransitScreen(from, mainScreen);
    }

    public void goToScreen(GameObject to)
    {
        TransitScreen(mainScreen, to);
    }

    public void Play_Game()
    {
        SFX_Play();
        readyScreen.SetActive(true);
    }

    public void QuitGame()
    {
        SFX_Play();
        Application.Quit();
    }
}