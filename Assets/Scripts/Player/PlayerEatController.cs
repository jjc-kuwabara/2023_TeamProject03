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

    //OnTrigger�ł��蔲����������
    private void OnTriggerStay(Collider other)
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