using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    //--------------------------------------
    //�Q�[���ɕK�v�ȃG�t�F�N�g���Ǘ�����
    //�K�v�Ȏ��Ɋe�X�N���v�g����Ăяo����悤�ɂ��Ă���
    //--------------------------------------

    [Header("Player�̋����Ɋւ���G�t�F�N�g")]
    public GameObject[] playerFX;

    [Header("�G��X�e�[�W�Ɋւ���G�t�F�N�g")]
    public GameObject[] StageFX;

    [Header("�i�s���Ɋւ��G�t�F�N�g")]
    public GameObject[] otherFX;

    //�ʂɃp�[�e�B�N���̃R���|�[�l���g���擾��������
    //public ParticleSystem fx001;


    void Start()
    {
        //fx001 = GetComponent<ParticleSystem>();
    }

}