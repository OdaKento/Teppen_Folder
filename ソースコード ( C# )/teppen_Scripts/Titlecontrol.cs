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

    //�T�E���h�ǉ��p
    [SerializeField, Header("�T�E���h�Ǘ�")] SoundManager soundManager;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip BGM;

    //�V�[���Ǘ�
    [SerializeField, Header("�V�[���Ǘ�")] SceneControlManager sceneManager;
    [SerializeField] SceneDataBase sceneDataBase;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SEPlay", 1.2f);     //1.2�b���SE���Đ�
        Invoke("BGMPlay", 1.7f);    //1.5�b���BGM���Đ�
    }

    // Update is called once per frame
    void Update()
    {
        if (_pressAnyKeyAction.triggered)
        {
            //Debug.Log("���͂���I�I");

            // ���ʉ�����x�����Đ�����
            if (!_isAudioEnd)
            {
                _isAudioEnd = true;
                //�C��
                BGMStop();  //BGM�̒�~
                //
                sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.modeselect, 1.0f);
            }
        }

    }

    //SE���Đ�
    void SEPlay()
    {
        soundManager.PlaySe(clip);
    }

    //BGM���Đ�
    void BGMPlay()
    {
        soundManager.PlayBgm(BGM);
    }

    //�C��
    //BGM��~�p
    void BGMStop()
    {
        soundManager.bgmStop();
        BGM = null;
    }
    //

}
