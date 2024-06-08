using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public Slider Slider;
    public PlayerCombatData CombatData;

    void Start()
    {
        //Setting Mana equal to current Mana and Max Mana equan to Max Mana when starting
        SetMana(CombatData.currentMana);
        SetMaxMana(CombatData.maxMana);
    }

    public void SetMana(int mana){
        Slider.value = mana;
    }

    public void SetMaxMana(int mana){
        Slider.maxValue = mana;
    }

    public void AbsorbMana(int mana) 
    {
        CombatData.currentMana += mana;
        SetMana(CombatData.currentMana);
    }

    public void ConsumeMana(int mana) 
    {
        CombatData.currentMana -= mana;
        SetMana(CombatData.currentMana);
    }

    public bool HasMana(int mana)
    {
        return CombatData.currentMana >= mana;
    }
    
}
