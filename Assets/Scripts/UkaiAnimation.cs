using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UkaiAnimation : MonoBehaviour
{
    //アニメーション用変数
    Animator animator;  //Animatorのコンポーネント取得用

    bool death = false;
    bool look = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.gameOver && GameManager.Instance.foundFLG && !death)
        {
            death = true;
            return;
        }

        if (GameManager.Instance.gameOver && !death)
        {
            animator.SetTrigger("Depressed");
            death = true;
            return;
        }

        if (GameManager.Instance.secondFoundFLG && !look)
        {
            animator.SetBool("Seek", false);
            animator.SetTrigger("Look");
            look = true;
            return;
        }else if (!GameManager.Instance.secondFoundFLG && look)
        {
            look = false;
            return;
        }

        animator.SetBool("Seek", true);
    }
}
