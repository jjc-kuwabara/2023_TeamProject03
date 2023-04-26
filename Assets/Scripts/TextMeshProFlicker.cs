using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProFlicker : MonoBehaviour
{

    //�C���X�y�N�^�[����ݒ肷�邩�A����������GetComponent���āATextMeshProUGUI�ւ̎Q�Ƃ��擾���Ă����B
    [SerializeField]
    TextMeshProUGUI tmp;

    [Header("1���[�v�̒���(�b�P��)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float duration = 1.0f;

    //�J�n���̐F�B
    [Header("���[�v�J�n���̐F")]
    [SerializeField]
    Color32 startColor = new Color32(255, 255, 255, 255);

    //�I��(�܂�Ԃ�)���̐F�B
    [Header("���[�v�I�����̐F")]
    [SerializeField]
    Color32 endColor = new Color32(255, 255, 255, 64);



    //�C���X�y�N�^�[����ݒ肵���ꍇ�́AGetComponent����K�v���Ȃ��Ȃ�ׁAAwake���폜���Ă��ǂ��B
    void Awake()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        tmp.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
    }
}