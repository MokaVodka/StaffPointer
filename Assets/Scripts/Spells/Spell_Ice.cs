using System.Collections;

using UnityEngine;


//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class Spell_Ice : Spell
    {
        [Header("Real time that cursor is slowed, in seconds")]
        [SerializeField]
        private float slow_time;

        [Header("Bigger mod = slower cursor, start from 1")]
        [SerializeField]
        private float slow_mod;


        void Update()
        {
            RB.velocity = velocity;
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            //When hits other element
            if(collision.gameObject.layer == LayerMask.NameToLayer("Spell Fire"))
            {
                Spell otherSpell = collision.gameObject.GetComponent<Spell>();

                //Destroy self if the other spel is of higher tier
                if(otherSpell.tier >= tier)
                {
                    //Feedback
                    Game.sfx.SFX_Cancel();

                    var fx_anim = Game.fx.Get_Animator("cancel");
                    fx_anim.transform.position = transform.position;
                    Game.fx.PlayFX(fx_anim, "cancel");

                    var ps_cancel = fx_anim.gameObject.GetComponent<ParticleSystem>();
                    Game.fx.PlayCancelPS(ps_cancel, "ice");

                    Destroy(gameObject);
                }
            }

            //When hits any player
            if (collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();

                player.cursor.Start_SlowStroke(slow_time, slow_mod);

                player.currentHealth -= dmg * reflect_dmgMod.Evaluate(timeReflected);

                player.Enable_FX("freeze");
                player.Disable_FX(slow_time);

                var fx_anim = Game.fx.Get_Animator("freeze");
                fx_anim.transform.position = player.transform.position;
                Game.fx.PlayFX(fx_anim, "freeze");

                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //When hits shield
            if(collision.gameObject.tag == "Shield")
                OnCollide_Shield();
        }
    }
}