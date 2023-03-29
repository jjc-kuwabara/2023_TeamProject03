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
        Debug_Eat();
    }

    void Debug_Eat()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.Eat();
        }
    }
}