using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSensor : MonoBehaviour
{
    //OnTriggerですり抜け判定を取る
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finish")
        {
            GameManager.Instance.GameClear();
        }
    }
}
