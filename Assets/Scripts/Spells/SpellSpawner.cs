using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    public SpellSpawner_Slot[] slots;


    public void Activate_Slot(GameObject spell, string type)
    {
        foreach(SpellSpawner_Slot slot in slots)
        {
            if(!slot.gameObject.activeInHierarchy)
            {
                slot.gameObject.SetActive(true);
                slot.Set_Spell(spell, type);
                break;
            }
        }
    }
}