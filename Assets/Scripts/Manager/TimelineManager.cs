using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    PlayableDirector playableDirector;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        PlayTimeline();
    }

    void Update()
    {
        
    }

    void PlayTimeline()
    {
        playableDirector.Play();
    }
}
