using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider Slider;
    public PlayerCombatData CombatData;

    void Start()
    {
        //Setting Health and Max Health equal to Max Health when starting
        CombatData.currentHealth = CombatData.maxHealth;
        SetMaxHealth(CombatData.maxHealth);
    }

    public void SetHealth(int health){
        Slider.value = health;
    }

    public void SetMaxHealth(int health){
        Slider.value = health;
        Slider.maxValue = health;
    }

    public void TakeDamage(int damage) 
    {
        CombatData.currentHealth -= damage;
        SetHealth(CombatData.currentHealth);
    }

    public void Die (){
        if (CombatData.currentHealth <= 0){
            CombatData.isAlive = false;
        }
    }
}
