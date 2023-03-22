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

    //OnTriggerですり抜け判定を取る
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