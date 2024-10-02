using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleAnimationStateController : MonoBehaviour
{
    Animator animator;
    MoleAI mole;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mole = GetComponent<MoleAI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(mole.CurrentState)
        {
            case MoleAI.MoleState.Chasing: // fallthrough
            case MoleAI.MoleState.Fleeing: // fallthrough
            case MoleAI.MoleState.Patrolling:
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);
                break;
            case MoleAI.MoleState.Attacking:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
                break;
        }
    }
}
