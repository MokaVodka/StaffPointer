using UnityEngine;

//Base class for all spells

//Remember to use Spells namespace to keep global namespace clean
namespace Spells
{
    public class Spell : MonoBehaviour
    {
        //Movement
        public    float spd;

        [SerializeField]
        protected float mod_spd;

        [HideInInspector]
        public    float dir;
        protected Rigidbody2D RB;
        protected Collider2D cld;
        protected Vector2 velocity = new Vector2(0, 0);

        //Damage
        public float tier;
        public float dmg;

        //Ping pong dmg
        [SerializeField]
        protected AnimationCurve reflect_dmgMod;
        protected int timeReflected;

        //Cooldown
        public float cd_dur;

        //Render
        protected SpriteRenderer spRdr;
        protected Animator anim;

        //Sfx
        protected AudioSource sfx;        


        //Put after dir is set
        public void Init()
        {
            RB  = GetComponent<Rigidbody2D>();
            cld = GetComponent<Collider2D>();
            cld.offset *= dir;

            velocity =  new Vector2(dir * spd, 0);

            anim  = GetComponent<Animator>();
            spRdr = GetComponent<SpriteRenderer>();
            spRdr.flipX = dir < 0;

            sfx = GetComponent<AudioSource>();
        }

        //Put in collision
        protected void OnCollide_Shield()
        {
            timeReflected++;

            //Increase the speed
            spd += mod_spd;

            //Flip direction
            dir *= -1;

            velocity = new Vector2(dir * spd, 0);

            if(anim != null)
                anim.SetTrigger("flip");

            sfx.Play();
        }

        public void FlipSprite()
        {
            spRdr.flipX  = dir < 0;
            cld.offset  *= dir;
        }
    }
}
