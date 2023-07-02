using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image barImage;
    public GameObject Player;
    private float mana;

    void Awake()
    {
        barImage=transform.gameObject.GetComponent<Image>();
        ;
    }

    // Update is called once per frame
    void Update()
    {
        barImage.fillAmount=GetManaNormalized();
    }
    public float GetManaNormalized()
    {
        return Player.GetComponent<PlayerMovement>().mana/Player.GetComponent<PlayerMovement>().maxmana;
    }
}