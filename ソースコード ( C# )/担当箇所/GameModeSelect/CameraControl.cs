using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Header("�J�����ړ����x")] float _cameraMove;

    [SerializeField, Header("�Q�[�����[�h�����n�_")] GameObject[] GamemodeClassroom;

    [SerializeField, Header("�e�L�X�g�I�u�W�F�N�g")] Text[] _textObject;

    [SerializeField, Header("�e�L�X�g�I�u�W�F�N�g_�M")] GameObject[] _anim;

    [SerializeField, Header("�V�[���̐�")] SceneData.SceneMode[] _scene;

    [SerializeField, Header("SceneManager")] SceneControlManager _sceneManager;

    [SerializeField] SoundManager soundManager;


    private bool _isAudioEnd;

    private Color _prevcolor;

    bool bSelect = false;       // ���[�h�̑I���\�����Ǘ�����t���O


    Vector3 distanceMove;   // �����Ԃł̋���

    int maxNumber;          // �����̍ő吔
    int minNumber;          // �����̍ŏ���

    int prevSceneNumber;    // �V�[�����Ƃ̔z��̗v�f�����Ǘ�����ϐ�
    private int modeNumber; // 

    int _prevtextSize;      // �e�L�X�g�t�H���g�T�C�Y

    float move;

    int changeCount = 0;
    bool changeF = false;

    // Start is called before the first frame update
    void Start()
    {
        
        bSelect = false;

        for (int i = 0; i < _textObject.Length; i++)
        {
            _textObject[i].text = _sceneManager.getSceneTextName(_scene[i]);
        }

        _prevtextSize = _textObject[0].fontSize;            // Fontsize���ꎞ�I�ɕۑ�����
        _prevcolor = _textObject[0].color;



        minNumber = modeNumber;                             // minNumber�̏�����������
        maxNumber = GamemodeClassroom.Length;               // maxNumber��������������
        Debug.Log(maxNumber);
    }

    // Update is called once per frame
    void Update()
    {
        // �����̏ꏊ���X�V���鏈��
        _targetRoom();

        // �J�����̍��W���X�V���鏈��
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (distanceMove.z * Time.deltaTime));

        if (_isAudioEnd)
        {
            _isAudioEnd = false;

        }
    }

    private void FixedUpdate()
    {
        if (changeF)
        {
            changeCount++;
            if (changeCount > 60)
            {
                _sceneManager.changeSceneWithSceneControl(modeNumber, 1.0f);
            }
        }
    }


    // �J�������I�������Q�[�����[�h�Ɉړ����郁�\�b�h
    void _targetRoom()
    {
        for (int i = 0; i < GamemodeClassroom.Length; i++)
        {
            // �����̔ԍ�����v�����ꍇ�A�ړ��ʂ��X�V����
            if (i == modeNumber)
            {
                distanceMove = GamemodeClassroom[i].transform.position - this.transform.position;
                _textObject[i].fontSize = 100;
                _textObject[i].color = _prevcolor;
                // Debug.Log(distanceMove);

                // �J�����̂y���̈ړ��ʂ��Βl�ɂ��ĕۑ�����
                move = Mathf.Abs(distanceMove.z);
            }
            else
            {
                // �e�L�X�g�̃T�C�Y��߂�
                _textObject[i].fontSize = _prevtextSize;
                _textObject[i].color = new Color(_prevcolor.r, _prevcolor.g, _prevcolor.b, 0.25f);
            }
        }
    }

    // �I�����𑝂₷����
    public void _OnCountUp(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            Debug.Log("�J�E���g�A�b�v");
            modeNumber++;
        }

        if (modeNumber >= maxNumber)
        {
            modeNumber = minNumber;
        }
    }

    // �I���������炷����
    public void _OnCountDown(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            Debug.Log("�J�E���g�_�E��");
            modeNumber--;
        }

        if (modeNumber < minNumber)
        {
            modeNumber = maxNumber - 1;
        }
    }

    // �I���������肷�鏈��
    public void _OnApply(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            bSelect = true;
            changeF = true;
            // ���ʉ�����x�����Đ�����
            if (!_isAudioEnd)
            {
                soundManager.bgmStop();
                _isAudioEnd = true;    // 
                _anim[modeNumber].GetComponent<Image>().enabled = true;
                _anim[modeNumber].GetComponent<Animator>().SetBool("hudeanim", _isAudioEnd);

            }
            prevSceneNumber = modeNumber;       //                                        // 
        }

    }
}
