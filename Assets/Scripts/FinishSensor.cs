using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSensor : MonoBehaviour
{
    //OnTrigger�ł��蔲����������
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finish")
        {
            GameManager.Instance.GameClear();
        }
    }
}
