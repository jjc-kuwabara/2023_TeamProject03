using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSensor : MonoBehaviour
{
    //OnTrigger‚Å‚·‚è”²‚¯”»’è‚ðŽæ‚é
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finish")
        {
            GameManager.Instance.GameClear();
        }
    }
}
