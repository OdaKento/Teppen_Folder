using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontrol : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")] float movespeed;                  // �L�����N�^�[�̈ړ����x���Ǘ�����ϐ�
    [SerializeField, Header("�W�����v��")] float jumppower;                // �L�����N�^�[�̃W�����v�͂��Ǘ�����ϐ�
    [SerializeField, Header("�d��")] float gravityScale;                  // �L�����N�^�[�̏d�͂��Ǘ�����ϐ� 
    [SerializeField, Header("����v���C���[")] GameObject _EnemyPlayer;    // ����v���C���[��gameobject���Ǘ�����ϐ�
    [SerializeField, Header("���[�J�����W�ł̑O��")] private Vector3 _foward = Vector3.forward;  //�O���̊�ƂȂ郍�[�J����ԃx�N�g��
    Rigidbody rb;   //���M�b�h�{�f�B���Ǘ�����ϐ�

    playerattack _playerAttack;

    private Vector2 _inputMove;             // InputSystem �̃R���g���[���[�̃X�e�B�b�N�̍��W���Ǘ�����ϐ�
    private float _inputMove_Degrees;       //InputSystem�̃X�e�B�b�N���͂�x�ɕϊ������l���Ǘ�����ϐ�


    private int player_move_F = 8;              //�i4���i�K�[�h�A���Ⴊ�݁@5���Ⴊ�݁@6���Ⴊ�݁j0�O���W�����v�@1�����W�����v�@2���W�����v 3�K�[�h�A��ށ@8�j���[�g�����@7�O�i�@

    private bool _jumpF = false;            // �W�����v���̃t���O���Ǘ�����ϐ�
    private bool _pressJumpKey;
    private bool _airF = true;              // �󒆂ɂ����Ԃ̃t���O���Ǘ�����ϐ�
    private int air_count;

    [SerializeField]private bool _controlF = true;          // �L�����N�^�[�̑���̉\��Ԃ̃t���O���Ǘ�����

    private int _MoveStop_Count_MAX;        //�������~�߂�t���[����
    private int _MoveStop_Count;            //�������~�߂�t���[�����̃J�E���g    
    private bool _MoveStopF = false;        //�������~�߂�t���O

    private int _LogMove = 8;                   //�ړ��L�[�̃��O���Ǘ�����ϐ�

    private bool _bottunHintActive;

    private bool _GuardF = false;          //�K�[�h����t���O
    private bool _MovefixF = false;          //�������]���Ă���t���O

    private GameObject _EnemyGuardColl = null;

    private int _BackStep = 0;
    private int _BackStepCount = 0;
    private int _BackStepCancelCount = 60;
    private bool _BackStepF = false;

    private Animator _Animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _playerAttack = GetComponent<playerattack>();

        _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�A�j���[�V�����Ǘ�
        _Animator.SetBool("_BackStepAnim", _BackStepF);
        _Animator.SetBool("_Air", _airF);
        _Animator.SetInteger("_MoveFAnim", player_move_F);
        
        

        if (Input.GetKeyDown(KeyCode.A))
        {
            _MoveStop(60);
        }

        if (transform.localEulerAngles.y >= 100.0f)
        {
            _MovefixF = true;
        }
        else
        {
            _MovefixF = false;
        }
        if (_EnemyGuardColl != null)
        {

            if (_EnemyGuardColl.GetComponent<BoxCollider>().enabled == false)
            {
                _GuardF = false;
                //Debug.Log(_EnemyGuardColl.GetComponent<BoxCollider>().enabled);
            }
        }
        if (_BackStepCancelCount <= 0)
        {
            _BackStepCancelCount = 90;
            _BackStep = 0;
        }



        //Debug.Log(_BackStepF);
        //Debug.Log(_BackStep);
    }
    private void FixedUpdate()
    {
        //�v���C���[
        if (_airF)
        {
            rb.position += new Vector3(0, air_count * -gravityScale, 0);
            air_count++;
        }
        // ����\�ȏ�Ԃ̂ݏ����̍X�V������
        if (_controlF)
        {
            _Move();        // �ړ�����������֐�
        }

        // ���͂����Ƃ��ɃW�����v����
        if ((player_move_F == 6 || player_move_F == 7 || player_move_F == 0) && _jumpF == false)
        {
                _jumpF = true;
                _MoveStop(3);
        }
        //�W�����v������֐�
        if (_jumpF)
        {
            _Jump();
        }

        //�������~�߂�
        if (_MoveStopF)
        {
            _MoveStop_Count++;
            if (_MoveStop_Count_MAX <= _MoveStop_Count)
            {
                _MoveStopF = false;
                _MoveStop_Count = 0;
                _MoveStop_Count_MAX = 0;
                if (player_move_F == 9)
                {
                    player_move_F = 8;
                }
                if (_BackStepF)
                {
                    _BackStepF = false;
                    _BackStepCount = 0;
                }
            }
        }
        // Debug.Log(player_move_F);
        if (_BackStep != 0)
        {
            _BackStepCancelCount--;
        }
        if (_BackStepF)
        {
            _BackStepCount++;
            if (_BackStepCount == 15)
            {
                player_move_F = 10;
            }
            if (_BackStepCount == 20)
            {
                player_move_F = 8;
            }
        }
    }

    private void _Move()
    {
        //Debug.Log(player_move_F);
        if (_MovefixF == false)
        {
            //�O�i���͑O���W�����v
            if (player_move_F == 1 || player_move_F == 0)
            {
                rb.velocity = new Vector3(1f * movespeed, 0f, 0f);
            }
            //��ޖ��͌���W�����v�̎�
            else if (player_move_F == 5 || player_move_F == 6)
            {
                if (_GuardF == false)
                {
                    rb.velocity = new Vector3(-0.8f * movespeed, 0f, 0f);

                }
                else
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    Debug.Log("���������[");
                }
            }
            //�m�b�N�o�b�N
            else if (player_move_F == 9)
            {
                rb.velocity = new Vector3(-1f * movespeed, 0f, 0f);
            }
            //backstep
            else if (player_move_F == 10)
            {
                rb.velocity = new Vector3(-5f * movespeed, 0f, 0f);
            }
            else
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
        }
        else if (_MovefixF == true)
        {
            //�O�i���͑O���W�����v
            if (player_move_F == 1 || player_move_F == 0)
            {
                rb.velocity = new Vector3(-1f * movespeed, 0f, 0f);
            }
            //��ޖ��͌���W�����v�̎�
            else if (player_move_F == 5 || player_move_F == 6)
            {
                if (_GuardF == false)
                {
                    rb.velocity = new Vector3(+0.8f * movespeed, 0f, 0f);

                }
                else
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    Debug.Log("���������[");
                }
            }
            //�m�b�N�o�b�N
            else if (player_move_F == 9)
            {
                rb.velocity = new Vector3(+1f * movespeed, 0f, 0f);
            }
            //backstep
            else if (player_move_F == 10)
            {
                rb.velocity = new Vector3(5f * movespeed, 0f, 0f);
            }
            else
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
        }
    }

    //�W�����v���Ǘ�����ϐ�
    private void _Jump()
    {

        rb.position += new Vector3(0, jumppower, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //�X�e�[�W�̒n�ʂƂԂ������Ƃ�
        if (collision.gameObject.CompareTag("stage"))
        {
            //�W�����v��������
            _jumpF = false;
            _airF = false;
            air_count = 0;
            player_move_F = 8;
            //�v���C���[�̌��������킹��
            var dir = new Vector3(_EnemyPlayer.transform.position.x - transform.position.x, 0f, _EnemyPlayer.transform.position.z - transform.position.z);
            var enemydir = new Vector3(transform.position.x - _EnemyPlayer.transform.position.x, 0f, transform.position.z - _EnemyPlayer.transform.position.z);
            var lookatRotation = Quaternion.LookRotation(dir, Vector3.up);
            var enemylookatRotation = Quaternion.LookRotation(enemydir, Vector3.up);
            var offsetRotation = Quaternion.FromToRotation(_foward, Vector3.forward);
            transform.rotation = lookatRotation * offsetRotation;
            _EnemyPlayer.transform.rotation = enemylookatRotation * offsetRotation;
        }

        // Debug.Log("�W�����v�t���O" + _jumpF);
        // Debug.Log("�󒆃t���O" + _airF);
        // Debug.Log("�؋󎞊�" + air_count);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("stage"))
        { _airF = true; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("guard"))
        {
            _EnemyGuardColl = other.gameObject;
            _GuardF = true;
            // Debug.Log("�������Ă܂���[");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("guard"))
        {
            _GuardF = false;
            //Debug.Log("�����܂���");
        }
    }


    // �R���g���[���[�ł̓��͂��擾����֐�
    public void _OnMove(InputAction.CallbackContext context)
    {
        // ����\�ȏ�Ԃ̂݃R���g���[���[�̓��͂��󂯕t����
        // �R���g���[���[�̓��͂̃x�N�g�����擾����
        _inputMove = context.ReadValue<Vector2>(); 

        //Debug.Log(context.ReadValue<Vector2>());
        //�X�e�B�b�N���͂̊p�x�����߂�
        _inputMove_Degrees = Mathf.Atan2(_inputMove.x, _inputMove.y) * Mathf.Rad2Deg;

        if (_inputMove_Degrees < 0f)
        {
            _inputMove_Degrees += 360f;
        }
        Debug.Log(_inputMove_Degrees);

        if (_airF == false && _MoveStopF == false)
        {
            //�����W�����v���Ă����ꍇ
            if ((_inputMove_Degrees >= 337.5f && _inputMove_Degrees <= 360f) || (_inputMove_Degrees >= 0 && _inputMove_Degrees < 22.5))
            {
                player_move_F = 7;
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if (_inputMove_Degrees >= 22.5f + 45f * i && _inputMove_Degrees < 22.5 + 45f * (i + 1))
                    {
                        if (i == 5 && player_move_F != 5 && _BackStep == 0 && transform.localEulerAngles.y <= 100.0f)
                        {
                            _BackStep++;
                        }
                        player_move_F = i;
                        break;
                    }
                }
            }

            if (player_move_F != 9)
            {
                _LogMove = player_move_F;           // �ړ��L�[�̓��̓��O���X�V����
            }

            //�t�����̂Ƃ�
            if (transform.localEulerAngles.y >= 100.0f)
            {
                if (player_move_F == 0)
                {
                    player_move_F = 6;
                }
                else if (player_move_F == 1)
                {
                    if (player_move_F != 1 && _BackStep == 0)
                    {
                        _BackStep++;
                    }
                    player_move_F = 5;
                }
                else if (player_move_F == 2)
                {
                    player_move_F = 4;
                }
                else if (player_move_F == 4)
                {
                    player_move_F = 2;
                }
                else if (player_move_F == 5)
                {
                    player_move_F = 1;
                }
                else if (player_move_F == 6)
                {
                    player_move_F = 0;
                }
                _MovefixF = true;
            }
            if (_BackStep == 2 && player_move_F == 5)
            {
                _MoveStop(30);
                _BackStepF = true;
                _BackStepCancelCount = 60;
            }

        }

        if (context.canceled && _airF == false)
        {
            if (_BackStep == 1)
            {
                _BackStep++;
            }
            player_move_F = 8;
            _LogMove = 8;
        }
    }

    // �v���C���[�̈ړ��̏�Ԃ�Ԃ����\�b�h
    public int _GetMove()
    {
        return player_move_F;
    }

    public int _GetLogMove()
    {
        return _LogMove;
    }

    public void _changeMove(int N)
    {
        if (_airF == false)
        {
            player_move_F = N;
        }

    }

    //�������X�g�b�v�����郁�\�b�h ����i�̓X�g�b�v������t���[����
    public void _MoveStop(int i)
    {
        _MoveStop_Count_MAX = i;
        _MoveStopF = true;
    }

    public bool _Get_MoveStopF()
    {
        return _MoveStopF;
    }

    // �|�[�Y��ʂ̏���
    public void _OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }

    // �{�^���q���g�̕\���E��\����؂�ւ��郁�\�b�h
    public void _OnBottunHint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LogController._bActive = !LogController._bActive;
        }
    }

    public bool _GetBottunHint()
    {
        return _bottunHintActive;
    }

    public void _setKnockBackMAX(int n)
    {
        _MoveStop(n);
        player_move_F = 9;
    }
}
