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

        Debug_Reset();
    }

    void Debug_Reset()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.SceneReset();
        }
    }
}