using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEatController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //OnTriggerですり抜け判定を取る
    private void OnTrigger(Collider other)
    {
        if (Input.GetButton("Fire2"))
        {
            if (other.transform.tag == "Fish")
            {
                GameManager.Instance.Eat();
                Destroy(other.gameObject);
            }

            if (other.transform.tag == "Bubble")
            {
                GameManager.Instance.AirGet();
            }
        }
    }
}