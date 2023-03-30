using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public GameObject scroll;
    ScrollManager scroll_M;

    void Start()
    {
        scroll_M = scroll.GetComponent<ScrollManager>();
    }

    void Update()
    {
        Debug_Quick();

        Debug_Air();

        Debug_Eat();

        Debug_Reset();
    }

    void Debug_Quick()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            scroll_M.moveSpeed *= 2;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            scroll_M.moveSpeed /= 2;
        }
    }

    void Debug_Air()
    {
        if (Input.GetKey(KeyCode.B))
        {
            GameManager.Instance.AirGet();
        }
    }

    void Debug_Eat()
    {
        if (Input.GetKey(KeyCode.N))
        {
            GameManager.Instance.Eat();
        }
    }

    void Debug_Reset()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.SceneReset();
        }
    }
}