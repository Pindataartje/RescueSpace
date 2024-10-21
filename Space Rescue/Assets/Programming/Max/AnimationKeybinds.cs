using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationKeybinds : MonoBehaviour
{
    public Animator animator;
    public Animator animatorv2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("Attack");
            animatorv2.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("Walking", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("Walking", false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animatorv2.SetTrigger("Ready");
            animatorv2.SetBool("Ready", true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animatorv2.SetBool("Ready", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("Sleep", true);
        }
    }
}
