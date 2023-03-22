using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FadeManager : Singleton<FadeManager>
{
	//--------------------------------------------------
	//�V�[���J�ڎ��Ƀt�F�[�h�C���E�A�E�g���s�����߂̏���
	//--------------------------------------------------

	//�g�p���@
	//�V�[���́u���O�v���g���đJ�ڂ���ꍇ��LoadScene���\�b�h�A
	//�V�[���́u�ԍ��v���g���đJ�ڂ���ꍇ��LoadSceneIndex���\�b�h���g��

	//�Ăяo�������^�C�~���O�ňȉ��̂悤�Ƀ��\�b�h���Ăяo��
	//FadeManager.Instance.LoadScene(�V�[����,�t�F�[�h�Ɋ|����b��);
	//FadeManager.Instance.LoadSceneIndex(�V�[���ԍ�,�t�F�[�h�Ɋ|����b��);

	//����
	//�t�F�[�h�̍ۂɁABGM�̉��ʂ��t�F�[�h����悤SoundManager�̏�����
	//�Ăяo���Ă���̂ŁA������̃X�N���v�g�i�Q�[���I�u�W�F�N�g�j��
	//�V�[����ɑ��݂��Ă��Ȃ��ƃG���[�ɂȂ�


	private float fadeAlpha = 0;                    //�t�F�[�h���̓����x
	[NonSerialized] public bool isFading = false;   //�t�F�[�h�����ǂ���
	public Color fadeColor = Color.black;           //�t�F�[�h�F

	public void Awake()
	{
		//�������̃I�u�W�F�N�g�̎q�ł���΁A�e�q�֌W������
		if (gameObject.transform.parent != null) gameObject.transform.parent = null;

		//��������FadeManager�����݂��Ă�����A���̃I�u�W�F�N�g��Destroy
		if (this != Instance)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);  //FadeManager�̓V�[���J�ڂ��Ă��폜���Ȃ�
	}

	public void OnGUI()
	{
		if (isFading == true)
		{
			//�F�Ɠ����x���X�V���Ĕ��e�N�X�`����`�� .
			fadeColor.a = fadeAlpha;
			GUI.color = fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
	}

	//�V�[�����ŉ�ʑJ��
	public void LoadScene(string scene, float interval)
	{
		StartCoroutine(TransScene(scene, interval));
	}

	//IndexNumber�ŉ�ʑJ��
	public void LoadSceneIndex(int sceneIndex, float interval)
	{
		StartCoroutine(TransSceneIndex(sceneIndex, interval));
	}

	public void FadeOutCall(float interval)
	{
		StartCoroutine(FadeOut(interval));
	}

	public void FadeInCall(float interval)
	{
		StartCoroutine(FadeIn(interval));
	}

	//�V�[���J�ڗp�R���[�`��
	private IEnumerator TransScene(string scene, float interval)
	{
		//���ʂ��t�F�[�h�A�E�g
		StartCoroutine(SoundManager.Instance.FadeOut(interval * 0.9f));

		//���񂾂�Â�
		isFading = true;
		float time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		Debug.Log("�V�[����J�ځc" + scene);
		SceneManager.LoadScene(scene); //�V�[���ؑ�

		//���ʂ��t�F�[�h�C��
		StartCoroutine(SoundManager.Instance.FadeIn(interval * 0.9f));

		//���񂾂񖾂邭
		time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		isFading = false;
	}

	//IndexNumber�̃V�[���J�ڗp�R���[�`��
	private IEnumerator TransSceneIndex(int sceneIndex, float interval)
	{
		//���ʂ��t�F�[�h�A�E�g
		StartCoroutine(SoundManager.Instance.FadeOut(interval * 0.9f));

		//���񂾂�Â�
		isFading = true;
		float time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		Debug.Log("�V�[����J�ځc" + sceneIndex);
		SceneManager.LoadScene(sceneIndex); //�V�[���ؑ�

		//���ʂ��t�F�[�h�C��
		StartCoroutine(SoundManager.Instance.FadeIn(interval * 0.9f));

		//���񂾂񖾂邭
		time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		isFading = false;
	}

	private IEnumerator FadeOut(float interval)
	{
		//���ʂ��t�F�[�h�A�E�g
		StartCoroutine(SoundManager.Instance.FadeOut(interval * 0.9f));

		//���񂾂�Â�
		isFading = true;
		float time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator FadeIn(float interval)
	{
		//���ʂ��t�F�[�h�C��
		StartCoroutine(SoundManager.Instance.FadeIn(interval * 0.9f));

		//���񂾂񖾂邭
		float time = 0;
		while (time <= interval)
		{
			fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return null;
		}

		isFading = false;
	}
}