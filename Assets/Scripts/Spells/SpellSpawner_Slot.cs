using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner_Slot : MonoBehaviour
{
    private GameObject ref_spell;
    private string type;
    private Vector3 destScale;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Animator anim;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(type == "ice")
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, destScale, ref velocity, 0.2f);
        }
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);   
        velocity = Vector3.zero;
    }

    public void Set_Spell(GameObject spell, string _type)
    {
        ref_spell = spell;
        type      = _type;

        if(type == "heart" || type == "go")
            return;
        
        ref_spell.SetActive(false);
        anim.SetLayerWeight(anim.GetLayerIndex(type), 0.6f);

        destScale = spell.transform.localScale;

        if(type == "fire")
            transform.localScale = destScale;

        else if(type == "ice")
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            destScale /= transform.parent.parent.localScale.x;
        }

        anim.SetTrigger("spawn");
    }

    public void SpawnSpell()
    {
        ref_spell.SetActive(true);
    }

    public void Reset_Components()
    {
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<SpriteRenderer>().sprite = null;
        gameObject.SetActive(false);
    }
}