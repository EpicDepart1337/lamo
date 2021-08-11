using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject, ICharacter
{
    
    
    public int direction;
    public int attackString= 0;
    public bool isDashing;
    public float speed;
    public float jumpSpeed;
    public string name;
    public virtual void QCF(PlayerInput pi, bool EX){}
    public virtual void QCB(PlayerInput pi, bool EX){}
    public virtual void BDP(PlayerInput pi, bool EX){}
    public virtual void FDP(PlayerInput pi, bool EX){}
    public virtual void HCF(PlayerInput pi, bool EX){}
    public virtual void HCB(PlayerInput pi, bool EX){}
    public virtual void Block(PlayerInput pi)
    {
    
    
    }
    public virtual void Attack(PlayerInput pi)
    {

    }
    public virtual void DashAttack(PlayerInput pi)
    {

    }
    public virtual void swap(PlayerInput pi) { }
    public virtual void HoldDStartup(PlayerInput pi) { }
    public virtual void SemiHoldD(PlayerInput pi) { }
    public virtual void FullHoldDAttack(PlayerInput pi) { }
    public virtual void resetAnim(PlayerInput pi) { }
    public virtual void StopBlocking(PlayerInput pi) { }
    public virtual void resetStrings() { }
    public virtual void Jump(PlayerInput pi){}
    public virtual void Dasher(PlayerInput pi ){}
    public virtual void StopDashing(PlayerInput pi) { }
    public virtual void WSkill(PlayerInput pi ){}
    public virtual void ASkill(PlayerInput pi ){}
    public virtual void HSkill(PlayerInput pi ){}
    public virtual void TSkill(PlayerInput pi ){}
    public virtual void Walk(PlayerInput pi) { }
    public virtual void JumpAttack(PlayerInput pi) { }
    public virtual void Idle(PlayerInput pi) { 
    
    }
    /// <summary>
    /// normal forwards dragon punch
    /// </summary>
    /// <param name="pi"></param>
    public virtual void NFDP(PlayerInput pi) { }
}
