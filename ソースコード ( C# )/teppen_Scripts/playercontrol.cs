using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontrol : MonoBehaviour
{
    [SerializeField, Header("移動速度")] float movespeed;                  // キャラクターの移動速度を管理する変数
    [SerializeField, Header("ジャンプ力")] float jumppower;                // キャラクターのジャンプ力を管理する変数
    [SerializeField, Header("重力")] float gravityScale;                  // キャラクターの重力を管理する変数 
    [SerializeField, Header("相手プレイヤー")] GameObject _EnemyPlayer;    // 相手プレイヤーのgameobjectを管理する変数
    [SerializeField, Header("ローカル座標での前方")] private Vector3 _foward = Vector3.forward;  //前方の基準となるローカル空間ベクトル
    Rigidbody rb;   //リギッドボディを管理する変数

    playerattack _playerAttack;

    private Vector2 _inputMove;             // InputSystem のコントローラーのスティックの座標を管理する変数
    private float _inputMove_Degrees;       //InputSystemのスティック入力を度に変換した値を管理する変数


    private int player_move_F = 8;              //（4下段ガード、しゃがみ　5しゃがみ　6しゃがみ）0前方ジャンプ　1垂直ジャンプ　2後ろジャンプ 3ガード、後退　8ニュートラル　7前進　

    private bool _jumpF = false;            // ジャンプ中のフラグを管理する変数
    private bool _pressJumpKey;
    private bool _airF = true;              // 空中にいる状態のフラグを管理する変数
    private int air_count;

    [SerializeField]private bool _controlF = true;          // キャラクターの操作の可能状態のフラグを管理する

    private int _MoveStop_Count_MAX;        //動きを止めるフレーム数
    private int _MoveStop_Count;            //動きを止めるフレーム数のカウント    
    private bool _MoveStopF = false;        //動きを止めるフラグ

    private int _LogMove = 8;                   //移動キーのログを管理する変数

    private bool _bottunHintActive;

    private bool _GuardF = false;          //ガードするフラグ
    private bool _MovefixF = false;          //動き反転しているフラグ

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
        //アニメーション管理
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
        //プレイヤー
        if (_airF)
        {
            rb.position += new Vector3(0, air_count * -gravityScale, 0);
            air_count++;
        }
        // 操作可能な状態のみ処理の更新をする
        if (_controlF)
        {
            _Move();        // 移動処理をする関数
        }

        // 入力したときにジャンプする
        if ((player_move_F == 6 || player_move_F == 7 || player_move_F == 0) && _jumpF == false)
        {
                _jumpF = true;
                _MoveStop(3);
        }
        //ジャンプさせる関数
        if (_jumpF)
        {
            _Jump();
        }

        //動きを止める
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
            //前進又は前方ジャンプ
            if (player_move_F == 1 || player_move_F == 0)
            {
                rb.velocity = new Vector3(1f * movespeed, 0f, 0f);
            }
            //後退又は後方ジャンプの時
            else if (player_move_F == 5 || player_move_F == 6)
            {
                if (_GuardF == false)
                {
                    rb.velocity = new Vector3(-0.8f * movespeed, 0f, 0f);

                }
                else
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    Debug.Log("うごけんよー");
                }
            }
            //ノックバック
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
            //前進又は前方ジャンプ
            if (player_move_F == 1 || player_move_F == 0)
            {
                rb.velocity = new Vector3(-1f * movespeed, 0f, 0f);
            }
            //後退又は後方ジャンプの時
            else if (player_move_F == 5 || player_move_F == 6)
            {
                if (_GuardF == false)
                {
                    rb.velocity = new Vector3(+0.8f * movespeed, 0f, 0f);

                }
                else
                {
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    Debug.Log("うごけんよー");
                }
            }
            //ノックバック
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

    //ジャンプを管理する変数
    private void _Jump()
    {

        rb.position += new Vector3(0, jumppower, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //ステージの地面とぶつかったとき
        if (collision.gameObject.CompareTag("stage"))
        {
            //ジャンプを初期化
            _jumpF = false;
            _airF = false;
            air_count = 0;
            player_move_F = 8;
            //プレイヤーの向きを合わせる
            var dir = new Vector3(_EnemyPlayer.transform.position.x - transform.position.x, 0f, _EnemyPlayer.transform.position.z - transform.position.z);
            var enemydir = new Vector3(transform.position.x - _EnemyPlayer.transform.position.x, 0f, transform.position.z - _EnemyPlayer.transform.position.z);
            var lookatRotation = Quaternion.LookRotation(dir, Vector3.up);
            var enemylookatRotation = Quaternion.LookRotation(enemydir, Vector3.up);
            var offsetRotation = Quaternion.FromToRotation(_foward, Vector3.forward);
            transform.rotation = lookatRotation * offsetRotation;
            _EnemyPlayer.transform.rotation = enemylookatRotation * offsetRotation;
        }

        // Debug.Log("ジャンプフラグ" + _jumpF);
        // Debug.Log("空中フラグ" + _airF);
        // Debug.Log("滞空時間" + air_count);
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
            // Debug.Log("当たってますよー");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("guard"))
        {
            _GuardF = false;
            //Debug.Log("消えました");
        }
    }


    // コントローラーでの入力を取得する関数
    public void _OnMove(InputAction.CallbackContext context)
    {
        // 操作可能な状態のみコントローラーの入力を受け付ける
        // コントローラーの入力のベクトルを取得する
        _inputMove = context.ReadValue<Vector2>(); 

        //Debug.Log(context.ReadValue<Vector2>());
        //スティック入力の角度を求める
        _inputMove_Degrees = Mathf.Atan2(_inputMove.x, _inputMove.y) * Mathf.Rad2Deg;

        if (_inputMove_Degrees < 0f)
        {
            _inputMove_Degrees += 360f;
        }
        Debug.Log(_inputMove_Degrees);

        if (_airF == false && _MoveStopF == false)
        {
            //垂直ジャンプしていた場合
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
                _LogMove = player_move_F;           // 移動キーの入力ログを更新する
            }

            //逆向きのとき
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

    // プレイヤーの移動の状態を返すメソッド
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

    //動きをストップさせるメソッド 引数iはストップさせるフレーム数
    public void _MoveStop(int i)
    {
        _MoveStop_Count_MAX = i;
        _MoveStopF = true;
    }

    public bool _Get_MoveStopF()
    {
        return _MoveStopF;
    }

    // ポーズ画面の処理
    public void _OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }

    // ボタンヒントの表示・非表示を切り替えるメソッド
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
