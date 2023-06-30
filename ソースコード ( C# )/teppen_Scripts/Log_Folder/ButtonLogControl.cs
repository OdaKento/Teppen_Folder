using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------
// 内容：ボタン入力のログを管理する
// 製作者：21cu0206_小田健人
//-------------------------------------------------------------
public class ButtonLogControl : MonoBehaviour
{
    [SerializeField,Header("プレイヤーオブジェクト")]playerattack _playerAttack;                // 
    uint _prevBit;

    [SerializeField, Header("ボタンの画像オブジェクト")] Image[] _buttonIcons;                  // ボタンの画像を管理する変数

    private Vector2 defalutIconScale;

    // Start is called before the first frame update
    void Start()
    {
        defalutIconScale =new Vector2( _buttonIcons[0].rectTransform.localScale.x, _buttonIcons[0].rectTransform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        // ボタンが押された際の更新処理
        _prevBit = _playerAttack._getButtonBit();

        _ActionButton(_prevBit, 0b0001, 0 );     // Xボタンの入力を受け付ける
        _ActionButton(_prevBit, 0b0010, 1 );     // Yボタンの入力を受け付ける
        _ActionButton(_prevBit, 0b0100, 2 );     // Bボタンの入力を受け付ける
        _ActionButton(_prevBit, 0b1000, 3 );     // Aボタンの入力を受け付ける

    }

    // ボタンのUIの大きさを管理するメソッド
    private void _ActionButton(uint _bit,uint _compainBit,int colorNum)
	{
        
        // ビットと一致した時にボタンの色を変更する
        if ((_bit & _compainBit)  == _compainBit)
        {
            // ボタンのアイコンの大きさを変更する
            _buttonIcons[colorNum].rectTransform.localScale = new Vector2(defalutIconScale.x * 0.25f, defalutIconScale.y * 0.25f);          
        }
        else 
        {
            // ボタンのアイコンの大きさを元に戻す
            _buttonIcons[colorNum].rectTransform.localScale = new Vector2(defalutIconScale.x, defalutIconScale.y);                          
        }

        
    }
}
