using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputLogControl : MonoBehaviour
{
    [SerializeField, Header("���O�̕\�����s���I�u�W�F�N�g")] private playercontrol _targetObject;
    [SerializeField, Header("�\�����郍�O�̐�")] private int _maxLog = 4;

    [SerializeField, Header("�A�N�V�����C���[�W�I�u�W�F�N�g")] private Image[] _ActionImages;
    [SerializeField, Header("�摜")] private Sprite [] _sprite;


    private List<int> _Actionlist = new List<int>();
    private List<Image> _ActionlistImage = new List<Image>();

    private GameObject[] _prevImages;

    private int _MoveType;
    private int _prevMoveType;

    uint flame = 0b_0001;

    // Start is called before the first frame update
    void Start()
    {
        _targetObject.GetComponent<playercontrol>();
        for (int i = 0; i < _ActionImages.Length; i++)
        {
            if (_ActionImages[i].sprite == null)
            {
                _ActionImages[i].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        flame = flame << 1;

        if(flame >= 0b_1000_0000_0000)
		{
            flame = 1;
        
            _MoveType = _targetObject._GetLogMove();
            // ������Ԃł͂Ȃ��ꍇ�Ƀ��O��\������
            if (_MoveType != 8)
            {
                //Debug.Log("�R���g���[���[���W�F" + _MoveType);
                _prevMoveType = _MoveType;                      // �v���C���[�̓������ꎟ�I�ɕۑ�����
                _Actionlist.Insert(0, _prevMoveType);

                for (int i = 0; i < _Actionlist.Count; i++)
                {
                    _ActionImages[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        _ActionImages[i].sprite = _sprite[(int)_Actionlist[i]];
                    
                    //Debug.Log("�A�N�V�������X�g["+ i + "]�F" + _Actionlist[i]);
                    //Debug.Log("�A�N�V�������X�g[" + i + "]�F" + _ActionImages[i].sprite);
                    if (i == _maxLog)
                    {
                        // �ߋ��̃��O���폜����
                        _Actionlist.RemoveAt(_maxLog);
                        // _ActionlistImage.RemoveAt(_maxLog);
                    }
                }
            }
		}



    }


}
