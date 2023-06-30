using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject Target1;
    public GameObject Target2;

    Vector3 CenterPos;      //プレイヤー間の中央座標
    public float OffcetY = 4;
    public float ratio;

    private void Start()
    {
        
    }
    private void Update()
    {
        CenterPos = (Target1.transform.position + Target2.transform.position) / 2;
        ratio = Vector3.Distance(Target1.transform.position, Target2.transform.position) / 10;
        transform.position = new Vector3(CenterPos.x, CenterPos.y + OffcetY, -ratio * ratio);
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour {

public GameObject Target1;
public GameObject Target2;

Vector3 CenterPos;
public float OffcetY = 4;
public float ratio;

void Update ()
{
CenterPos = (Target1.transform.position + Target2.transform.position) / 2;
ratio = Vector3.Distance(Target1.transform.position, Target2.transform.position) /10;
transform.position = new Vector3(CenterPos.x, CenterPos.y + OffcetY, -ratio * ratio);
}
} 
 */