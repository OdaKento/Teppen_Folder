using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandBagControl : playerattack
{
    public int _Hp;      //プレイヤーの体力
    private int _MaxHP = 100;
    private bool _HitHardAttack = false;            //強攻撃が当たったか  
    private bool _HitWeakAttack = false;            //弱攻撃が当たったか
    private bool _HitEXAttack = false;              //EX攻撃が当たったか
    private bool _HitStareAttack = false;            //
    public Image HPimage;       //Sliderの追加
    private SceneControlManager _sceneControl;
    private PlayerHP _playerHP;             //  PlayerHPスクリプトを管理する変数

    //サウンド追加用
    [SerializeField, Header("サウンド管理")] SoundManager soundManager;
    [SerializeField] AudioClip HardHit;
    [SerializeField] AudioClip WeakHit;

    //エフェクト追加用
    [SerializeField, Header("エフェクト管理")] EffectManager effectManager;
    [SerializeField] ParticleSystem HardHitEffect;          // 強攻撃のエフェクト
    [SerializeField] ParticleSystem WeakHitEffect;          // 弱攻撃のエフェクト

    float UITime;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerHPを継承
        _playerHP = GetComponent<PlayerHP>();
        //SceneControlManagerを継承
        _sceneControl = GetComponent<SceneControlManager>();
        //プレイヤーのHPを最大に
        _Hp = _MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //攻撃が当たったか
        if (_HitHardAttack)
        {
            Debug.Log("ペ・ヨンジュンex");
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

            Debug.Log("フォレトスex");
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

        //攻撃が当たってなくてHPが減っている時5秒後にTest1を呼ぶ
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

    //攻撃の当たり判定
    private void OnTriggerEnter(Collider other)
    {

        //髪の毛が触れているか
        if (other.gameObject.CompareTag("Hit"))
        {
            Debug.Log(other.gameObject.name + ":" + other.gameObject.tag);

            Debug.Log(other.gameObject.transform.root.gameObject.name);     // オブジェクトの一番上のオブジェクトを参照する

            Debug.Log("強攻撃：" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._HardAttackF);
            Debug.Log("弱攻撃：" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._WeakAttackF);
            Debug.Log("EX攻撃：" + other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._EXAttackF);

            //攻撃判定が出ている時Hitフラグを上げる
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
