using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBagControl : playerattack
{
    public int _Hp;      //�v���C���[�̗̑�
    private int _MaxHP = 100;
    private bool _HitHardAttack = false;            //���U��������������  
    private bool _HitWeakAttack = false;            //��U��������������
    private bool _HitEXAttack = false;              //EX�U��������������
    private bool _HitStareAttack = false;            //
    public Image HPimage;       //Slider�̒ǉ�
    private SceneControlManager _sceneControl;
    private PlayerHP _playerHP;             //  PlayerHP�X�N���v�g���Ǘ�����ϐ�

    //�T�E���h�ǉ��p
    [SerializeField, Header("�T�E���h�Ǘ�")] SoundManager soundManager;
    [SerializeField] AudioClip HardHit;
    [SerializeField] AudioClip WeakHit;

    //�G�t�F�N�g�ǉ��p
    [SerializeField, Header("�G�t�F�N�g�Ǘ�")] EffectManager effectManager;
    [SerializeField] ParticleSystem HardHitEffect;          // ���U���̃G�t�F�N�g
    [SerializeField] ParticleSystem WeakHitEffect;          // ��U���̃G�t�F�N�g

    float UITime;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerHP���p��
        _playerHP = GetComponent<PlayerHP>();
        //SceneControlManager���p��
        _sceneControl = GetComponent<SceneControlManager>();
        //�v���C���[��HP���ő��
        _Hp = _MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //�U��������������
        if (_HitHardAttack)
        {
            Debug.Log("�y�E�����W����ex");
            if (soundManager != null)
            {
                soundManager.PlaySe(HardHit);
            }
            effectManager.InstantiateEffect(HardHitEffect, this.gameObject.transform);

            _Hp -= 10;

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

            _Hp -= 3;

            //Debug.Log("slider.value : " + slider.value);
            _HitWeakAttack = false;
        }

        if (_HitStareAttack)
        {
            _Hp -= 4;

            _HitStareAttack = false;
        }

        HPimage.fillAmount = (float)_Hp / _MaxHP;

        UITime += Time.deltaTime;

        //�U�����������ĂȂ���HP�������Ă��鎞5�b���Test1���Ă�
        if (UITime >= 5.0f && _Hp != _MaxHP)
        {
            UITime = 0.0f;
            Test1();
        }
    }

    void Test1()
    {
        _MaxHP = 100;
        _Hp = _MaxHP;
        Debug.Log("Invoke Test");
    }

    //�U���̓����蔻��
    private void OnTriggerEnter(Collider other)
    {

        //���̖т��G��Ă��邩
        if (other.gameObject.CompareTag("Hit"))
        {
            Debug.Log(other.gameObject.name + ":" + other.gameObject.tag);

            Debug.Log(other.gameObject.transform.root.gameObject.name);     // �I�u�W�F�N�g�̈�ԏ�̃I�u�W�F�N�g���Q�Ƃ���

            Debug.Log("���U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._HardAttackF);
            Debug.Log("��U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._WeakAttackF);
            Debug.Log("EX�U���F" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._EXAttackF);

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
}
