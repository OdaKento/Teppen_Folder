using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Titlecontrol : MonoBehaviour
{
    private InputAction _pressAnyKeyAction =
                new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();

    bool _isAudioEnd;

    //サウンド追加用
    [SerializeField, Header("サウンド管理")] SoundManager soundManager;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip BGM;

    //シーン管理
    [SerializeField, Header("シーン管理")] SceneControlManager sceneManager;
    [SerializeField] SceneDataBase sceneDataBase;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SEPlay", 1.2f);     //1.2秒後にSEを再生
        Invoke("BGMPlay", 1.7f);    //1.5秒後にBGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        if (_pressAnyKeyAction.triggered)
        {
            //Debug.Log("入力あり！！");

            // 効果音を一度だけ再生する
            if (!_isAudioEnd)
            {
                _isAudioEnd = true;
                //修正
                BGMStop();  //BGMの停止
                //
                sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.modeselect, 1.0f);
            }
        }

    }

    //SEを再生
    void SEPlay()
    {
        soundManager.PlaySe(clip);
    }

    //BGMを再生
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
