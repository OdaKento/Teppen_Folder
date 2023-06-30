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
        //‰Šú—Ê‚ğ’Ç‰Á
        Waxgauge = 75;
        //Slider‚ğ‹K’è’l‚É
        Waxslider.value = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        //UŒ‚‚ª“–‚½‚Á‚½‚©
        if (WaxAttack)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            WaxAttack = false;
        }

        //–hŒä‚µ‚½ê‡(‰¼‘g‚İ‚Å‚µ)
        if (Brock)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            Brock = false;
        }

        //•KE‹Zg—p(‰¼‘g‚İ‚Å‚¿)
        if (EX)
        {
            Waxgauge += 10;
            Waxslider.value = (float)Waxgauge / MaxWax;
            Debug.Log("slider.value : " + Waxslider.value);
            EX = false;
        }

    }

    //UŒ‚‚Ì“–‚½‚è”»’è
    private void OnTriggerEnter(Collider other)
    {
        //”¯‚Ì–Ñ‚ªG‚ê‚Ä‚¢‚é‚©
        if (other.gameObject.CompareTag("Hit"))
        {
            //Debug.Log("[1] other.gameObject.tag : " + other.gameObject.tag);

            //UŒ‚”»’è‚ªo‚Ä‚¢‚éHitƒtƒ‰ƒO‚ğã‚°‚é
            if (other.gameObject.transform.root.gameObject.GetComponent<playerattack>()._get_HardAttack_CollisionF())
            {
                //Debug.Log("[2] other.gameObject.tag : " + other.gameObject.tag);
                WaxAttack = true;
            }

        }

    }
}
