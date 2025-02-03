using UnityEngine;


//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class Spell_Heart : Spell
    {
        void Update()
        {
            RB.velocity = velocity;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //When hits any player
            if(collision.gameObject.tag == "Player")
            {
                Player hp = collision.gameObject.GetComponent<Player>();

                hp.currentHealth += dmg;

                Destroy(gameObject);
            }
        }
    }
}