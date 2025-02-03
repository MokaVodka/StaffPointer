using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FX_ScreenTransition : MonoBehaviour
{
    private SpriteRenderer img;
    private Animator anim;

    [SerializeField]
    private float wait_time, transit_time;

    void Start()
    {
        img  = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        img.enabled = true;
    }

    public void LoadScene(string scene_name)
    {
        StartCoroutine(Load(scene_name));
    }

    private IEnumerator Load(string scene_name)
    {
        yield return new WaitForSecondsRealtime(wait_time);

        anim.SetTrigger("Out");

        yield return new WaitForSecondsRealtime(transit_time);

        SceneManager.LoadScene(scene_name);
    }
}
