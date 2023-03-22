using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCheck : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    //OnTrigger‚Å‚·‚è”²‚¯”»’è‚ðŽæ‚é
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finish")
        {

        }

        if (other.transform.tag == "Air")
        {
            if (!GameManager.Instance.airFLG)
            {
                GameManager.Instance.AirFLG(true);
            }
        }
        else if(GameManager.Instance.airFLG)
        {
            GameManager.Instance.AirFLG(false);
        }
    }
}