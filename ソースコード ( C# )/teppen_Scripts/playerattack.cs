using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerattack : MonoBehaviour
{
    // ボタン入力を管理する列挙型の変数
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

    private uint _prevbit = 0b_0000;                    // ボタンが押されたかをbitで管理する変数

    protected playercontrol _playercontrol;

    //大攻撃
    public bool _HardAttackF = false;                  //大攻撃のフラグ
    public bool _HardAttack_CollisionF = false;        //大攻撃の当たり判定出現中のフラグ
    private int _HardAttack_Count = 0;                 //大攻撃のカウント
    [SerializeField, Header("大攻撃の判定開始フレーム")] private int _HardAttack_StartCollision;    //大攻撃の当たり判定開始フレーム
    [SerializeField, Header("大攻撃の判定終了フレーム")] private int _HardAttack_FinishCollision;   //大攻撃の当たり判定終了フレーム

    //弱攻撃
    public bool _WeakAttackF = false;                  //弱攻撃のフラグ
    public bool _WeakAttack_CollisionF = false;        //弱攻撃の当たり判定出現中のフラグ
    private int _WeakAttack_Count = 0;                 //弱攻撃のカウント
    [SerializeField, Header("弱攻撃の判定開始フレーム")] private int _WeakAttack_StartCollision;     //弱攻撃の当たり判定開始フレーム
    [SerializeField, Header("弱攻撃の判定終了フレーム")] private int _WeakAttack_FinishCollision;    //弱攻撃の当たり判定終了フレーム

    //必殺技
    public bool _EXAttackF = false;                    //必殺技のフラグ
    public bool _EXAttack_CollisionF = false;          //必殺技の当たり判定出現中のフラグ
    private int _EXAttack_Count = 0;                   //必殺技のカウント
    [SerializeField, Header("必殺技の判定開始フレーム")] private int _EXAttack_StartCollision;    //必殺技の当たり判定開始フレーム
    [SerializeField, Header("必殺技の判定終了フレーム")] private int _EXAttack_FinishCollision;   //必殺技の当たり判定終了フレーム
                                                                                      //必殺技
    public bool _stareF = false;                       //必殺技のフラグ
    private int _stare_Count = 0;                      //必殺技のカウント
    [SerializeField, Header("掴み合いの判定開始フレーム")] private int _stare_StartCollision;    //必殺技の当たり判定開始フレーム
    [SerializeField, Header("掴み合いの判定終了フレーム")] private int _stare_FinishCollision;   //必殺技の当たり判定終了フレーム
    private bool _StareF;
    private bool _simultaneouslyAttackF = false;

    [SerializeField, Header("ガードモーションを起こす当たり判定")] private GameObject _GuardColl;
    [SerializeField, Header("攻撃の当たり判定")] private GameObject _AttackColl;

    // 連打数を管理する変数
    int _consecutiveButtonCount;

    [SerializeField]stareStagingControl _stareControl;                 //掴み合いイベント

    //アニメーター管理
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
        //アニメーター管理
        _Animator.SetBool("_HardAttack",_HardAttackF);
        _Animator.SetBool("_WeakAttack",_WeakAttackF);

        Debug.Log("攻撃状態：" + _WeakAttackF);

        // 掴み合いイベント中ではない場合
        if (!_stareF)
        {
            // 連打数を初期化する
            if(_consecutiveButtonCount != 0)
            { _consecutiveButtonCount = 0; }

            _SimultaneouslyAttack(_prevbit, 0b_1100_0000, "RT,LT", 30);     // rt,ltを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0011_0000, "RB,LB", 30);     // rb,lbを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_0011, "X,Y", 30);       // x,yを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_0110, "Y,B", 30);       // y,bを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_1100, "B,A", 30);       // b,aを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_1001, "A,X", 30);       // a,xを同時押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_0001, "X", 31);         // xを押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_0010, "Y", 11);         // yを押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_0100, "B", 42);         // bを押し
            _SimultaneouslyAttack(_prevbit, 0b_0000_1000, "A", 31);         // aを押し

            if (_prevbit == 0)
            {
                _simultaneouslyAttackF = false;
            }

            //大攻撃の場合
            //大攻撃フラグがtrueの時
            if (_HardAttackF)
            {
                _Attack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _HardAttack_Count = 0;
            }

            //攻撃の当たり判定管理
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

            //弱攻撃の場合
            if (_WeakAttackF)
            {
                _WeakAttack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _WeakAttack_Count = 0;
            }

            //攻撃の当たり判定管理
            if (_WeakAttack_CollisionF)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = true;
                //Debug.Log("弱当たり判定が出た");
            }
            else if (_HardAttack_CollisionF == false && _EXAttack_CollisionF == false && _WeakAttack_CollisionF == false)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = false;
                _GuardColl.GetComponent<BoxCollider>().enabled = false;
                //Debug.Log("弱当たり判定が無くなった");
            }

            if (_playercontrol._Get_MoveStopF() == false)
            {
                _WeakAttackF = false;
            }

            //必殺技の場合
            if (_EXAttackF)
            {
                _EXAttack_Collision_Start();
                _GuardColl.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                _EXAttack_Count = 0;
            }

            //攻撃の当たり判定管理
            if (_EXAttack_CollisionF)
            {
                _AttackColl.GetComponent<BoxCollider>().enabled = true;
                //Debug.Log("EX当たり判定が出た");
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

            //掴みの場合
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
            Debug.Log(this.gameObject.name+ "：" + _consecutiveButtonCount );
        }
    }
    private void FixedUpdate()
    {
        // 掴み合いイベント中ではない場合
        if (!_stareF)
        {
            //大攻撃のフラグがtrueの時
            if (_HardAttackF)
            {
                _HardAttack_Count++;
            }
            if (_HardAttack_CollisionF)
            {
                //Debug.Log("強当たり判定が出た");
            }
            //弱攻撃のフラグがtrueの時
            if (_WeakAttackF)
            {
                _WeakAttack_Count++;
            }

            //必殺技のフラグがtrueの時
            if (_EXAttackF)
            {
                _EXAttack_Count++;
            }

            //掴み合いフラグがtrueの時
            if (_stareF)
            {
                _stare_Count++;
            }
        }
    }

    //攻撃の当たり判定を出現させるフレーム管理
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

    //弱攻撃の当たり判定を出現させるフレーム管理
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

    //必殺技の当たり判定を出現させるフレーム管理
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

    // 連打数を更新するメソッド
    private void _ConsecutiveButton()
    {
        // 掴み合いイベントが発生している場合
        if(_stareControl.GetStare())
        {
            _consecutiveButtonCount++;      // 連打の数を更新する
        }
    }

    //inputのAttackに割り振られているキーを入力されたとき
    public void pressX(InputAction.CallbackContext context)
    {

        // ボタン入力がされた際に、ビットの値( 1 )を更新する
        {
            // 押されたXボタンのフラグをたてる ( 0b0001 )
            if (context.started)
            {
                _prevbit = _prevbit | (uint)Button.x;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // 連打数を検知する
                _ConsecutiveButton();
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.x);
            }
        }
    }

    public void pressY(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 2 )を更新する
        {
            if (context.started)
            {
                // 押されたYボタンのフラグをたてる ( 0b0010 )
                _prevbit = _prevbit | (uint)Button.y;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // 連打数を検知する
                _ConsecutiveButton();
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.y);
            }
        }
    }

    public void pressB(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 4 )を更新する
        {
            if (context.started)
            {
                // 押されたBボタンのフラグをたてる ( 0b0100 )
                _prevbit = _prevbit | (uint)Button.b;
            }
            if (context.canceled)
            {
                // 連打数を検知する
                _ConsecutiveButton();
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.b);
            }
        }
    }

    public void pressA(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 8 )を更新する
        {
            if (context.started)
            {
                // 押されたBボタンのフラグをたてる ( 0b1000 )
                _prevbit = _prevbit | (uint)Button.a;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // 連打数を検知する
                _ConsecutiveButton();
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.a);
            }
        }
    }

    public void pressRB(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 16 )を更新する
        {
            if (context.started)
            {
                // 押されたBボタンのフラグをたてる ( 0b0001_0000 )
                _prevbit = _prevbit | (uint)Button.rb;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.rb);
            }
        }
    }

    public void pressLB(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 32 )を更新する
        {
            if (context.started)
            {
                // 押されたBボタンのフラグをたてる ( 0b0010_0000 )
                _prevbit = _prevbit | (uint)Button.lb;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.lb);
            }
        }
    }

    public void pressRT(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 64 )を更新する
        {
            // 押されたBボタンのフラグをたてる ( 0b0100_0000 )
            if (context.started)
            {
                _prevbit = _prevbit | (uint)Button.rt;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.rt);
            }
        }
    }

    public void pressLT(InputAction.CallbackContext context)
    {
        // ボタン入力がされた際に、ビットの値( 128 )を更新する
        {
            if (context.started)
            {
                // 押されたBボタンのフラグをたてる ( 0b1000_0000 )
                _prevbit = _prevbit | (uint)Button.lt;            // 押されたボタンの値を代入する
            }
            if (context.canceled)
            {
                // ビットを元に戻す
                _setButtonBit(~(uint)Button.lt);
            }
        }
    }

    // 
    private void _SimultaneouslyAttack(uint _inputBit,uint _judgeBit,string _buttonName,int _stopFrame)
    {
        // 同時押しの判定
        // 押し始めたとき
        if ((_inputBit & _judgeBit) == _judgeBit  && _playercontrol._Get_MoveStopF() == false)
        {   
            // 攻撃フラグを管理する処理 (攻撃フラグをここで追加する事で攻撃を開始する)
            {
                //------------------------------------------------
                // ボタン同時押しの場合の処理
                //------------------------------------------------
                // RT,LTボタンが入力した
                if (_inputBit == 0b1100_0000)
                {  }

                // RB,LBボタンが入力された時
                else if (_inputBit == 0b0011_0000)
                { _EXAttackF = true; }

                // X,Aボタンが入力された時
                else if (_inputBit == 0b0000_1001)
                {
                    Debug.Log("投げ技");
                    _stareF = true; }

                //------------------------------------------------
                // ボタン単体押しの場合の処理
                //------------------------------------------------
                // Xボタンが入力された時
                else if (_inputBit == 0b0000_0001)
                { _HardAttackF = true; }

                // Yボタンが入力された時
                else if (_inputBit == 0b0000_0010)
                { _WeakAttackF = true; }
                
                // Bボタンが入力された時
                else if (_inputBit == 0b0000_0100)
                {  }
                
                // Aボタンが入力された時
                else if (_inputBit == 0b0000_1000)
                {  }
            }

            // 一度だけ入力を受け付ける
            if (!_simultaneouslyAttackF)
            {
                _simultaneouslyAttackF = true;
                // Debug.Log(_buttonName + "を押ししました");
                _playercontrol._MoveStop(_stopFrame);
                _playercontrol._changeMove(8);
            }
        }
    }

    // ボタンのビットを取得するメソッド
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
