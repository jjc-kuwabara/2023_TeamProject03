using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    [Header("�����X�s�[�h�A����")]
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
        PC.x_L += move.x;
        PC.x_R += move.x;

        if (PC.x_L >= PC.transform.position.x)
        {
            PC.Move_2(move);
        }
        transform.Translate(move);
    }
}