using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateValue(int health, int maxHealth)
    {
        slider.value = (float)health / (float)maxHealth;
    }

    public void hide()
    {
        slider.gameObject.SetActive(false);
    }
}
