using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
	[Space(5)]
    public PlayerCombatData CombatData;
    public Animator Animator;
    public PlayerMovement PlayerMovement { get; private set; }
    public Health HealthBar;
    public Mana ManaBar;
    public FollowCamera Camera;
    [Space(10)]


    [Header("AttackPoints")]
	[Space(5)]
    public Transform normalAttackPoint;
    public Transform strongAttackPoint;
    public Transform groundSlamPoint;
    public Transform rangeAttackPoint;
    [Space(10)]


    [Header("Attack Statement")]
	[Space(5)]
    public bool isNormalAttacking;
    public bool isStrongAttacking;
    public bool isGroundSlamAttacking;
    public bool isRangeAttacking;
    [Space(10)]


    [Header("Layers & Tags")]
	[Space(5)]
    public LayerMask enemyLayers;
    [Space(10)]


    float lastAttackTime = 0f;


    private void Awake()
	{
        PlayerMovement = GetComponent<PlayerMovement>();
        Animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && IsCombatSkillUnlocked(CombatData.normalAttackUnlocked) && CanAttack()){
            NormalAttack();
        }

        if(Input.GetKeyDown(KeyCode.Q) && IsCombatSkillUnlocked(CombatData.strongAttackUnlocked) && CanAttack() && ManaBar.HasMana(CombatData.strongAttackCost)){
            StrongAttack();
        }

        if(Input.GetKeyDown(KeyCode.E) && IsCombatSkillUnlocked(CombatData.groundSlamUnlocked) && CanAttack() && ManaBar.HasMana(CombatData.groundSlamCost)){
            GroundSlam();
        }

        if(Input.GetKeyDown(KeyCode.Mouse2) && IsCombatSkillUnlocked(CombatData.rangeAttackUnlocked) && CanAttack() && ManaBar.HasMana(CombatData.rangeAttackCost)){
            RangeAttack();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            HealthBar.TakeDamage(20);
        }

        IsAttacking();
        HealthBar.Die();
    }

    public bool CanAttack()
    {
        return (PlayerMovement.MovementSkillInertia() && !AttackOnCooldown());
    }

    public bool IsAttacking()
    {
        return (isNormalAttacking || isStrongAttacking || isGroundSlamAttacking || isRangeAttacking);
    }

    public bool AttackOnCooldown()
    {
       return (Time.time - lastAttackTime < CombatData.attackCooldown);
    }

    public bool IsCombatSkillUnlocked(bool unlocked)
    {
       return (unlocked);
    }

    void NormalAttack() 
    {   
        lastAttackTime = Time.time;
        CombatData.normalAttackCounter++;

        isNormalAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(normalAttackPoint.position, CombatData.normalAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
            ManaBar.AbsorbMana(CombatData.normalAttackCost);
        }

        StartCoroutine(ResetAttackState(0.5f));
    }

    void StrongAttack() 
    {
        lastAttackTime = Time.time;

        ManaBar.ConsumeMana(CombatData.strongAttackCost);

        isStrongAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(strongAttackPoint.position, CombatData.strongAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
    }

    void GroundSlam() 
    {
        lastAttackTime = Time.time;

        isGroundSlamAttacking = true;

        ManaBar.ConsumeMana(CombatData.groundSlamCost);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(groundSlamPoint.position, CombatData.groundSlamRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length));
    }

    void RangeAttack() 
    {
        lastAttackTime = Time.time;

        ManaBar.ConsumeMana(CombatData.rangeAttackCost);

        isRangeAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rangeAttackPoint.position, CombatData.rangeAttackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {   
            Debug.Log("We hit" + enemy.name);
        }

        StartCoroutine(ResetAttackState(Animator.GetCurrentAnimatorStateInfo(0).length)); 
    }

    private IEnumerator ResetAttackState(float delay)
    {
        // Start a coroutine to reset isAttacking when the animation finishes
        Camera.CameraShake();
        yield return new WaitForSeconds(delay);
        isNormalAttacking = false;
        isStrongAttacking = false;
        isGroundSlamAttacking = false;
        isRangeAttacking = false;
    }

    void OnDrawGizmosSelected() 
    {
        if(normalAttackPoint != null){
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(normalAttackPoint.position, CombatData.normalAttackRange);
        }

        if(strongAttackPoint != null){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(strongAttackPoint.position, CombatData.strongAttackRange);
        }

        if(groundSlamPoint != null){
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundSlamPoint.position, CombatData.groundSlamRange);
        }

        if(rangeAttackPoint != null){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rangeAttackPoint.position, CombatData.rangeAttackRange);
        }
    }
}
