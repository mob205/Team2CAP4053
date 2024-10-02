using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moleAnimationStateContoller : MonoBehaviour
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
        if (mole.getState() == "patroling")
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }

        else if (mole.getState() == "attacking")
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isWalking", false);
        }

        else if (mole.getState() == "chasing")
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }

        else if (mole.getState() == "fleeing")
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
            
    }
}
