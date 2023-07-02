using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image barImage;
    public GameObject Player;

    void Awake()
    {
        barImage=transform.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        barImage.fillAmount=GetHPNormalized();
    }
    public float GetHPNormalized()
    {
        return Player.GetComponent<PlayerMovement>().health/Player.GetComponent<PlayerMovement>().maxHealth;
    }
}