using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Debug_mainGame();

        Debug_Eat();
    }

    void Debug_mainGame()
    {
        if (!GameManager.Instance.mainGameFLG)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.mainGameFLG = true;
            }
        }
    }

    void Debug_Eat()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.Eat();
        }
    }
}