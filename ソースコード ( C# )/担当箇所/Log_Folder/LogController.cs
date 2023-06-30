using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField, Header("�v���C���[1��UI�I�u�W�F�N�g")] private GameObject _player1UI;      //
    [SerializeField, Header("�v���C���[2��UI�I�u�W�F�N�g")] private GameObject _player2UI;      //

    [SerializeField, Header("�v���C���[1�̃I�u�W�F�N�g")] private playercontrol _playerObj1;
    [SerializeField, Header("�v���C���[2�̃I�u�W�F�N�g")] private playercontrol _playerObj2;

    [SerializeField] private  bool _HintActive;
    public static  bool _bActive;



    // Start is called before the first frame update
    void Start()
    {
        _playerObj1 = _playerObj1.GetComponent<playercontrol>();
        _playerObj2 = _playerObj2.GetComponent<playercontrol>();

    }

    // Update is called once per frame
    void Update()
    {
        Active(_player1UI, _bActive);
        Active(_player2UI, _bActive);

        _HintActive = _bActive;
    }


    // �{�^���q���g�̕\���E��\�����s��
    private void Active(GameObject _object,bool _active)
    {
        _object.SetActive(_active);
    }
}
