using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu_Credits : MonoBehaviour
{
    private int page = 0;

    [SerializeField]
    private int pages;

    [SerializeField]
    [TextArea]
    private string[] role_txt, ppl_txt;

    [SerializeField]
    private TextMeshProUGUI role, ppl;


    void Start()
    {
        Set_Credits();
    }

    void OnEnable()
    {
        page = 0;
        Set_Credits();
    }

    public void Next_Page()
    {
        page++;
        Set_Credits();
    }

    void Set_Credits()
    {
        role.text = role_txt[page%pages];
        ppl.text  = ppl_txt[page%pages];
    }
}