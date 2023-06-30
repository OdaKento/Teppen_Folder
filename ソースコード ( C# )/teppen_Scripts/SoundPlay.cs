using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] SoundManager _soundManager;
    [SerializeField] AudioClip _BGM;
    // Start is called before the first frame update
    void Start()
    {
        _soundManager.bgmStop();
        _soundManager.PlayBgm(_BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
