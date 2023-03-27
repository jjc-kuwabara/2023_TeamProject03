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
    PlayerController PC;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PC = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (GameManager.Instance.mainGameFLG)
        {
            Scroll();
        }
    }

    void Scroll()
    {
        move = moveAngle * moveSpeed * Time.deltaTime;
        PC.x_L += (int)move.x;
        PC.x_R += (int)move.x;
        transform.Translate(move);
    }
}
