using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KOmanager : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField, Header("player1")] GameObject _player1;
    [SerializeField, Header("player2")] GameObject _player2;
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        //Ç«ÇøÇÁÇ©ÇÃÉvÉåÉCÉÑÅ[ÇÃHPÇ™0Ç…Ç»Ç¡ÇΩèÍçá
        if (_player1.GetComponent<PlayerHP>()._getPlayerHP() <= 0 || _player2.GetComponent<PlayerHP>()._getPlayerHP() <= 0)
        {
            GetComponent<Animator>().SetBool("_KOF", true);
            Time.timeScale = 0.1f;
        }

        
    }
}
