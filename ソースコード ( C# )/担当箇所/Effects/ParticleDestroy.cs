using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    ParticleSystem _particle;

    // Start is called before the first frame update
    void Start()
    {
        // パーティクルの型を設定する
        _particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // パーティクル終了後にオブジェクトを破棄する
        if(_particle.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
