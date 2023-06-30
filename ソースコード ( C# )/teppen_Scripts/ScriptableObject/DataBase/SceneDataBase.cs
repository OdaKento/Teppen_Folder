using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class SceneDataBase : ScriptableObject
{
    // SceneData をリスト化する
    public List<SceneData> SceneBaseList = new List<SceneData>();
}
