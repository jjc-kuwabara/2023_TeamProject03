using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    GameObject player;
    GameObject goal;

    float probressValue;
    float distanceMax;
    float distanceCurrent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goal = GameObject.FindGameObjectWithTag("Finish");

        distanceMax = goal.transform.position.x - player.transform.position.x;

        probressValue = 1;
    }

    void Update()
    {
        distanceCurrent = goal.transform.position.x - player.transform.position.x;

        probressValue = distanceCurrent / distanceMax;

        GameManager.Instance.ProgressUpdate(probressValue);
    }
}
