using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaxGauge : playerattack
{
    private int Waxgauge = 0;
    private int MaxWax = 100;
    private bool WaxAttack = false;
    private bool Brock = false;
    private bool EX = false;
    private Slider Waxslider;


    // Start is called before the first frame update
    void Start()
    {
        //�����ʂ�ǉ�
        Waxgauge = 75;
        //Slider���K��l��
        Waxslider.value = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        //�U��������������
        if (WaxAttack)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            WaxAttack = false;
        }

        //�h�䂵���ꍇ(���g�݂ł�)
        if (Brock)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            Brock = false;
        }

        //�K�E�Z�g�p��(���g�݂ł�)
        if (EX)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            EX = false;
        }

    }

    //�U���̓����蔻��
    private void OnTriggerEnter(Collider other)
    {
        //���̖т��G��Ă��邩
        if (other.gameObject.CompareTag("Hit"))
        {
            //Debug.Log("[1] other.gameObject.tag : " + other.gameObject.tag);

            //�U�����肪�o�Ă��鎞Hit�t���O���グ��
            if (other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._get_HardAttack_CollisionF())
            {
                //Debug.Log("[2] other.gameObject.tag : " + other.gameObject.tag);
                WaxAttack = true;
            }

        }

    }
}
