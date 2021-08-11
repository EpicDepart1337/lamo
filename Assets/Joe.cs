using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Joe", menuName = "Characters/Joe", order = 2)]
public class Joe : Character
{
    public string name = "Joe";
    public Character swapto;
    public override void swap(PlayerInput pi)
    {
        pi.currChar = swapto;
    }
    public override void QCF(PlayerInput pi, bool EX)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetTrigger("JoeQCF");
        pi.nameHurtbox("IceBall");
        pi.damageHurtbox(0);
    }

}
