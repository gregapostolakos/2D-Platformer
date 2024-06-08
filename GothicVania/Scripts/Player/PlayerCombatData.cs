using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatData : MonoBehaviour
{
	[Header("Health & Mana")]
	[Space(10)]
	public int maxHealth = 100;
	public int currentHealth;
	[Space(5)]
    public int maxMana = 80;
	public int currentMana = 20;
	[Space(5)]
	public bool isAlive = true;
    [Space(20)]


    [Header("Skills Damage")]
	[Space(10)]
    public int normalAttackDamage = 10;
    public int groundSlamDamage = 40;
    public int strongAttackDamage = 30;
    public int rangeAttackDamage = 20;
    [Space(20)]


    [Header("Skills Mana Cost")]
	[Space(10)]
    public int normalAttackCost = 10;
    public int groundSlamCost = 40;
    public int strongAttackCost = 30;
    public int rangeAttackCost = 20;
    [Space(20)]


    [Header("Skills Damage Range")]
	[Space(10)]
    public float normalAttackRange = 0.5f;
    public float rangeAttackRange = 0.5f;
    public float groundSlamRange = 0.5f;
    public float strongAttackRange = 0.5f;
    [Space(20)]


    [Header("Timers & Counters")]
	[Space(10)]
    public int normalAttackCounter = 0;
    public float attackCooldown = 1f;

}