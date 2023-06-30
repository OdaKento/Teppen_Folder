using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------
// 内容：インゲーム( 本戦 )画面の制限時間を実装
// 製作者： 小田健人
//-----------------------------------------------------------

public class battleTimer : MonoBehaviour
{
    [SerializeField, Header("制限時間")] private float _gameTime;
    [SerializeField, Header("テキストオブジェクト")] private Text _textObject;
    [SerializeField, Header("テキストの色(制限時間残り僅かの場合)")] private Color _textColor;

    [SerializeField, Header("プレイヤー1のHPオブジェクト")] PlayerHP _player1HP;
    [SerializeField, Header("プレイヤー2のHPオブジェクト")] PlayerHP _player2HP;

    [SerializeField, Header("サウンド管理")] SoundManager soundManager;
    [SerializeField] AudioClip clip;

    private float _prevTimer;               // 制限時間をスクリプト側で管理する変数
    private float _frameTime;               // フレーム数を管理する変数

    public bool _GameEnd;
    private bool _StopTimer;                // カウントダウンを停止する処理

    private bool _Tstart = false;

    // Start is called before the first frame update
    void Start()
    {
        _prevTimer = _gameTime;             // 制限時間を設定する
        _GameEnd = false;
        _StopTimer = false;                 // 
        soundManager.PlaySe(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player1HP._PlayerHp <= 0 || _player2HP._PlayerHp <= 0)
        {
            _StopTimer = true;
        }

        // _prevTimer が 0 以上の場合のみ値を減らす
        if (_prevTimer >= 0)
        {
            if (!_Tstart) { Invoke("StartTimer", 3.0f); }

            if (!_StopTimer && _Tstart)
            {
                // 時間を減らす                

                _prevTimer -= Time.deltaTime;

                if (_prevTimer <= 10.0f)
                {
                    _textObject.color = _textColor;
                    // _textObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }

                // _gameTime に _prevTimer の整数値を代入する
                _gameTime = (int)_prevTimer;

                // テキストオブジェクトで _gameTime の値を表示させる
                _textObject.text = _gameTime.ToString();
            }
        }
        else
        {
            // 試合終了関数を一度だけ呼び出す
            if (!_GameEnd)
            {
                _GameEnd = true;
                
                Debug.Log("終了！");
            }
        }
    }

    // 画面を切り替える
    public bool _getTimeOver()
    {
        return _GameEnd;
    }

    void StartTimer()
    {
        _Tstart = true;
    }
}
