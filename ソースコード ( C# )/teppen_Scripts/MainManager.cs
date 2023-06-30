using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //�T�E���h�ǉ��p
    [SerializeField, Header("�T�E���h�Ǘ�")] SoundManager soundManager;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip BGM;

    //�C��
    [SerializeField] PlayerHP _hp1;
    [SerializeField] PlayerHP _hp2;
    int hp1;
    int hp2;

    [SerializeField] battleTimer _battletimer;

    //
    [SerializeField, Header("�V�[���Ǘ�")] SceneControlManager sceneManager;
    [SerializeField, Header("�V�[���Ǘ�")] private SceneDataBase _sceneDataBase;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("BGMPlay", 3.0f);        //BGM���Đ�����
        soundManager.PlaySe(clip);      //�����J�n����SE���Đ�
        //�C��
        Invoke("BGMStop()", 110.0f);    //BGM���~����
        //
    }

    // Update is called once per frame
    void Update()
    {
        //�C��
        hp1 = _hp1._getPlayerHP();
        hp2 = _hp2._getPlayerHP();
        if (hp1 <= 0 || hp2 <= 0)///������HP��0�ɂȂ������������ΏI����...!
        {
            BGMStop();
            sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.result, 1.0f);
        }

        if(_battletimer._getTimeOver()) //�������Ԃ�0�ɂȂ����Ƃ��ɃV�[����؂�ւ���
		{
            BGMStop(); 
            sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.result, 1.0f);
        }
        //
    }

    //BGM�Đ��p
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
