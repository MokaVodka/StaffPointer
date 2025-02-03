using UnityEngine;


//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class Spell_Go : Spell
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            //When hits any player
            if(collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();

                var fx_ps = Game.fx.Get_PS("go");

                Vector3 pos = fx_ps.transform.position;
                pos.x = player.transform.position.x;
                fx_ps.transform.position = pos;

                Game.fx.PlayPS(fx_ps);

                Destroy(gameObject);
            }
        }
    }
}