using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatiePrinter : MonoBehaviour
{
    public Animator animator;
    public Animator animatorv2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("Walking", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("Walking", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animatorv2.SetTrigger("Oppakken");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            animatorv2.SetBool("Maken", true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animatorv2.SetBool("Maken", false);
        }
    }
}
