using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingControl : MonoBehaviour
{
    
    [SerializeField] SceneControlManager sceneManager;

    [SerializeField, Header("�v���C���[�I�u�W�F�N�g")] private playerattack _playerObj;
    [SerializeField, Header("�ޏo�{�^���𒷉������鎞��")]private float pressExitTime;
    [SerializeField, Header("�߂�UI�e�L�X�g")] Text _ExitUI;

    private Color _ExitUIColor;

    private float _exitTime;
    uint _bit;
    private void Start()
    {
        _ExitUIColor = _ExitUI.color;
    }

    private void Update()
    {
        _bit = _playerObj._getButtonBit();
        if ((_bit & 0b0000_0100) == 0b0000_0100)
        {
            _exitTime += Time.deltaTime;
            _ExitUI.color = new Color((1.0f * (_exitTime / pressExitTime)), 0.0f, 0.0f, 1.0f);
        }
        else
        {
            _exitTime = 0.0f;
            _ExitUI.color = _ExitUIColor;
        }

        if(_exitTime > pressExitTime)
        {
            sceneManager.changeSceneWithSceneControl((int)SceneData.SceneMode.modeselect, 0.0f);
        }
    }
}
