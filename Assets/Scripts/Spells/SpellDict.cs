using System;
using System.Collections.Generic;

using UnityEngine;

//Spell dictionary

//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class SpellDict : MonoBehaviour
    {
        [Serializable]
        public class Spell_Info
        {
            public string rune, type;
            public GameObject spell_ball;
            public Sprite icon;

            [HideInInspector]
            public float nextFireTime = 0;
        }

        //Spells
        [SerializeField]
        private List<Spell_Info> spells;

        public Dictionary<string, Spell_Info> dict
         = new Dictionary<string, Spell_Info>();


        void Start()
        {
            //Set up dictionary
            for(int i = 0; i < spells.Count; i++)
                dict.Add(spells[i].rune, spells[i]);
        }
    }

}