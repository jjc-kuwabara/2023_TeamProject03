using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("表示するテキスト等")]
    public GameObject[] move;
    public GameObject[] air;
    public GameObject[] attack;
    public GameObject[] eatFish;
    public GameObject[] eatItem;
    public GameObject[] speedDown;

    [Header("表示するエリア")]
    public GameObject moveArea;
    public GameObject airArea;
    public GameObject attackArea;
    public GameObject eatFishArea;
    public GameObject eatItemArea;
    public GameObject speedDownArea;

    TutorialArea moveFLG;
    TutorialArea airFLG;
    TutorialArea attackFLG;
    TutorialArea eatFishFLG;
    TutorialArea eatItemFLG;
    TutorialArea speedDownFLG;

    [Header("移動時間")]
    public float moveTime = 3;
    float inputTime = 0;

    [Header("チュートリアル用の敵")]
    public GameObject[] enemy;

    [Header("スクロールのオブジェクト")]
    public GameObject scroll;
    ScrollManager scrollM;

    void Start()
    {
        moveFLG = moveArea.GetComponent<TutorialArea>();
        airFLG = airArea.GetComponent<TutorialArea>();
        attackFLG = attackArea.GetComponent<TutorialArea>();
        eatFishFLG = eatFishArea.GetComponent<TutorialArea>();
        eatItemFLG = eatItemArea.GetComponent<TutorialArea>();
        speedDownFLG = speedDownArea.GetComponent<TutorialArea>();

        scrollM = scroll.GetComponent<ScrollManager>();
    }

    void Update()
    {
        if (moveFLG.flg)
        {
            Move();
        }

        if (airFLG.flg)
        {
            Air();
        }

        if (attackFLG.flg)
        {
            Attack();
        }

        if (eatFishFLG.flg)
        {
            EatFish();
        }

        if (eatItemFLG.flg)
        {
            EatItem();
        }

        if (speedDownFLG.flg)
        {
            SpeedDown();
        }
    }

    void Move()
    {
        if (!move[0].activeSelf && !move[1].activeSelf)
        {
            scrollM.ScrollFLGChange(false);
            move[0].SetActive(true);
        }

        if (move[0].activeSelf && Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            inputTime += Time.deltaTime;
        }

        if(inputTime >= moveTime)
        {
            move[0].SetActive(false);
            move[1].SetActive(true);

            if (Input.anyKeyDown)
            {
                move[1].SetActive(false);
                moveFLG.flg = false;

                scrollM.ScrollFLGChange(true);
            }
        }
    }

    void Air()
    {
        if (!air[0].activeSelf && !air[1].activeSelf && !air[2].activeSelf)
        {
            scrollM.ScrollFLGChange(false);
            air[0].SetActive(true);
        }

        if (GameManager.Instance.foundTimeCurrent >= GameManager.Instance.foundTime * 0.5)
        {
            air[0].SetActive(false);
            air[1].SetActive(true);
        }

        if (air[1].activeSelf && !air[2].activeSelf && !GameManager.Instance.foundFLG)
        {
            air[1].SetActive(false);
            air[2].SetActive(true);

            if (Input.anyKeyDown)
            {
                air[2].SetActive(false);
                airFLG.flg = false;

                scrollM.ScrollFLGChange(true);
            }
        }
    }

    void Attack()
    {

    }

    void EatFish()
    {

    }

    void EatItem()
    {

    }

    void SpeedDown()
    {

    }
}
