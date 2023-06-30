using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Header("カメラ移動速度")] float _cameraMove;

    [SerializeField, Header("ゲームモード教室地点")] GameObject[] GamemodeClassroom;

    [SerializeField, Header("テキストオブジェクト")] Text[] _textObject;

    [SerializeField, Header("テキストオブジェクト_筆")] GameObject[] _anim;

    [SerializeField, Header("シーンの数")] SceneData.SceneMode[] _scene;

    [SerializeField, Header("SceneManager")] SceneControlManager _sceneManager;

    [SerializeField] SoundManager soundManager;


    private bool _isAudioEnd;

    private Color _prevcolor;

    bool bSelect = false;       // モードの選択可能かを管理するフラグ


    Vector3 distanceMove;   // 教室間での距離

    int maxNumber;          // 教室の最大数
    int minNumber;          // 教室の最小数

    int prevSceneNumber;    // シーンごとの配列の要素数を管理する変数
    private int modeNumber; // 

    int _prevtextSize;      // テキストフォントサイズ

    float move;

    int changeCount = 0;
    bool changeF = false;

    // Start is called before the first frame update
    void Start()
    {
        
        bSelect = false;

        for (int i = 0; i < _textObject.Length; i++)
        {
            _textObject[i].text = _sceneManager.getSceneTextName(_scene[i]);
        }

        _prevtextSize = _textObject[0].fontSize;            // Fontsizeを一時的に保存する
        _prevcolor = _textObject[0].color;



        minNumber = modeNumber;                             // minNumberの初期化をする
        maxNumber = GamemodeClassroom.Length;               // maxNumberを初期化をする
        Debug.Log(maxNumber);
    }

    // Update is called once per frame
    void Update()
    {
        // 教室の場所を更新する処理
        _targetRoom();

        // カメラの座標を更新する処理
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (distanceMove.z * Time.deltaTime));

        if (_isAudioEnd)
        {
            _isAudioEnd = false;

        }
    }

    private void FixedUpdate()
    {
        if (changeF)
        {
            changeCount++;
            if (changeCount > 60)
            {
                _sceneManager.changeSceneWithSceneControl(modeNumber, 1.0f);
            }
        }
    }


    // カメラが選択したゲームモードに移動するメソッド
    void _targetRoom()
    {
        for (int i = 0; i < GamemodeClassroom.Length; i++)
        {
            // 教室の番号を一致した場合、移動量を更新する
            if (i == modeNumber)
            {
                distanceMove = GamemodeClassroom[i].transform.position - this.transform.position;
                _textObject[i].fontSize = 100;
                _textObject[i].color = _prevcolor;
                // Debug.Log(distanceMove);

                // カメラのＺ軸の移動量を絶対値にして保存する
                move = Mathf.Abs(distanceMove.z);
            }
            else
            {
                // テキストのサイズを戻す
                _textObject[i].fontSize = _prevtextSize;
                _textObject[i].color = new Color(_prevcolor.r, _prevcolor.g, _prevcolor.b, 0.25f);
            }
        }
    }

    // 選択肢を増やす処理
    public void _OnCountUp(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            Debug.Log("カウントアップ");
            modeNumber++;
        }

        if (modeNumber >= maxNumber)
        {
            modeNumber = minNumber;
        }
    }

    // 選択肢を減らす処理
    public void _OnCountDown(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            Debug.Log("カウントダウン");
            modeNumber--;
        }

        if (modeNumber < minNumber)
        {
            modeNumber = maxNumber - 1;
        }
    }

    // 選択肢を決定する処理
    public void _OnApply(InputAction.CallbackContext context)
    {
        if (context.started && !bSelect)
        {
            bSelect = true;
            changeF = true;
            // 効果音を一度だけ再生する
            if (!_isAudioEnd)
            {
                soundManager.bgmStop();
                _isAudioEnd = true;    // 
                _anim[modeNumber].GetComponent<Image>().enabled = true;
                _anim[modeNumber].GetComponent<Animator>().SetBool("hudeanim", _isAudioEnd);

            }
            prevSceneNumber = modeNumber;       //                                        // 
        }

    }
}
