using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerattack : MonoBehaviour
{
    // �{�^�����͂��Ǘ�����񋓌^�̕ϐ�
    enum Button
    {
        x = 1 << 0,         // 0b0000_0001 = 1
        y = 1 << 1,         // 0b0000_0010 = 2
        b = 1 << 2,         // 0b0000_0100 = 4
        a = 1 << 3,         // 0b0000_1000 = 8
        rb = 1 << 4,        // 0b0001_0000 = 16
        lb = 1 << 5,        // 0b0010_0000 = 32
        rt = 1 << 6,        // 0b0100_0000 = 64
        lt = 1 << 7,        // 0b1000_0000 = 128
    };

    private uint _prevbit = 0b_0000;                    // �{�^���������ꂽ����bit�ŊǗ�����ϐ�

    protected playercontrol _playercontrol;

    //��U��
    public bool _HardAttackF = false;                  //��U���̃t���O
    public bool _HardAttack_CollisionF = false;        //��U���̓����蔻��o�����̃t���O
    private int _HardAttack_Count = 0;                 //��U���̃J�E���g
    [SerializeField, Header("��U���̔���J�n�t���[��")] private int _HardAttack_StartCollision;    //��U���̓����蔻��J�n�t���[��
    [SerializeField, Header("��U���̔���I���t���[��")] private int _HardAttack_FinishCollision;   //��U���̓����蔻��I���t���[��

    //��U��
    public bool _WeakAttackF = false;                  //��U���̃t���O
    public bool _WeakAttack_CollisionF = false;        //��U���̓����蔻��o�����̃t���O
    private int _WeakAttack_Count = 0;                 //��U���̃J�E���g
    [SerializeField, Header("��U���̔���J�n�t���[��")] private int _WeakAttack_StartCollision;     //��U���̓����蔻��J�n�t���[��
    [SerializeField, Header("��U���̔���I���t���[��")] private int _WeakAttack_FinishCollision;    //��U���̓����蔻��I���t���[��

    //�K�E�Z
    public bool _EXAttackF = false;                    //�K�E�Z�̃t���O
    public bool _EXAttack_CollisionF = false;          //�K�E�Z�̓����蔻��o�����̃t���O
    private int _EXAttack_Count = 0;                   //�K�E�Z�̃J�E���g
    [SerializeField, Header("�K�E�Z�̔���J�n�t���[��")] private int _EXAttack_StartCollision;    //�K�E�Z�̓����蔻��J�n�t���[��
    [SerializeField, Header("�K�E�Z�̔���I���t���[��")] private int _EXAttack_FinishCollision;   //�K�E�Z�̓����蔻��I���t���[��
                                                                                      //�K�E�Z
    public bool _stareF = false;                       //�K�E�Z�̃t���O
    private int _stare_Count = 0;                      //�K�E�Z�̃J�E���g
    [SerializeField, Header("�͂ݍ����̔���J�n�t���[��")] private int _stare_StartCollision;    //�K�E�Z�̓����蔻��J�n�t���[��
    [SerializeField, Header("�͂ݍ����̔���I���t���[��")] private int _stare_FinishCollision;   //�K�E�Z�̓����蔻��I���t���[��
    private bool _StareF;
    private bool _simultaneouslyAttackF = false;

    [SerializeField, Header("�K�[�h���[�V�������N���������蔻��")] private GameObject _GuardColl;
    [SerializeField, Header("�U���̓����蔻��")] private GameObject _AttackColl;

    // �A�Ő����Ǘ�����ϐ�
    int _consecutiveButtonCount;

    [SerializeField]stareStagingControl _stareControl;                 //�͂ݍ����C�x���g

    //�A�j���[�^�[�Ǘ�
    protected Animator _Animator;

    // Start is called before the first frame update
    void Start()
    {
        _playercontrol = GetComponent<playercontrol>();
        _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_stareControl != null)
        {
            _stareF = _stareControl.GetStare();
        }
        else
        { _stareF = false; }
        //�A�j���[�^�[�Ǘ�
        _Animator.SetBool("_HardAttack",_HardAttackF);
        _Animator.SetBool("_WeakAttack",_WeakAttackF);

        Debug.Log("�U����ԁF" + _WeakAttackF);

        // �͂ݍ����C�x���g���ł͂Ȃ��ꍇ
        if (!_stareF)
        {
            // �A�Ő�������������
            if(_consecutiveButtonCount != 0)
            { _consecutiveButtonCount = 0; }

            _SimultaneouslyAttack(_prevbit, 0b_1100_0000, "RT,LT", 30);     // rt,lt�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0011_0000, "RB,LB", 30);     // rb,lb�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0000_0011, "X,Y", 30);       // x,y�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0000_0110, "Y,B", 30);       // y,b�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0000_1100, "B,A", 30);       // b,a�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0000_1001, "A,X", 30);       // a,x�𓯎�����
            _SimultaneouslyAttack(_prevbit, 0b_0000_0001, "X", 31);         // x������
            _SimultaneouslyAttack(_prevbit, 0b_0000_0010, "Y", 11);         // y������
            _SimultaneouslyAttack(_prevbit, 0b_0000_0100, "B", 42);         // b������
            _SimultaneouslyAttack(_prevbit, 0b_0000_1000, "A", 31);         // a������

            if (_prevbit == 0)
            {
                _simultaneouslyAttackF = false;
            }

            //��U���̏ꍇ
            //��U���t���O��true�̎�
            if (_HardAttackF)
            {
                _Attack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _HardAttack_Count = 0;
            }

            //�U���̓����蔻��Ǘ�
            if (_HardAttack_CollisionF)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = true;

            }
            else if (_WeakAttack_CollisionF == false && _EXAttack_CollisionF == false && _HardAttack_CollisionF == false)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = false;
                _GuardColl.GetComponent<BoxCollider>().enabled = false;

            }

            if (_playercontrol._Get_MoveStopF() == false)
            {
                _HardAttackF = false;
            }

            //��U���̏ꍇ
            if (_WeakAttackF)
            {
                _WeakAttack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _WeakAttack_Count = 0;
            }

            //�U���̓����蔻��Ǘ�
            if (_WeakAttack_CollisionF)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = true;
                //Debug.Log("�㓖���蔻�肪�o��");
            }
            else if (_HardAttack_CollisionF == false && _EXAttack_CollisionF == false && _WeakAttack_CollisionF == false)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = false;
                _GuardColl.GetComponent<BoxCollider>().enabled = false;
                //Debug.Log("�㓖���蔻�肪�����Ȃ���");
            }

            if (_playercontrol._Get_MoveStopF() == false)
            {
                _WeakAttackF = false;
            }

            //�K�E�Z�̏ꍇ
            if (_EXAttackF)
            {
                _EXAttack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _EXAttack_Count = 0;
            }

            //�U���̓����蔻��Ǘ�
            if (_EXAttack_CollisionF)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = true;
                //Debug.Log("EX�����蔻�肪�o��");
            }
            else if (_HardAttack_CollisionF == false && _WeakAttack_CollisionF == false)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = false;
                _GuardColl.GetComponent<BoxCollider>().enabled = false;
            }

            if (_playercontrol._Get_MoveStopF() == false)
            {
                _EXAttackF = false;
            }

            //�݂͂̏ꍇ
            if (!_stareF)
            {
                _stare_Count = 0;
            }

            if (_playercontrol._Get_MoveStopF() == false)
            {
                _stareF = false;
            }
        }
        else
        {
            Debug.Log(this.gameObject.name+ "�F" + _consecutiveButtonCount );
        }
    }
    private void FixedUpdate()
    {
        // �͂ݍ����C�x���g���ł͂Ȃ��ꍇ
        if (!_stareF)
        {
            //��U���̃t���O��true�̎�
            if (_HardAttackF)
            {
                _HardAttack_Count++;
            }
            if (_HardAttack_CollisionF)
            {
                //Debug.Log("�������蔻�肪�o��");
            }
            //��U���̃t���O��true�̎�
            if (_WeakAttackF)
            {
                _WeakAttack_Count++;
            }

            //�K�E�Z�̃t���O��true�̎�
            if (_EXAttackF)
            {
                _EXAttack_Count++;
            }

            //�͂ݍ����t���O��true�̎�
            if (_stareF)
            {
                _stare_Count++;
            }
        }
    }

    //�U���̓����蔻����o��������t���[���Ǘ�
    private void _Attack_Collision_Start()
    {
        if (_HardAttack_StartCollision == _HardAttack_Count)
        {
            _HardAttack_CollisionF = true;
        }
        if (_HardAttack_FinishCollision == _HardAttack_Count)
        {
            _HardAttack_CollisionF = false;
        }
    }

    //��U���̓����蔻����o��������t���[���Ǘ�
    private void _WeakAttack_Collision_Start()
    {
        if (_WeakAttack_StartCollision == _WeakAttack_Count)
        {
            _WeakAttack_CollisionF = true;
        }
        if (_WeakAttack_FinishCollision == _WeakAttack_Count)
        {
            _WeakAttack_CollisionF = false;
        }
    }

    //�K�E�Z�̓����蔻����o��������t���[���Ǘ�
    private void _EXAttack_Collision_Start()
    {
        if (_EXAttack_StartCollision == _EXAttack_Count)
        {
            _EXAttack_CollisionF = true;
        }
        if (_EXAttack_FinishCollision == _EXAttack_Count)
        {
            _EXAttack_CollisionF = false;
        }
    }

    // �A�Ő����X�V���郁�\�b�h
    private void _ConsecutiveButton()
    {
        // �͂ݍ����C�x���g���������Ă���ꍇ
        if(_stareControl.GetStare())
        {
            _consecutiveButtonCount++;      // �A�ł̐����X�V����
        }
    }

    //input��Attack�Ɋ���U���Ă���L�[����͂��ꂽ�Ƃ�
    public void pressX(InputAction.CallbackContext context)
    {

        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 1 )���X�V����
        {
            // �����ꂽX�{�^���̃t���O�����Ă� ( 0b0001 )
            if (context.started)
            {
                _prevbit = _prevbit | (uint)Button.x;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �A�Ő������m����
                _ConsecutiveButton();
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.x);
            }
        }
    }

    public void pressY(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 2 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽY�{�^���̃t���O�����Ă� ( 0b0010 )
                _prevbit = _prevbit | (uint)Button.y;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �A�Ő������m����
                _ConsecutiveButton();
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.y);
            }
        }
    }

    public void pressB(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 4 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽB�{�^���̃t���O�����Ă� ( 0b0100 )
                _prevbit = _prevbit | (uint)Button.b;
            }
            if (context.canceled)
            {
                // �A�Ő������m����
                _ConsecutiveButton();
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.b);
            }
        }
    }

    public void pressA(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 8 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽB�{�^���̃t���O�����Ă� ( 0b1000 )
                _prevbit = _prevbit | (uint)Button.a;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �A�Ő������m����
                _ConsecutiveButton();
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.a);
            }
        }
    }

    public void pressRB(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 16 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽB�{�^���̃t���O�����Ă� ( 0b0001_0000 )
                _prevbit = _prevbit | (uint)Button.rb;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.rb);
            }
        }
    }

    public void pressLB(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 32 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽB�{�^���̃t���O�����Ă� ( 0b0010_0000 )
                _prevbit = _prevbit | (uint)Button.lb;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.lb);
            }
        }
    }

    public void pressRT(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 64 )���X�V����
        {
            // �����ꂽB�{�^���̃t���O�����Ă� ( 0b0100_0000 )
            if (context.started)
            {
                _prevbit = _prevbit | (uint)Button.rt;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.rt);
            }
        }
    }

    public void pressLT(InputAction.CallbackContext context)
    {
        // �{�^�����͂����ꂽ�ۂɁA�r�b�g�̒l( 128 )���X�V����
        {
            if (context.started)
            {
                // �����ꂽB�{�^���̃t���O�����Ă� ( 0b1000_0000 )
                _prevbit = _prevbit | (uint)Button.lt;            // �����ꂽ�{�^���̒l��������
            }
            if (context.canceled)
            {
                // �r�b�g�����ɖ߂�
                _setButtonBit(~(uint)Button.lt);
            }
        }
    }

    // 
    private void _SimultaneouslyAttack(uint _inputBit,uint _judgeBit,string _buttonName,int _stopFrame)
    {
        // ���������̔���
        // �����n�߂��Ƃ�
        if ((_inputBit & _judgeBit) == _judgeBit  && _playercontrol._Get_MoveStopF() == false)
        {   
            // �U���t���O���Ǘ����鏈�� (�U���t���O�������Œǉ����鎖�ōU�����J�n����)
            {
                //------------------------------------------------
                // �{�^�����������̏ꍇ�̏���
                //------------------------------------------------
                // RT,LT�{�^�������͂���
                if (_inputBit == 0b1100_0000)
                {  }

                // RB,LB�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0011_0000)
                { _EXAttackF = true; }

                // X,A�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0000_1001)
                {
                    Debug.Log("�����Z");
                    _stareF = true; }

                //------------------------------------------------
                // �{�^���P�̉����̏ꍇ�̏���
                //------------------------------------------------
                // X�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0000_0001)
                { _HardAttackF = true; }

                // Y�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0000_0010)
                { _WeakAttackF = true; }
                
                // B�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0000_0100)
                {  }
                
                // A�{�^�������͂��ꂽ��
                else if (_inputBit == 0b0000_1000)
                {  }
            }

            // ��x�������͂��󂯕t����
            if (!_simultaneouslyAttackF)
            {
                _simultaneouslyAttackF = true;
                // Debug.Log(_buttonName + "���������܂���");
                _playercontrol._MoveStop(_stopFrame);
                _playercontrol._changeMove(8);
            }
        }
    }

    // �{�^���̃r�b�g���擾���郁�\�b�h
    public uint _getButtonBit()
	{
        return _prevbit;
	}

    public void _setButtonBit(uint _inputBit)
	{
        _prevbit = _prevbit & _inputBit;
        _stareF = false;
	}

    public bool _get_HardAttack_CollisionF()
    {
        return _HardAttack_CollisionF;
    }

    public bool _getStare()
    { return _stareF; }

    public int _getConsecutive()
    { return _consecutiveButtonCount; }

}
