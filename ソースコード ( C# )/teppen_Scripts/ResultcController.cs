using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResultcController : MonoBehaviour
{

    // Inspector上で「文章と変数」を表示する
    [SerializeField,Header("選択肢のオブジェクト")]GameObject[] seleteObj;
    [SerializeField, Header("選択肢のイメージオブジェクト")] Image[] _image;

    [SerializeField] SceneDataBase sceneDataBase;
    [SerializeField] SceneControlManager sceneControlManager;
    [SerializeField] SceneData.SceneMode[] _nextScene;

    private int _maxSelect;             // 選択肢の最大数を管理する変数
    private int _minSelect=0;           // 選択肢の最小値を管理する変数

    Color _prevColor;                   // 起動時の色情報を管理する変数
    [SerializeField] Color _color;      // 色情報を管理する変数

    private bool _audioFlag;

    private int iNum;

    private void Start()
    {
        _maxSelect = _image.Length;
        iNum = _minSelect;
    }

    private void Update()
    {
        changeColor();
    }

    void changeColor()
    {
        for(int i = 0; i < _maxSelect;i++)
        {
            if(i == iNum)
            {
                _image[i].color = _color;
            }
            else
            {
                _image[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    // 選択番号を増やすメソッド 「 PlayerInput：DefalutMap(Select)で設定可能」
    public void _OnCountUp(InputAction.CallbackContext context)
    {
        // ボタンを押した瞬間のみ処理を行う
        if (context.started)
        {
            iNum++;
            // 選択している番号を増やす
            if(iNum > _maxSelect-1)
            { iNum = _minSelect; }
        }
    }

    // 選択番号を減らすメソッド 「 PlayerInput：DefalutMap(Select)で設定可能」
    public void _OnCountDown(InputAction.CallbackContext context)
    {
        // ボタンを押した瞬間のみ処理を行う
        if (context.started)
        {
            iNum--;
            // 選択している番号を増やす
            if (iNum < _minSelect)
            { iNum = _maxSelect - 1 ; }
        }
    }

    // 決定ボタンが押された時の処理メソッド 「 PlayerInput：DefalutMap(Select)で設定可能」
    public void _OnApply(InputAction.CallbackContext context)
    {   
        // 決定ボタンが押された瞬間に シーンを切り替え、効果音を鳴らす
        if(context.started && !_audioFlag)
        {
            sceneControlManager.changeSceneWithSceneControl(iNum, 0.0f);
            _audioFlag = true;                          // 効果音を鳴らすフラグを ON にする
        }
    }

}
