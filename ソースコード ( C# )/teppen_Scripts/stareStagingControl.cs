using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stareStagingControl : MonoBehaviour
{
    [SerializeField, Header("プレイヤー1のオブジェクト")] playerattack _player1;
    [SerializeField, Header("プレイヤー1のHPオブジェクト")] PlayerHP _player1HP;
    [SerializeField, Header("プレイヤー2のオブジェクト")] playerattack _player2;
    [SerializeField, Header("プレイヤー2のHPオブジェクト")] PlayerHP _player2HP;

    [SerializeField, Header("掴みイベントが起きる時間")] float maxEventTime;

    [SerializeField, Header("イベントタイマーテキストオブジェクト")]Text _stareEventTime;                 // 掴みイベントカウントダウン
    [SerializeField, Header("連打数テキストオブジェクト")] Text[] _stareConsecutiveText = new Text[2];    // 掴みイベント中の連打数

    private float _stareEventTimer;             // 掴みイベント時間を管理する変数

    [SerializeField, Header("イベントタイマーオブジェクト")]GameObject [] _stareStagingObj = new GameObject[2];   // タイマーオブジェクト配列
    private int [] _prevConsecutive = new int[2];                                                                 // 連打数を管理する配列

    [SerializeField, Header("ボタン画像オブジェクト")] Image [] _ButtonImage;

    public bool _HitStareAttack;

    // オブジェクト同士の距離を管理する変数
    float _distance;

    // 掴み合いイベントの発生距離
    [SerializeField, Header("掴み合いイベント発生距離：")] float _stareEventStartdistance;

    // 掴み合いイベントの発生フラグ
    private bool _stareEventF;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log( _player1.gameObject.name + "：" + _player1._getStare() + " " + _player2.gameObject.name + "：" + _player2._getStare() + " " );

        if (_player1 != null && _player2 != null)
        {

            _distance = Mathf.Abs(_player1.transform.position.x - _player2.transform.position.x);

            if (_distance <= _stareEventStartdistance)
            {
                //Debug.Log("掴み合いイベント発生圏内");
                if (_player1._getStare() == true && _player2._getStare() == true)
                {
                    if (!_stareEventF)
                    { _stareEventTimer = maxEventTime; }

                    _stareEventF = true;                                // 掴み合いイベントを開始する
                    LogController._bActive = false;                     // ログを非表示にする
                    Debug.Log("掴み合いイベント：" + _stareEventF);
                }
            }

            // 
            if (_stareEventF)
            {
                for (int i = 0; i < _stareStagingObj.Length; i++)
                {
                    _stareStagingObj[i].SetActive(true);                      // イベントタイマーを表示する
                }
                int _prevTime = (int)_stareEventTimer;          // 
                _stareEventTime.text = _prevTime.ToString();    // テキストを更新する

                if (_player1._getConsecutive() != _prevConsecutive[0])
                {
                    _stareConsecutiveText[0].rectTransform.localScale = new Vector2(1.0f, 2.0f);
                    _prevConsecutive[0] = _player1._getConsecutive();
                    _stareConsecutiveText[0].text = _player1._getConsecutive().ToString();
                }

                if (_player2._getConsecutive() != _prevConsecutive[1])
                {
                    _stareConsecutiveText[1].rectTransform.localScale = new Vector2(1.0f, 2.0f);
                    _prevConsecutive[1] = _player2._getConsecutive();
                    _stareConsecutiveText[1].text = _player2._getConsecutive().ToString();
                }

                for (int i = 0; i < _stareConsecutiveText.Length; i++)
                {
                    if (_stareConsecutiveText[i].fontSize == 120)
                    {
                        _stareConsecutiveText[i].rectTransform.localScale = new Vector2(1.0f, 1.0f);
                    }
                }


                if (_stareEventTimer >= 0.0f)
                {
                    // カウントを減らす処理
                    _stareEventTimer -= Time.deltaTime;
                }
                else
                {
                    _stareEventF = false;
                    if (_player1._getConsecutive() > _player2._getConsecutive())
                    {
                        Debug.Log("player1の勝ち!!!!");
                        _player2HP._setConsecutive(true);
                    }
                    else if (_player1._getConsecutive() < _player2._getConsecutive())
                    {
                        Debug.Log("player2の勝ち!!!!");
                        _player1HP._setConsecutive(true);
                    }
                    else
                    {
                        Debug.Log("引き分け!!!!");
                    }

                }
            }
            else
            {
                for (int i = 0; i < _stareStagingObj.Length; i++)
                {
                    _stareStagingObj[i].SetActive(false);                      // イベントタイマーを表示する
                }
            }
        }
    }

    public bool GetStare() 
    { return _stareEventF; }
        
}
