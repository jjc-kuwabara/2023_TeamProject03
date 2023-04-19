using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArea : MonoBehaviour
{
    public bool flg = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            flg = true;

            this.gameObject.SetActive(false);
        }
    }
}
