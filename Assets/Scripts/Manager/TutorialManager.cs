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
    public GameObject endArea;

    TutorialArea moveFLG;
    TutorialArea airFLG;
    TutorialArea attackFLG;
    TutorialArea eatFishFLG;
    TutorialArea eatItemFLG;
    TutorialArea speedDownFLG;
    TutorialArea endFLG;

    [Header("移動時間")]
    public float moveTime = 3;
    float inputTime = 0;

    [Header("チュートリアル用の敵")]
    public GameObject enemy_Attack;
    public GameObject enemy_Eat;

    [Header("チュートリアル用のアイテム")]
    public GameObject item_Heal;
    public GameObject item_Score;
    public GameObject item_Bullet;
    bool item_HealFLG = false;
    bool item_ScoreFLG = false;
    bool item_BulletFLG = false;

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
        endFLG = speedDownArea.GetComponent<TutorialArea>();

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

        if (endFLG.flg)
        {
            TextAllNotActive();
            endFLG.flg = false;
        }
    }

    void Move()
    {
        if (!move[0].activeSelf && !move[1].activeSelf)
        {
            TextAllNotActive();
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

            scrollM.ScrollFLGChange(true);

            moveFLG.flg = false;
        }
    }

    void Air()
    {
        if (!air[0].activeSelf && !air[1].activeSelf && !air[2].activeSelf)
        {
            TextAllNotActive();
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

            scrollM.ScrollFLGChange(true);

            airFLG.flg = false;
        }
    }

    void Attack()
    {
        if (!attack[0].activeSelf && !attack[1].activeSelf)
        {
            TextAllNotActive();
            scrollM.ScrollFLGChange(false);
            move[0].SetActive(true);
        }

        if (enemy_Attack = null)
        {
            attack[0].SetActive(false);
            attack[1].SetActive(true);

            scrollM.ScrollFLGChange(true);

            attackFLG.flg = false;
        }
    }

    void EatFish()
    {
        if (!eatFish[0].activeSelf && !eatFish[1].activeSelf)
        {
            TextAllNotActive();
            scrollM.ScrollFLGChange(false);
            eatFish[0].SetActive(true);
        }

        if (enemy_Eat = null)
        {
            eatFish[0].SetActive(false);
            eatFish[1].SetActive(true);

            scrollM.ScrollFLGChange(true);

            eatFishFLG.flg = false;
        }
    }

    void EatItem()
    {
        if (!eatItem[0].activeSelf && !eatItem[1].activeSelf)
        {
            TextAllNotActive();
            scrollM.ScrollFLGChange(false);
            eatItem[0].SetActive(true);
        }

        if (item_Heal = null)
        {
            TextAllNotActive();
            eatItem[1].SetActive(true);

            item_HealFLG = true;
        }

        if (item_Score = null)
        {
            TextAllNotActive();
            eatItem[2].SetActive(true);

            item_ScoreFLG = true;
        }

        if (item_Bullet = null)
        {
            TextAllNotActive();
            eatItem[3].SetActive(true);

            item_BulletFLG = true;
        }

        if(item_HealFLG && item_ScoreFLG && item_BulletFLG)
        {
            scrollM.ScrollFLGChange(true);

            eatItemFLG.flg = false;
        }
    }

    void SpeedDown()
    {
        if (!speedDown[0].activeSelf && !speedDown[1].activeSelf)
        {
            TextAllNotActive();
            scrollM.ScrollFLGChange(false);
            speedDown[0].SetActive(true);

            inputTime = 0;
        }

        if (speedDown[0].activeSelf && Input.GetButton("Fire3"))
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                inputTime += Time.deltaTime;
            }
        }

        if (inputTime >= moveTime)
        {
            speedDown[0].SetActive(false);
            speedDown[1].SetActive(true);

            scrollM.ScrollFLGChange(true);

            speedDownFLG.flg = false;
        }
    }

    void TextAllNotActive()
    {
        for (int i = 0; i < move.Length; i++)
        {
            move[i].SetActive(false);
        }
        for (int i = 0; i < air.Length; i++)
        {
            air[i].SetActive(false);
        }
        for (int i = 0; i < attack.Length; i++)
        {
            attack[i].SetActive(false);
        }
        for (int i = 0; i < eatFish.Length; i++)
        {
            eatFish[i].SetActive(false);
        }
        for (int i = 0; i < eatItem.Length; i++)
        {
            eatItem[i].SetActive(false);
        }
        for (int i = 0; i < speedDown.Length; i++)
        {
            speedDown[i].SetActive(false);
        }
    }
}
