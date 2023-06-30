using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stareStagingControl : MonoBehaviour
{
    [SerializeField, Header("�v���C���[1�̃I�u�W�F�N�g")] playerattack _player1;
    [SerializeField, Header("�v���C���[1��HP�I�u�W�F�N�g")] PlayerHP _player1HP;
    [SerializeField, Header("�v���C���[2�̃I�u�W�F�N�g")] playerattack _player2;
    [SerializeField, Header("�v���C���[2��HP�I�u�W�F�N�g")] PlayerHP _player2HP;

    [SerializeField, Header("�͂݃C�x���g���N���鎞��")] float maxEventTime;

    [SerializeField, Header("�C�x���g�^�C�}�[�e�L�X�g�I�u�W�F�N�g")]Text _stareEventTime;                 // �͂݃C�x���g�J�E���g�_�E��
    [SerializeField, Header("�A�Ő��e�L�X�g�I�u�W�F�N�g")] Text[] _stareConsecutiveText = new Text[2];    // �͂݃C�x���g���̘A�Ő�

    private float _stareEventTimer;             // �͂݃C�x���g���Ԃ��Ǘ�����ϐ�

    [SerializeField, Header("�C�x���g�^�C�}�[�I�u�W�F�N�g")]GameObject [] _stareStagingObj = new GameObject[2];   // �^�C�}�[�I�u�W�F�N�g�z��
    private int [] _prevConsecutive = new int[2];                                                                 // �A�Ő����Ǘ�����z��

    [SerializeField, Header("�{�^���摜�I�u�W�F�N�g")] Image [] _ButtonImage;

    public bool _HitStareAttack;

    // �I�u�W�F�N�g���m�̋������Ǘ�����ϐ�
    float _distance;

    // �͂ݍ����C�x���g�̔�������
    [SerializeField, Header("�͂ݍ����C�x���g���������F")] float _stareEventStartdistance;

    // �͂ݍ����C�x���g�̔����t���O
    private bool _stareEventF;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log( _player1.gameObject.name + "�F" + _player1._getStare() + " " + _player2.gameObject.name + "�F" + _player2._getStare() + " " );

        if (_player1 != null && _player2 != null)
        {

            _distance = Mathf.Abs(_player1.transform.position.x - _player2.transform.position.x);

            if (_distance <= _stareEventStartdistance)
            {
                //Debug.Log("�͂ݍ����C�x���g��������");
                if (_player1._getStare() == true && _player2._getStare() == true)
                {
                    if (!_stareEventF)
                    { _stareEventTimer = maxEventTime; }

                    _stareEventF = true;                                // �͂ݍ����C�x���g���J�n����
                    LogController._bActive = false;                     // ���O���\���ɂ���
                    Debug.Log("�͂ݍ����C�x���g�F" + _stareEventF);
                }
            }

            // 
            if (_stareEventF)
            {
                for (int i = 0; i < _stareStagingObj.Length; i++)
                {
                    _stareStagingObj[i].SetActive(true);                      // �C�x���g�^�C�}�[��\������
                }
                int _prevTime = (int)_stareEventTimer;          // 
                _stareEventTime.text = _prevTime.ToString();    // �e�L�X�g���X�V����

                if (_player1._getConsecutive() != _prevConsecutive[0])
                {
                    _stareConsecutiveText[0].rectTransform.localScale = new Vector2(1.0f, 2.0f);
                    _prevConsecutive[0] = _player1._getConsecutive();
                    _stareConsecutiveText[0].text = _player1._getConsecutive().ToString();
                }

                if (_player2._getConsecutive() != _prevConsecutive[1])
                {
                    _stareConsecutiveText[1].rectTransform.localScale = new Vector2(1.0f, 2.0f);
                    _prevConsecutive[1] = _player2._getConsecutive();
                    _stareConsecutiveText[1].text = _player2._getConsecutive().ToString();
                }

                for (int i = 0; i < _stareConsecutiveText.Length; i++)
                {
                    if (_stareConsecutiveText[i].fontSize == 120)
                    {
                        _stareConsecutiveText[i].rectTransform.localScale = new Vector2(1.0f, 1.0f);
                    }
                }


                if (_stareEventTimer >= 0.0f)
                {
                    // �J�E���g�����炷����
                    _stareEventTimer -= Time.deltaTime;
                }
                else
                {
                    _stareEventF = false;
                    if (_player1._getConsecutive() > _player2._getConsecutive())
                    {
                        Debug.Log("player1�̏���!!!!");
                        _player2HP._setConsecutive(true);
                    }
                    else if (_player1._getConsecutive() < _player2._getConsecutive())
                    {
                        Debug.Log("player2�̏���!!!!");
                        _player1HP._setConsecutive(true);
                    }
                    else
                    {
                        Debug.Log("��������!!!!");
                    }

                }
            }
            else
            {
                for (int i = 0; i < _stareStagingObj.Length; i++)
                {
                    _stareStagingObj[i].SetActive(false);                      // �C�x���g�^�C�}�[��\������
                }
            }
        }
    }

    public bool GetStare() 
    { return _stareEventF; }
        
}
