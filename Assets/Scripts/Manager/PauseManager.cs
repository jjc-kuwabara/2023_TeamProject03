using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //�t���O
    public bool pauseFLG;    //�|�[�Y��
    public bool hitStopFLG;  //�q�b�g�X�g�b�v

    [Header("�L�����o�X")]
    [SerializeField] GameObject[] canvas;

    [Header("�|�[�Y���j���[�̃J�[�\�������ʒu")]
    [SerializeField] GameObject focusPausemenu;

    [Header("��������̃J�[�\�������ʒu")]
    [SerializeField] GameObject focusInstructions;

    [Header("���ʐݒ�̃J�[�\�������ʒu")]
    [SerializeField] GameObject focusVolumeChange;

    [Header("�q�b�g�X�g�b�v")]
    [SerializeField] float timeScale = 0.1f;
    [SerializeField] float slowTime = 1f;
    float curentTime;

    //�t�H�[�J�X���O��Ȃ��悤�ɂ��鏈���p
    GameObject currentFocus;   //����
    GameObject previousFocus;  //�O�t���[��

    [Header("SE�̔ԍ�")]
    public int pauseSE = 0;
    public int focusMoveSE = 0;
    public int decisionSE = 0;
    public int cancelSE = 0;

    void Start()
    {
        //������
        CanvasInit();

        //���C�����j���[�����A�N�e�B�u
        canvas[0].SetActive(true);
    }

    void Update()
    {
        //�A�b�v�f�[�g���\�b�h�̓^�C���X�P�[����0�ł������͎~�܂�Ȃ�

        //�|�[�Y���łȂ��Ƃ��̂݃{�^�����󂯓����
        if (!pauseFLG && GameManager.Instance.mainGameFLG)
        {
            //P���������玞�Ԓ�~
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangePause(true);
                SoundManager.Instance.PlaySE_Sys(pauseSE);
                return;
            }

            if (!hitStopFLG)
            {
                //O����������q�b�g�X�g�b�v
                if (Input.GetKeyDown(KeyCode.O))
                {
                    HitStopStart();
                    return;
                }
            }

            //�q�b�g�X�g�b�v���̎��Ԍv��
            HitStopTime();
        }

        if (pauseFLG)
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                SoundManager.Instance.PlaySE_Sys(focusMoveSE);
            }
        }

        //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
        FocusCheck();
    }

    /*
    private void FixedUpdate()
    {
        ������̓^�C���X�P�[����0�ɂ���Ə������~�܂�i���W�b�g�{�f�B���j
        �A�j���[�V�����͎~�߂���~�߂Ȃ�������ł���iParticle��cinemachine���j
    }*/

    //�q�b�g�X�g�b�v�J�n
    void HitStopStart()
    {
        curentTime = 0f;
        Time.timeScale = timeScale;
        hitStopFLG = true;
    }

    //�q�b�g�X�g�b�v���Ԍv��
    void HitStopTime()
    {
        if (hitStopFLG)
        {
            curentTime += Time.unscaledDeltaTime;

            //���Ԓ��߂Ō��̑�����
            if (curentTime >= slowTime)
            {
                Time.timeScale = 1;
                hitStopFLG = false;
            }
        }
    }

    //�|�[�Y����
    public void ChangePause(bool flg)
    {
        //�L�����o�X�S������
        CanvasInit();

        pauseFLG = flg;

        //�|�[�Y���������玞�Ԓ�~
        if (flg)
        {
            GameManager.Instance.MainGameFLG(false);

            Time.timeScale = 0;
            canvas[1].SetActive(true);

            //�����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
        else
        {
            Time.timeScale = 1;
            canvas[0].SetActive(true);

            GameManager.Instance.MainGameFLG(true);
        }
    }

    //���ׂẴL�����o�X���\����
    void CanvasInit()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }
    }

    //�t�H�[�J�X���O��Ă��Ȃ����`�F�b�N
    void FocusCheck()
    {
        //���݂̃t�H�[�J�X���i�[
        currentFocus = EventSystem.current.currentSelectedGameObject;

        //�����O��܂ł̃t�H�[�J�X�Ɠ����Ȃ瑦�I��
        if (currentFocus == previousFocus) return;

        //�����t�H�[�J�X���O��Ă�����O�t���[���̃t�H�[�J�X�ɖ߂�
        if (currentFocus == null)
        {
            EventSystem.current.SetSelectedGameObject(previousFocus);
            return;
        }

        //�c���ꂽ��������A�t�H�[�J�X�����݂���̂͊m��
        //�O�t���[���̃t�H�[�J�X���X�V
        previousFocus = EventSystem.current.currentSelectedGameObject;
    }

    //�V�[�������X�^�[�g
    public void Restart()
    {
        //timeScale�����Ƃɖ߂��Ă���
        ChangePause(false);

        GameManager.Instance.SceneReset();
    }

    public void SceneMove(int no)
    {
        //timeScale�����Ƃɖ߂��Ă���
        ChangePause(false);

        GameManager.Instance.SceneMove(no);
    }

    public void CanvasChange_Instructions(bool change)
    {
        CanvasInit();

        if (change)
        {
            canvas[2].SetActive(true);

            //�����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusInstructions);
        }
        else
        {
            canvas[1].SetActive(true);

            //�����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
    }

    public void CanvasChange_VolumeChange(bool change)
    {
        CanvasInit();

        if (change)
        {
            canvas[3].SetActive(true);

            //�����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusVolumeChange);
        }
        else
        {
            canvas[1].SetActive(true);

            //�����J�[�\���ʒu�ݒ�
            EventSystem.current.SetSelectedGameObject(focusPausemenu);
        }
    }

    public void Decision()
    {
        SoundManager.Instance.PlaySE_Sys(decisionSE);
    }

    public void Cancel()
    {
        SoundManager.Instance.PlaySE_Sys(cancelSE);
    }
}