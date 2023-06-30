using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    ParticleSystem _particle;

    // Start is called before the first frame update
    void Start()
    {
        // �p�[�e�B�N���̌^��ݒ肷��
        _particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // �p�[�e�B�N���I����ɃI�u�W�F�N�g��j������
        if(_particle.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
