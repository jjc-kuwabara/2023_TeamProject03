using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    [Header("動くスピード、向き")]
    public float moveSpeed = 3f;
    public Vector3 moveAngle;

    Vector3 move;

    GameObject player;
    PlayerController controller;

    bool scrollFLG = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (scrollFLG)
        {
            Scroll();
        }
    }

    void Scroll()
    {
        move = moveAngle * moveSpeed * Time.deltaTime;
        controller.x_L += move.x;
        controller.x_R += move.x;

        if (controller.x_L >= controller.transform.position.x)
        {
            controller.Move_2(move);
        }
        transform.Translate(move);
    }

    public void ScrollFLGChange(bool flg)
    {
        scrollFLG = flg;
    }
}
