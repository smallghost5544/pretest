using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniControl : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAnimate();
        Attack();
    }

    private void MoveAnimate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v != 0 || h != 0)
            animator.SetBool("IsWalk", true);
        else
            animator.SetBool("IsWalk", false);
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
}
