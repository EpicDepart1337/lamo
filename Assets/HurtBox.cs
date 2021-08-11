using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HurtBox : MonoBehaviour
{
    //default name
    public string moveName = "UNKNOWN";
    public int damage;
    public PlayerInput parent;
    public Slider opponent;
    public GameObject oppModel;
    public Rigidbody oppBody;
    bool frozen;
    int onGround = 0;
    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Player"))
        {
            return;
        }
        //<> searches on compile while () searches on runtime, DON"T PUT IT IN ()



        if (this.isActiveAndEnabled)
        {

            Debug.Log($"AI was hit by {parent.name}'s {moveName}");
            
            Animator animator = oppModel.GetComponent<Animator>();

            if (moveName == "Bob Punch 3" || moveName == "bobNFDP" || moveName == "Bob Semi Hold D" || moveName == "Full Hold D Attack")
            {
                if (onGround == 0)
                {
                    animator.SetTrigger("BobKnockDown");
                    onGround = 300;
                    if (moveName == "Bob Punch 3" || moveName == "bobNFDP" || moveName == "Bob Semi Hold D")
                    {

                        oppBody.velocity += Vector3.up * 50;
                    }
                    else if (moveName == "Full Hold D Attack")
                    {
                        oppBody.velocity += Vector3.right * 50;
                        oppBody.velocity += Vector3.up * 20;
                    }
                    opponent.value -= damage;
                }
            }
            else if (moveName == "IceBall")
            {
                if (!frozen)
                {
                    animator.SetTrigger("Frozen");
                    frozen = true;
                    onGround = 0;
                    opponent.value -= damage;
                }
            }
            else
            {
                if (!frozen && onGround == 0)
                {
                    animator.SetTrigger("BobHurt");
                    opponent.value -= damage;
                }

            }
            moveName = "UNKNOWN";
            damage = 0;

        }
        if(opponent.value == 0)
        {
            Debug.Log("Win");
            Destroy(oppModel);
        }
    }
    
    private void Update()
    {
        if(onGround != 0)
        {
            onGround--;
        }
    }
}
