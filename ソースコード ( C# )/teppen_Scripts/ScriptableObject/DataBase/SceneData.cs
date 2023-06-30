using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------
// ������F2023/04/30
// ����ҁF���c���l
//
// SceneData�@�Ŋe�V�[�����Ǘ�����
//-------------------------------------------------------

[CreateAssetMenu]
[SerializeField]
public class SceneData : ScriptableObject
{
    public enum SceneMode
    {
        title = 0,          // �^�C�g��
        synopsis,           // ���炷��
        modeselect,         // �Q�[�����[�h�I��
        training,           // �g���[�j���O���[�h�I��
        charaselect,        // �L�����N�^�[�I��
        main,               // �C���Q�[��
        result,             // ���U���g
        exit,               // �����I��
    };

    [SerializeField, Header("�V�[���̎��")] public SceneMode sceneType;              // �V�[���̎��
    [SerializeField, Header("�V�[���̖��O")] public string sceneName;             // �V�[����
    [SerializeField, Header("�V�[���̔ԍ�")] public int sceneNumber;              // �V�[���ԍ�
    [SerializeField, Header("�Q�[�����[�h�e�L�X�g")] public string textName;      // �Q�[�����[�h�ŕ\������e�L�X�g
}




