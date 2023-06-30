using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResultcController : MonoBehaviour
{

    // Inspector��Łu���͂ƕϐ��v��\������
    [SerializeField,Header("�I�����̃I�u�W�F�N�g")]GameObject[] seleteObj;
    [SerializeField, Header("�I�����̃C���[�W�I�u�W�F�N�g")] Image[] _image;

    [SerializeField] SceneDataBase sceneDataBase;
    [SerializeField] SceneControlManager sceneControlManager;
    [SerializeField] SceneData.SceneMode[] _nextScene;

    private int _maxSelect;             // �I�����̍ő吔���Ǘ�����ϐ�
    private int _minSelect=0;           // �I�����̍ŏ��l���Ǘ�����ϐ�

    Color _prevColor;                   // �N�����̐F�����Ǘ�����ϐ�
    [SerializeField] Color _color;      // �F�����Ǘ�����ϐ�

    private bool _audioFlag;

    private int iNum;

    private void Start()
    {
        _maxSelect = _image.Length;
        iNum = _minSelect;
    }

    private void Update()
    {
        changeColor();
    }

    void changeColor()
    {
        for(int i = 0; i < _maxSelect;i++)
        {
            if(i == iNum)
            {
                _image[i].color = _color;
            }
            else
            {
                _image[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    // �I��ԍ��𑝂₷���\�b�h �u PlayerInput�FDefalutMap(Select)�Őݒ�\�v
    public void _OnCountUp(InputAction.CallbackContext context)
    {
        // �{�^�����������u�Ԃ̂ݏ������s��
        if (context.started)
        {
            iNum++;
            // �I�����Ă���ԍ��𑝂₷
            if(iNum > _maxSelect-1)
            { iNum = _minSelect; }
        }
    }

    // �I��ԍ������炷���\�b�h �u PlayerInput�FDefalutMap(Select)�Őݒ�\�v
    public void _OnCountDown(InputAction.CallbackContext context)
    {
        // �{�^�����������u�Ԃ̂ݏ������s��
        if (context.started)
        {
            iNum--;
            // �I�����Ă���ԍ��𑝂₷
            if (iNum < _minSelect)
            { iNum = _maxSelect - 1 ; }
        }
    }

    // ����{�^���������ꂽ���̏������\�b�h �u PlayerInput�FDefalutMap(Select)�Őݒ�\�v
    public void _OnApply(InputAction.CallbackContext context)
    {   
        // ����{�^���������ꂽ�u�Ԃ� �V�[����؂�ւ��A���ʉ���炷
        if(context.started && !_audioFlag)
        {
            sceneControlManager.changeSceneWithSceneControl(iNum, 0.0f);
            _audioFlag = true;                          // ���ʉ���炷�t���O�� ON �ɂ���
        }
    }

}
