using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Bob",menuName = "Characters/Bob", order = 1)]
public class Bob : Character
{
    public Character swapto;
    // float speed = 5;
    public string name = "bob";
    public override void swap(PlayerInput pi)
    {
        pi.currChar = swapto;
    }
    public override void Dasher(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        if (!pi.isGrounded)
        {

           // animator.SetBool("IsDashing", false);
            return;
        }
        
        //Debug.Log($"{pi.name} is dashing!");
        animator.SetBool("IsDashing", true);
        switch (this.direction)
        {
            case 1:
                pi.model.transform.position += new Vector3(-speed * 2, 0, -speed * 2) * Time.deltaTime;
                break;
            case 2:
                pi.model.transform.position += new Vector3(0, 0, -speed * 2) * Time.deltaTime;
                break;
            case 3:
                pi.model.transform.position += new Vector3(speed * 2, 0, -speed * 2) * Time.deltaTime;
                break;
            case 4:
                pi.model.transform.position += new Vector3(-speed * 2, 0, 0) * Time.deltaTime;
                break;
            case 6:
                pi.model.transform.position += new Vector3(speed * 2, 0, 0) * Time.deltaTime;
                break;
            case 7:
                pi.model.transform.position += new Vector3(-speed * 2, 0, speed * 2) * Time.deltaTime;
                break;
            case 8:
                pi.model.transform.position += new Vector3(0, 0, speed * 2) * Time.deltaTime;
                break;
            case 9:
                pi.model.transform.position += new Vector3(speed * 2, 0, speed * 2) * Time.deltaTime;
                break;
            default:
                break;
        }
        if (animator.GetBool("IsWalking"))
        {
            animator.SetBool("IsWalking", false);
        }
    }
    public override void StopDashing(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetBool("IsDashing", false);
        pi.isDashing = false;
    }

    public override void DashAttack(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
      //  Debug.Log($"{pi.name} is dash attacking!");
        animator.SetTrigger("isDashAttacking");
        animator.ResetTrigger("Punch");
        pi.nameHurtbox("Dash Attack");
        pi.resetholdDTimer();
        pi.damageHurtbox(10);
        pi.recoveryFrames = 30;
        pi.comboWindow = false;
        pi.accemptableInputs = -1;
       

        
        
    }
    public override void resetStrings()
    {
        attackString = 0;
    }
    public override void JumpAttack(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        Debug.Log($"{pi.name} is jump attacking!");
        pi.nameHurtbox("Bob Jump attack");
        animator.SetBool("isJumpAttacking", true);
        pi.damageHurtbox(5);

    }
    public override void Jump(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        Debug.Log($"{pi.name} is jumping!");
        animator.SetBool("IsJumping", true);
        
        pi.model.velocity = Vector3.up * jumpSpeed;
        if(animator.GetBool("IsWalking"))
        {
            switch(direction)
            {
                case 1:
                    pi.model.velocity += Vector3.left * jumpSpeed / 4;
                    pi.model.velocity += Vector3.back * jumpSpeed / 4;
                    break;
                case 2:
                    pi.model.velocity += Vector3.back * jumpSpeed/2;
                    break;
                case 3:
                    pi.model.velocity += Vector3.back * jumpSpeed /4;
                    pi.model.velocity += Vector3.right * jumpSpeed /4;
                    break;
                case 4:
                    pi.model.velocity += Vector3.left * jumpSpeed/2;
                    break;
                case 5:
                    break;
                case 6:
                    pi.model.velocity += Vector3.right * jumpSpeed/2;
                    break;
                case 7:
                    pi.model.velocity += Vector3.forward * jumpSpeed/4;
                    pi.model.velocity += Vector3.left * jumpSpeed / 4;
                    break;
                case 8:
                    pi.model.velocity += Vector3.forward * jumpSpeed/2;
                    break;
                case 9:
                    pi.model.velocity += Vector3.forward * jumpSpeed /4;
                    pi.model.velocity += Vector3.right * jumpSpeed / 4;
                    break;

                default:
                    break;
            }
            animator.SetBool("IsWalking", false);
        }

        if(animator.GetBool("IsDashing"))
        {
            switch (direction)
            {
                case 1:
                    pi.model.velocity += Vector3.left * jumpSpeed / 2;
                    pi.model.velocity += Vector3.back * jumpSpeed / 2;
                    break;
                case 2:
                    pi.model.velocity += Vector3.back * jumpSpeed ;
                    break;
                case 3:
                    pi.model.velocity += Vector3.back * jumpSpeed / 2;
                    pi.model.velocity += Vector3.right * jumpSpeed / 2;
                    break;
                case 4:
                    pi.model.velocity += Vector3.left * jumpSpeed ;
                    break;
                case 5:
                    break;
                case 6:
                    pi.model.velocity += Vector3.right * jumpSpeed ;
                    break;
                case 7:
                    pi.model.velocity += Vector3.forward * jumpSpeed / 2;
                    pi.model.velocity += Vector3.left * jumpSpeed / 2;
                    break;
                case 8:
                    pi.model.velocity += Vector3.forward * jumpSpeed ;
                    break;
                case 9:
                    pi.model.velocity += Vector3.forward * jumpSpeed /2;
                    pi.model.velocity += Vector3.right * jumpSpeed / 2;
                    break;

                default:
                    break;
            }
            animator.SetBool("IsDashing", false);
        }


    }
    public override void Attack(PlayerInput pi)
    {
        
        Animator animator = pi.GetComponent<Animator>();
        switch (attackString)
        {
            
       
            case 0:
                //Debug.Log("Attempting bob attack!");
                animator.SetTrigger("Punch");
                pi.nameHurtbox("Bob Punch 1");
                pi.damageHurtbox(5);
                pi.recoveryFrames = 52;
                attackString++;
                pi.comboWindow = false;
                pi.accemptableInputs = 1;
               // Debug.Log("Attack part 1");
                break;
            case 1:
                
                animator.SetTrigger("Punch2");
                pi.nameHurtbox("Bob Punch 2");
                pi.damageHurtbox(5);
                pi.recoveryFrames = 52;
                attackString++;
                pi.comboWindow = false;
                pi.accemptableInputs = 1;
                //Debug.Log("Attack part 2");
                break;


            case 2:
                animator.SetTrigger("Punch3");
                pi.nameHurtbox("Bob Punch 3");
                pi.damageHurtbox(10);
                pi.recoveryFrames = 40;
                attackString = 0;
                pi.comboWindow = false;
                pi.accemptableInputs = 0;
               
               // Debug.Log("Attack part 3");
                resetStrings();
                break;
            default:
                attackString = 0;
                break;
        }
       
    

        
    }
    public override void Walk(PlayerInput pi)
    {

        if (!pi.isGrounded)
        {
            Animator animator = pi.GetComponent<Animator>();
            //animator.SetBool("IsWalking", false);
            return;
        }
        if (!isDashing)
        {
            Animator animator = pi.GetComponent<Animator>();
           // Debug.Log($"{pi.name} is walking!");
            animator.SetBool("IsWalking", true);
            switch (this.direction)
            {
                case 1:
                    pi.model.transform.position += new Vector3(-speed, 0, -speed) * Time.deltaTime;
                    break;
                case 2:
                    pi.model.transform.position += new Vector3(0, 0, -speed) * Time.deltaTime;
                    break;
                case 3:
                    pi.model.transform.position += new Vector3(speed, 0, -speed) * Time.deltaTime;
                    break;
                case 4:
                    pi.model.transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
                    break;
                case 6:
                    pi.model.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
                    break;
                case 7:
                    pi.model.transform.position += new Vector3(-speed, 0, speed) * Time.deltaTime;
                    break;
                case 8:
                    pi.model.transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
                    break;
                case 9:
                    pi.model.transform.position += new Vector3(speed, 0, speed) * Time.deltaTime;
                    break;
                default:
                    break;
            }
            
        }
        
    }
    public override void Idle(PlayerInput pi)
    {
        
        Animator animator = pi.GetComponent<Animator>();
        Debug.Log($"{pi.name} is Idle!");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsDashing", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("isJumpAttacking", false);
        animator.SetBool("Blocking", false);
        animator.ResetTrigger("isDashAttacking");
        
        pi.isGrounded = true;
        pi.isBlocking = false;
    }
    public override void Block(PlayerInput pi)
    {

        Animator animator = pi.GetComponent<Animator>();
        animator.SetBool("Blocking", true);
        pi.isBlocking = true;
        pi.accemptableInputs = -1;
        pi.recoveryFrames = 5;
    }
    public override void StopBlocking(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetBool("Blocking", false);
        pi.isBlocking = false;
        pi.recoveryFrames = 0;
        pi.dashInputRecording = 5;
        pi.dashTimer = 0;
    }
    public override void HoldDStartup(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetTrigger("bobHoldDCharge");
    }
    public override void SemiHoldD(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetTrigger("bobSemiDRelease");
        animator.ResetTrigger("bobHoldDCharge");
        pi.nameHurtbox("Bob Semi Hold D");
        pi.damageHurtbox(10);
    }
    public override void FullHoldDAttack(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.SetTrigger("bobFullDRelease");
        animator.ResetTrigger("bobHoldDCharge");
        pi.nameHurtbox("Full Hold D Attack");
        pi.damageHurtbox(25);
    }
    public override void NFDP(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.enabled = false;
        animator.enabled = true;
        animator.SetTrigger("bobNFDP");
        pi.nameHurtbox("Bob Forward Dragon Punch");
        pi.recoveryFrames = 120;
        pi.accemptableInputs = -1;
        pi.comboWindow = false;
        pi.damageHurtbox(25);

    }
    public override void resetAnim(PlayerInput pi)
    {
        Animator animator = pi.GetComponent<Animator>();
        animator.ResetTrigger("Punch");
        animator.ResetTrigger("bobNFDP");
    }
    public override void QCF(PlayerInput pi, bool EX)
    {
        return;
    }
}

