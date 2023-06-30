using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    //-------------------------------------
    // 制作者：小田健人
    // 画面遷移を行う際は必ず継承させる事
    //-------------------------------------
    [SerializeField] SceneDataBase sceneDataBase;

    public SceneData.SceneMode [] _sceneMode;

    private string _sceneName;
    private bool _sceneChangeF;
    private SoundPlay soundPlay;

    

    // startメソッドの前に呼び出す
    private void Awake()
    {
        GameObject sceneManager = CheckOtherSceneManager();
        bool checkResult = sceneManager != null && sceneManager != gameObject;

        Time.timeScale = 1f;        // 時間の速度を戻す(1.0f)

        if (checkResult)
        {
            Debug.Log("SceneControlManagerは既に存在しています");
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSceneManager()
    {
        return GameObject.FindGameObjectWithTag("SceneManager");
    }

    // シーンデータのシーン名を取得する
    public string getSceneName(SceneData.SceneMode _sceneType)
    {
        SceneData scene = _getSceneData(_sceneType);
        if (scene == null)
        {
            return "";
        }
        return scene.name;
    }

    // シーンデータのシーンのテキストを取得する
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

    // シーンを切り替える関数
    public void changeSceneWithSceneControl(int index,float _Changetime = 0.0f)
    {
        changeScene(_sceneMode[index], _Changetime);
    }

    // 
    private void changeScene(SceneData.SceneMode SceneType, float _scenetime = 0.0f)
    {
        // シーンデータの情報を取得する
        SceneData nextScene = _getSceneData(SceneType);

        // nextScene が null の場合は処理を飛ばす
        if (nextScene == null)
        {
            return;
        }

        // 画面の遷移を行うメソッドを呼び出す
        changeNextScene(nextScene.name, _scenetime);
    }

    // sceneDataのnullチェックを行い値を返すメソッド
    private SceneData _getSceneData(SceneData.SceneMode SceneType)
    {
        // シーンのデータがない場合 null を返す
        if (sceneDataBase == null)
            return null;

        SceneData result = null;

        foreach (var elem in sceneDataBase.SceneBaseList)
        {
            // 配列が一致しない場合処理を飛ばす
            if ((int)(elem.sceneType) != (int)(SceneType))
                continue;

            // 次のシーンの番号を代入する
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

    //ゲーム終了
    private void _EndGame()
    {
        if (_sceneName == sceneDataBase.SceneBaseList[7].sceneName)
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;        //ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
