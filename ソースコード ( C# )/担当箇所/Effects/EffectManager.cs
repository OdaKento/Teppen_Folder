using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField, Header("offsetPos")] private float y;
    // �G�t�F�N�g�𐶐�����
    public void InstantiateEffect(ParticleSystem _particle,Transform _transform)
    {
        // _particle�ɉ����Ȃ��Ƃ��́A�������Ȃ�
        if (_particle == null)
        {
            return;
        }
        Vector3 newPosition = new Vector3(_transform.position.x, _transform.position.y + y, _transform.position.z);
        Quaternion newRotation = Quaternion.Euler(_transform.rotation.x, -_transform.rotation.y, _transform.rotation.z); ;
        //Debug.Log("�G�t�F�N�g�����I�I");
        ParticleSystem _newParticle = Instantiate(_particle, newPosition, newRotation);       // �G�t�F�N�g�𐶐�����
        //_newParticle.transform.position = new Vector3(transform.position.x, transform.position.y + y,transform.position.z);       // �ʒu���X�V����
    }
}
