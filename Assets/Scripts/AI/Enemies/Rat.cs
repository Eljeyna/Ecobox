using System;
using UnityEngine;

public enum RatSounds
{
    Attack      = 0,
    Death       = 1,
}

public class Rat : AIEnemy
{
    public AudioDirector audioDirector;

    private void FixedUpdate()
    {
        if (Game.isPause)
        {
            return;
        }

        isGrounded = IsGrounded();
    }

    /*public override void Attack()
    {
        if (!weapon)
        {
            return;
        }

        if (weapon.nextAttack > Time.time)
        {
            return;
        }

        if (weapon.clip == 0)
        {
            weapon.fireWhenEmpty = true;
        }

        weapon.enabled = true;
        targetDirection = (target.position - transform.position).normalized;
        audioDirector.Play((int)SlimeSounds.Attack);
        weapon.PrimaryAttack();
    }

    public override void OnDamaged(object sender, EventArgs e)
    {
        base.OnDamaged(sender, e);

        StaticGameVariables.GetRandom();
        //audioDirector.Play((int)((float)SlimeSounds.Hit1 + StaticGameVariables.random * (SlimeSounds.HitLast - SlimeSounds.Hit1) + 0.5f));
        audioDirector.Play((int)((float)SlimeSounds.Hit1 + StaticGameVariables.random * (float)SlimeSounds.Hit1 + 0.5f));
    }

    public override void OnDie(object sender, EventArgs e)
    {
        base.OnDie(sender, e);

        audioDirector.Play((int)SlimeSounds.Death);
    }*/
}
