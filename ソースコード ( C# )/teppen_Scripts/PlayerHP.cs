using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : playerattack
{
    public int _PlayerHp;      //�v���C���[�̗̑�
    private int _MaxHP = 100;
    private bool _HitHardAttack = false;            //���U��������������  
    private bool _HitWeakAttack = false;            //��U��������������
    private bool _HitEXAttack = false;              //EX�U��������������
    private bool _HitStareAttack = false;           //

    [SerializeField, Header("HP�o�[�̃C���[�W�摜")] Image _HPBarImage;                       //HPBar�̃C���[�W�摜��ǉ�
    private playerattack _pAttack;

    //�T�E���h�ǉ��p
    [SerializeField, Header("�T�E���h�Ǘ�")] SoundManager soundManager;
    [SerializeField] AudioClip HardHit;
    [SerializeField] AudioClip WeakHit;

    //�G�t�F�N�g�ǉ��p
    [SerializeField, Header("�G�t�F�N�g�Ǘ�")] EffectManager effectManager;
    [SerializeField] ParticleSystem HardHitEffect;          // ���U���̃G�t�F�N�g
    [SerializeField] ParticleSystem WeakHitEffect;          // ��U���̃G�t�F�N�g

    int iAttack = 0;
    private bool HardHitAnim;
    private bool WeakHitAnim;

    // Start is called before the first frame update
    void Start()
    {
        _pAttack = GetComponent<playerattack>();
        //�v���C���[��HP���ő��
        _PlayerHp = _MaxHP;

        _playercontrol = GetComponent<playercontrol>();

        _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _Animator.SetBool("_HitHardAttack", _HitHardAttack);
        _Animator.SetBool("_HitWeakAttack", _HitWeakAttack);


        //�U��������������
        if (_HitHardAttack)
        {

            Debug.Log("�y�E�����W����ex");
            if (soundManager != null)
            {
                soundManager.PlaySe(HardHit);
            }
            effectManager.InstantiateEffect(HardHitEffect, this.gameObject.transform);
            if (_playercontrol._GetMove() != 5)
            {
                _PlayerHp -= 10;
                
                _playercontrol._setKnockBackMAX(20);
            }
            _playercontrol._setKnockBackMAX(10);
            //Debug.Log("slider.value : " + slider.value);
            _HitHardAttack = false;
        }
        if (_HitWeakAttack)
        {
            
            Debug.Log("�t�H���g�Xex");
            if (soundManager != null)
            {
                soundManager.PlaySe(WeakHit);
            }
            effectManager.InstantiateEffect(WeakHitEffect, this.gameObject.transform);
            if (_playercontrol._GetMove() != 3)
            {
                _PlayerHp -= 3;
                _playercontrol._setKnockBackMAX(6);
            }

            //Debug.Log("slider.value : " + slider.value);
            _HitWeakAttack = false;
        }

        if (_HitStareAttack)
        {
            _PlayerHp -= 4;

            _HitStareAttack = false;
        }

        //HP��0�ɂȂ����烊�U���g�Ɉړ�����
        if (_PlayerHp <= 0)
        {
            StartCoroutine("Defeat");
        }

        if (_HPBarImage != null)
        {
            _HPBarImage.fillAmount = (float)_PlayerHp / _MaxHP;
            //Debug.Log(_PlayerHp);
        }

    }
    private void FixedUpdate()
	{
        if (_HitEXAttack)
        {
            if (iAttack % 2 == 0)
            {
                if (_playercontrol._GetMove() == 5)
                {
                    _PlayerHp -= 1;
                }
                else
                {
                    _PlayerHp -= 5;
                }
            }
            iAttack++;

            if (iAttack % 16 == 0)
            { _HitEXAttack = false; }
        }
	}

    //�U���̓����蔻��
    private void OnTriggerEnter(Collider other)
    {

        //���̖т��G��Ă��邩
        if (other.gameObject.CompareTag("Hit"))
        {
            //Debug.Log(other.gameObject.name + ":" + other.gameObject.tag);

            //Debug.Log(other.gameObject.transform.root.gameObject.name);     // �I�u�W�F�N�g�̈�ԏ�̃I�u�W�F�N�g���Q�Ƃ���

            //Debug.Log("���U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._HardAttackF);
            //Debug.Log("��U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._WeakAttackF);
            //Debug.Log("EX�U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._EXAttackF);

            //�U�����肪�o�Ă��鎞Hit�t���O���グ��
            if (other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._HardAttackF)
            {
                Debug.Log("[2] other.gameObject.tag : " + other.gameObject.tag);
                _HitHardAttack = true;
            }
            if (other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._WeakAttackF)
            {
                Debug.Log("[2] other.gameObject.tag : " + other.gameObject.tag);
                _HitWeakAttack = true;
            }
			if (other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._EXAttackF)
			{
				//Debug.Log("[2] other.gameObject.tag : " + other.gameObject.tag)
				_HitEXAttack = true;
			}
		}
    }

    IEnumerator Defeat()
    {
        //�����������x�点��KO�Ƃ��o��悤�ɂ���


        // 1�b�x�点��
        yield return new WaitForSeconds(1);

        //���U���g�Ɉړ�
        //Debug.Log("�o���Ă��I");
    }

    public void _setConsecutive(bool _Consecutive)
    {
        _HitStareAttack = _Consecutive;
    }

    //�v���C���[��HP��Ԃ��v���O����
    public int _getPlayerHP()
    {
        return _PlayerHp;
    }
}
