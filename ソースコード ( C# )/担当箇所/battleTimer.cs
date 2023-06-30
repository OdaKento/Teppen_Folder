using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------
// ���e�F�C���Q�[��( �{�� )��ʂ̐������Ԃ�����
// ����ҁF ���c���l
//-----------------------------------------------------------

public class battleTimer : MonoBehaviour
{
    [SerializeField, Header("��������")] private float _gameTime;
    [SerializeField, Header("�e�L�X�g�I�u�W�F�N�g")] private Text _textObject;
    [SerializeField, Header("�e�L�X�g�̐F(�������Ԏc��͂��̏ꍇ)")] private Color _textColor;

    [SerializeField, Header("�v���C���[1��HP�I�u�W�F�N�g")] PlayerHP _player1HP;
    [SerializeField, Header("�v���C���[2��HP�I�u�W�F�N�g")] PlayerHP _player2HP;

    [SerializeField, Header("�T�E���h�Ǘ�")] SoundManager soundManager;
    [SerializeField] AudioClip clip;

    private float _prevTimer;               // �������Ԃ��X�N���v�g���ŊǗ�����ϐ�
    private float _frameTime;               // �t���[�������Ǘ�����ϐ�

    public bool _GameEnd;
    private bool _StopTimer;                // �J�E���g�_�E�����~���鏈��

    private bool _Tstart = false;

    // Start is called before the first frame update
    void Start()
    {
        _prevTimer = _gameTime;             // �������Ԃ�ݒ肷��
        _GameEnd = false;
        _StopTimer = false;                 // 
        soundManager.PlaySe(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player1HP._PlayerHp <= 0 || _player2HP._PlayerHp <= 0)
        {
            _StopTimer = true;
        }

        // _prevTimer �� 0 �ȏ�̏ꍇ�̂ݒl�����炷
        if (_prevTimer >= 0)
        {
            if (!_Tstart) { Invoke("StartTimer", 3.0f); }

            if (!_StopTimer && _Tstart)
            {
                // ���Ԃ����炷                

                _prevTimer -= Time.deltaTime;

                if (_prevTimer <= 10.0f)
                {
                    _textObject.color = _textColor;
                    // _textObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }

                // _gameTime �� _prevTimer �̐����l��������
                _gameTime = (int)_prevTimer;

                // �e�L�X�g�I�u�W�F�N�g�� _gameTime �̒l��\��������
                _textObject.text = _gameTime.ToString();
            }
        }
        else
        {
            // �����I���֐�����x�����Ăяo��
            if (!_GameEnd)
            {
                _GameEnd = true;
                
                Debug.Log("�I���I");
            }
        }
    }

    // ��ʂ�؂�ւ���
    public bool _getTimeOver()
    {
        return _GameEnd;
    }

    void StartTimer()
    {
        _Tstart = true;
    }
}
