using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    //-------------------------------------
    // ����ҁF���c���l
    // ��ʑJ�ڂ��s���ۂ͕K���p�������鎖
    //-------------------------------------
    [SerializeField] SceneDataBase sceneDataBase;

    public SceneData.SceneMode [] _sceneMode;

    private string _sceneName;
    private bool _sceneChangeF;
    private SoundPlay soundPlay;

    

    // start���\�b�h�̑O�ɌĂяo��
    private void Awake()
    {
        GameObject sceneManager = CheckOtherSceneManager();
        bool checkResult = sceneManager != null && sceneManager != gameObject;

        Time.timeScale = 1f;        // ���Ԃ̑��x��߂�(1.0f)

        if (checkResult)
        {
            Debug.Log("SceneControlManager�͊��ɑ��݂��Ă��܂�");
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSceneManager()
    {
        return GameObject.FindGameObjectWithTag("SceneManager");
    }

    // �V�[���f�[�^�̃V�[�������擾����
    public string getSceneName(SceneData.SceneMode _sceneType)
    {
        SceneData scene = _getSceneData(_sceneType);
        if (scene == null)
        {
            return "";
        }
        return scene.name;
    }

    // �V�[���f�[�^�̃V�[���̃e�L�X�g���擾����
    public string getSceneTextName(SceneData.SceneMode SceneType)
    {
        SceneData scene = _getSceneData(SceneType);
        if (scene == null)
        {
            return "";
        }
        return scene.textName;
    }

    // 
    private void changeNextScene(string _scenename,float _scenetime = 0.0f)
    {
        _sceneName = _scenename;
        Invoke("SceneLoad", _scenetime);
    }

    // �V�[����؂�ւ���֐�
    public void changeSceneWithSceneControl(int index,float _Changetime = 0.0f)
    {
        changeScene(_sceneMode[index], _Changetime);
    }

    // 
    private void changeScene(SceneData.SceneMode SceneType, float _scenetime = 0.0f)
    {
        // �V�[���f�[�^�̏����擾����
        SceneData nextScene = _getSceneData(SceneType);

        // nextScene �� null �̏ꍇ�͏������΂�
        if (nextScene == null)
        {
            return;
        }

        // ��ʂ̑J�ڂ��s�����\�b�h���Ăяo��
        changeNextScene(nextScene.name, _scenetime);
    }

    // sceneData��null�`�F�b�N���s���l��Ԃ����\�b�h
    private SceneData _getSceneData(SceneData.SceneMode SceneType)
    {
        // �V�[���̃f�[�^���Ȃ��ꍇ null ��Ԃ�
        if (sceneDataBase == null)
            return null;

        SceneData result = null;

        foreach (var elem in sceneDataBase.SceneBaseList)
        {
            // �z�񂪈�v���Ȃ��ꍇ�������΂�
            if ((int)(elem.sceneType) != (int)(SceneType))
                continue;

            // ���̃V�[���̔ԍ���������
            result = elem;
            break;
        }

        return result;

    }


    private void SceneLoad()
    {
        _EndGame();

        if (!_sceneChangeF)
        {
            _sceneChangeF = true;
            SceneManager.LoadScene(_sceneName);
        }
    }

    //�Q�[���I��
    private void _EndGame()
    {
        if (_sceneName == sceneDataBase.SceneBaseList[7].sceneName)
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;        //�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }
}
