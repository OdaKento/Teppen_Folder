using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------
// ���e�F�{�^�����͂̃��O���Ǘ�����
// ����ҁF21cu0206_���c���l
//-------------------------------------------------------------
public class ButtonLogControl : MonoBehaviour
{
    [SerializeField,Header("�v���C���[�I�u�W�F�N�g")]playerattack _playerAttack;                // 
    uint _prevBit;

    [SerializeField, Header("�{�^���̉摜�I�u�W�F�N�g")] Image[] _buttonIcons;                  // �{�^���̉摜���Ǘ�����ϐ�

    private Vector2 defalutIconScale;

    // Start is called before the first frame update
    void Start()
    {
        defalutIconScale =new Vector2( _buttonIcons[0].rectTransform.localScale.x, _buttonIcons[0].rectTransform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        // �{�^���������ꂽ�ۂ̍X�V����
        _prevBit = _playerAttack._getButtonBit();

        _ActionButton(_prevBit, 0b0001, 0 );     // X�{�^���̓��͂��󂯕t����
        _ActionButton(_prevBit, 0b0010, 1 );     // Y�{�^���̓��͂��󂯕t����
        _ActionButton(_prevBit, 0b0100, 2 );     // B�{�^���̓��͂��󂯕t����
        _ActionButton(_prevBit, 0b1000, 3 );     // A�{�^���̓��͂��󂯕t����

    }

    // �{�^����UI�̑傫�����Ǘ����郁�\�b�h
    private void _ActionButton(uint _bit,uint _compainBit,int colorNum)
	{
        
        // �r�b�g�ƈ�v�������Ƀ{�^���̐F��ύX����
        if ((_bit & _compainBit)  == _compainBit)
        {
            // �{�^���̃A�C�R���̑傫����ύX����
            _buttonIcons[colorNum].rectTransform.localScale = new Vector2(defalutIconScale.x * 0.25f, defalutIconScale.y * 0.25f);          
        }
        else 
        {
            // �{�^���̃A�C�R���̑傫�������ɖ߂�
            _buttonIcons[colorNum].rectTransform.localScale = new Vector2(defalutIconScale.x, defalutIconScale.y);                          
        }

        
    }
}
