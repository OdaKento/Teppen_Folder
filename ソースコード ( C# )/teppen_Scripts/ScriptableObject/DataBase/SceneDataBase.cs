using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class SceneDataBase : ScriptableObject
{
    // SceneData �����X�g������
    public List<SceneData> SceneBaseList = new List<SceneData>();
}
