using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// 制作日：2023/04/30
// 製作者：小田健人
//
// SceneData　で各シーンを管理する
//-------------------------------------------------------

[CreateAssetMenu]
[SerializeField]
public class SceneData : ScriptableObject
{
    public enum SceneMode
    {
        title = 0,          // タイトル
        synopsis,           // あらすじ
        modeselect,         // ゲームモード選択
        training,           // トレーニングモード選択
        charaselect,        // キャラクター選択
        main,               // インゲーム
        result,             // リザルト
        exit,               // 強制終了
    };

    [SerializeField, Header("シーンの種類")] public SceneMode sceneType;              // シーンの種類
    [SerializeField, Header("シーンの名前")] public string sceneName;             // シーン名
    [SerializeField, Header("シーンの番号")] public int sceneNumber;              // シーン番号
    [SerializeField, Header("ゲームモードテキスト")] public string textName;      // ゲームモードで表示するテキスト
}




