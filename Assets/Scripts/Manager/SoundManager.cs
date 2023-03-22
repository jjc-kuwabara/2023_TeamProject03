using System.Collections;
using UnityEngine;
using UnityEngine.Audio;    //AudioMixer���g�p����̂ɕK�v
using SaveData_Settings;    //���O�ō����SaveData_Settings�̎g�p�ɕK�v

public class SoundManager : Singleton<SoundManager>
{
    //--------------------------------------
    //�Q�[���ɕK�v�ȃT�E���h���Ǘ�����
    //--------------------------------------

    //�g�p���@
    //���̃X�N���v�g�ŃQ�[���Ɏg�p����S�Ă�Audio���Ǘ�����
    //BGM�ASystemSE�AGameSE��3��ނ�AudioClip�𕪂��Ă���̂ŁA
    //�ǉ��������ꍇ�́A�G�f�B�^�[�Ŕz��̐��𑝂₵�Ēǉ�����

    //�Q�[���N�����AAudioClip���Ƃ�AudioSorce�������I�ɒǉ����Ă���
    //AudioMixer�ŃO���[�v���Ƃɂ܂Ƃ߂ĉ��ʂ�ύX�ł���

    //�J�e�S�����Ƃɉ����Đ����郁�\�b�h�𕪂��Ă���̂ŁA
    //����炵�����^�C�~���O�Ŋe���\�b�h���Ăяo���Ă�����Ηǂ�
    //��@PlaySE_Sys

    //����
    //���O�ŗp�ӂ���NameSpace�uSaveData_Settings�v���g���A
    //���ʂ̕ۑ��A���[�h���s���Ă���̂ŁA���̃X�N���v�g���Ȃ��ƃG���[�ɂȂ�
    //SaveData_Settings�̃X�N���v�g�̓v���W�F�N�g�ɑ��݂��Ă��邾���ŗǂ�


    [Header("���ʃR���g���[����Mixer")]
    [SerializeField] public AudioMixer mixer;   //���ʂ��R���g���[������
    [SerializeField, Label("Master�O���[�v")] public AudioMixerGroup MasterGroup;
    [SerializeField, Label("BGM�O���[�v")] public AudioMixerGroup BGMGroup;
    [SerializeField, Label("SE�O���[�v")] public AudioMixerGroup SEGroup;
    [Space(10)]
    //�e�J�e�S�����Ƃ�AudioClip������ϐ���z��ŗp��
    public AudioClip[] bgmClip;
    public AudioClip[] se_SysClip;
    public AudioClip[] se_GameClip;

    //�e�����p��AudioSource��p�ӂ���
    [System.NonSerialized] public AudioSource BGMSource;
    [System.NonSerialized] public AudioSource SE_SysSource;
    [System.NonSerialized] public AudioSource[] SE_GameSource;

    //���ʂ̒i�K�iSlider��Value�Őݒ�j
    float[] vol_BGM = { -80f, -30f, -27, -24f, -21f, -18f, -15f, -12.5f, -10f, -7.5f, -5f };
    float[] vol_SE = { -80f, -14f, -12f, -9f, -7f, -5f, -3f, -1f, 1f, 3f, 5f };

    //�t�F�[�h�p�ɉ��ʂ�ێ�
    float bgmVol;


    //�V�[���J�n����iStart���\�b�h��葁���j�ɏ���
    void Awake()
    {
        Load.Audio(); //�ۑ����ꂽ���ʂ����[�h

        //AddComponent��AudioSource��ǉ��A���[�v�ݒ�A�D��x�AMixerGroup�̐ݒ�
        //BGM
        BGMSource = gameObject.AddComponent<AudioSource>();
        BGMSource.loop = true;
        BGMSource.priority = 0;
        BGMSource.outputAudioMixerGroup = BGMGroup;

        //��{�I�ɕ�����System�pSE�������ɂȂ邱�Ƃ͂Ȃ�����
        //System�p��SE��炷�����ł�AudioSource�͂P����
        SE_SysSource = gameObject.AddComponent<AudioSource>();
        SE_SysSource.loop = false;
        SE_SysSource.priority = 1;
        SE_SysSource.outputAudioMixerGroup = SEGroup;

        //���C���̃Q�[�����Ɏg�p����SE�͕����̉��������ɖ邱�Ƃ���������
        //SE�̃N���b�v���Ɠ�������AudioSource��p�ӂ���
        SE_GameSource = new AudioSource[se_GameClip.Length];

        for (int i = 0; i < se_GameClip.Length; i++)
        {
            if (se_GameClip[i] != null)
            {
                SE_GameSource[i] = gameObject.AddComponent<AudioSource>();
                SE_GameSource[i].loop = false;
                SE_GameSource[i].priority = 1;
                SE_GameSource[i].clip = se_GameClip[i];
                SE_GameSource[i].outputAudioMixerGroup = SEGroup;
            }
        }
    }

    //BGM���O������Ăяo����
    public void PlayBGM(int i)
    {
        BGMSource.clip = bgmClip[i];
        BGMSource.Play();
    }

    //SytemSE���O������Ăяo����
    public void PlaySE_Sys(int i)
    {
        SE_SysSource.clip = se_SysClip[i];
        SE_SysSource.Play();
    }

    //GameSE���O������Ăяo����
    public void PlaySE_Game(int i)
    {
        SE_GameSource[i].Play();
    }

    //Silder�ɂ�鉹�ʂ̒���
    //�i��1������BGM�A��2������SE�̃{�����[���j
    public void VolumeChange(int vol1, int vol2)
    {
        mixer.SetFloat("BGVol", vol_BGM[vol1]);
        mixer.SetFloat("SEVol", vol_SE[vol2]);

        bgmVol = vol_BGM[vol1];
    }

    //��ʂ��t�F�[�h�A�E�g���鎞
    //���ʂ��ꏏ�Ƀt�F�[�h�A�E�g �t�F�[�h����̂�BGM�̂�
    public IEnumerator FadeOut(float interval)
    {
        float time = 0;
        while (time <= interval)
        {
            float bgm = Mathf.Lerp(bgmVol, -80f, time / interval);
            mixer.SetFloat("BGVol", bgm);
            time += Time.deltaTime;
            yield return null;
        }
    }

    //��ʂ��t�F�[�h�A�E�g���鎞
    //���ʂ��ꏏ�Ƀt�F�[�h�C��
    public IEnumerator FadeIn(float interval)
    {
        float time = 0;
        while (time <= interval)
        {
            float bgm = Mathf.Lerp(-80f, bgmVol, time / interval);
            mixer.SetFloat("BGVol", bgm);
            time += Time.deltaTime;
            yield return null;
        }
    }
}