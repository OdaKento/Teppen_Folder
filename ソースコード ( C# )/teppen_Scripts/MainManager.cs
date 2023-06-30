using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //サウンド追加用
    [SerializeField, Header("サウンド管理")] SoundManager soundManager;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip BGM;

    //修正
    [SerializeField] PlayerHP _hp1;
    [SerializeField] PlayerHP _hp2;
    int hp1;
    int hp2;

    [SerializeField] battleTimer _battletimer;

    //
    [SerializeField, Header("シーン管理")] SceneControlManager sceneManager;
    [SerializeField, Header("シーン管理")] private SceneDataBase _sceneDataBase;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("BGMPlay", 3.0f);        //BGMを再生する
        soundManager.PlaySe(clip);      //試合開始時のSEを再生
        //修正
        Invoke("BGMStop()", 110.0f);    //BGMを停止する
        //
    }

    // Update is called once per frame
    void Update()
    {
        //修正
        hp1 = _hp1._getPlayerHP();
        hp2 = _hp2._getPlayerHP();
        if (hp1 <= 0 || hp2 <= 0)///ここにHPが0になった判定を取れれば終わるんだ...!
        {
            BGMStop();
            sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.result, 1.0f);
        }

        if(_battletimer._getTimeOver()) //制限時間が0になったときにシーンを切り替える
		{
            BGMStop(); 
            sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.result, 1.0f);
        }
        //
    }

    //BGM再生用
    void BGMPlay()
    {
        soundManager.PlayBgm(BGM);
    }

    //修正
    //BGM停止用
    void BGMStop()
    {
        soundManager.bgmStop();
        BGM = null;
    }
    //
}
