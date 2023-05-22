using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DebugManager : MonoBehaviour
{
    public GameObject imageFLG;
    public TextMeshProUGUI x_L;
    public TextMeshProUGUI x_R;

    GameObject scroll;
    ScrollManager scrollManager;

    GameObject player;
    PlayerController controller;

    void Start()
    {
        scroll = GameObject.FindGameObjectWithTag("Scroll");
        scrollManager = scroll.GetComponent<ScrollManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (scrollManager.scrollFLG)
        {
            imageFLG.SetActive(false);
        }

        x_L.text = controller.x_L.ToString("00.0");
        x_R.text = controller.x_R.ToString("00.0");
    }
}