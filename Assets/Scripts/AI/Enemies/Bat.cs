using System;
using UnityEngine;

/*public enum BatSounds
{
    Attack      = 0,
    Death       = 1,
}*/

public class Bat : AIEnemy
{
    //public AudioDirector audioDirector;
    public override void StateNormal()
    {
        float angle = Game.GetAngleBetweenPositions(target.position, transform.position);

        if (angle <= 90f && angle >= -90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public override void SetAnimation()
    {
        animations.SetInteger(Game.animationKeyID, (int)state);
    }

    public override void OnDie(object sender, EventArgs e)
    {
        rb.simulated = true;
        rb.gravityScale = 4;
        deathTime = Time.time + 3f;
        state = EntityState.Death;
    }
}
