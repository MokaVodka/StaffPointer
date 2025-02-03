using UnityEngine;


//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class Spell_Fire : Spell
    {
        [SerializeField]
        private float chance;

        [Header("Crit modifier, start from 1")]
        [SerializeField]
        private float mod_critDmg;


        void Update()
        {
            RB.velocity = velocity;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //When hits other element
            if(collision.gameObject.layer == LayerMask.NameToLayer("Spell Ice"))
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
                    Game.fx.PlayCancelPS(ps_cancel, "fire");

                    Destroy(gameObject);                    
                }
            }
            
            //When hits any player
            if(collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();

                player.Enable_FX("burn");

                float randomNum = Random.Range(0, 100);

                if (randomNum <= chance)
                {
                    player.currentHealth -= (dmg * reflect_dmgMod.Evaluate(timeReflected) * mod_critDmg);

                    var fx_anim = Game.fx.Get_Animator("crit");
                    fx_anim.transform.position = player.transform.position;
                    Game.fx.PlayFX(fx_anim, "crit");

                    Game.sfx.SFX_Crit();
                }
                else
                {
                    player.currentHealth -= dmg * reflect_dmgMod.Evaluate(timeReflected);
                    player.Set_Active_PS("burn", false);
                }

                player.Disable_FX(2.5f);

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
