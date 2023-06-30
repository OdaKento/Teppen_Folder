using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [SerializeField, Header("プレイヤー1のUIオブジェクト")] private GameObject _player1UI;      //
    [SerializeField, Header("プレイヤー2のUIオブジェクト")] private GameObject _player2UI;      //

    [SerializeField, Header("プレイヤー1のオブジェクト")] private playercontrol _playerObj1;
    [SerializeField, Header("プレイヤー2のオブジェクト")] private playercontrol _playerObj2;

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


    // ボタンヒントの表示・非表示を行う
    private void Active(GameObject _object,bool _active)
    {
        _object.SetActive(_active);
    }
}
